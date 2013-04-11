using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NetDiscovery.Packets;

namespace NetDiscovery
{
    public class Discoverable : INetDiscoverable
    {
        private static readonly IPAddress SourceAddress = IPAddress.Any;

        private readonly UdpClient _client = new UdpClient();
        private readonly int _port;

        private readonly IPEndPoint _offeredEndpoint;

        public Discoverable(int port, IPEndPoint offeredEndpoint)
        {
            if (offeredEndpoint == null)
                throw new ArgumentNullException("offeredEndpoint");
            _port = port;
            _offeredEndpoint = offeredEndpoint;
        }

        public IPEndPoint OfferedEndpoint { get { return _offeredEndpoint; } }

        private bool _cancelListening;
        public void Cancel()
        {
            _cancelListening = true;
        }

        private async void SendOfferEndpointPacket(IPEndPoint clientEndpoint)
        {
            var response = CreateResponsePacket();
            var data = PacketHandler.CreateData(response);
            await _client.SendAsync(data, data.Length, clientEndpoint);
        }

        public void Listen()
        {
            _cancelListening = false;

            var clientEndpoint = new IPEndPoint(SourceAddress, _port);
            _client.Client.Bind(clientEndpoint);

            IPEndPoint remoteEndPoint = null;

            while (!_cancelListening)
            {
                var requestPacketData = _client.Receive(ref remoteEndPoint);

                var packet = PacketHandler.GetPacketInstance(requestPacketData);
                if (packet == null)
                    continue;

                SendOfferEndpointPacket(remoteEndPoint);
            }
        }

        public async void ListenAsync()
        {
            _cancelListening = false;

            var clientEndpoint = new IPEndPoint(SourceAddress, _port);
            _client.Client.Bind(clientEndpoint);

            while (!_cancelListening)
            {
                var updResult = await _client.ReceiveAsync();
                
                var packet = PacketHandler.GetPacketInstance(updResult.Buffer);
                if (packet == null)
                    continue;

                SendOfferEndpointPacket(updResult.RemoteEndPoint);
            }
        }

        private IPacket _offeredEndpointPacket;
        private IPacket CreateResponsePacket()
        {
            return _offeredEndpointPacket ?? (_offeredEndpointPacket = new OfferEndPointPacket(_offeredEndpoint));
        }
    }
}