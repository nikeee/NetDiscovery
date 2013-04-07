using System;
using System.Net;
using System.Net.Sockets;

namespace NetDiscovery
{
    class Discoverable : INetDiscoverable
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

        public void Listen()
        {
            _cancelListening = false;

            var clientEndpoint = new IPEndPoint(SourceAddress, _port);
            _client.Client.Bind(clientEndpoint);

            while (!_cancelListening)
            {
                var requestPacketData = _client.Receive(ref clientEndpoint);

                var packet = PacketHandler.GetPacketInstance(requestPacketData);
                if (packet == null)
                    continue;

                var response = CreateResponsePacket();
                _client.Send(response, response.Length, clientEndpoint);
            }
        }

        private byte[] _offeredEndpointData = null;
        private byte[] CreateResponsePacket()
        {
            return _offeredEndpointData ?? (_offeredEndpointData = _offeredEndpoint.SerializeToBytes());
        }
    }
}