using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetDiscovery.Packets;

namespace NetDiscovery
{
    public class Discoverer : INetDiscoverer
    {
        private static readonly IPAddress SourceAddress = IPAddress.Any;
        private static readonly IPAddress DestinationAddress = IPAddress.Broadcast;

        private readonly UdpClient _client = new UdpClient();
        private readonly int _port;

        public Discoverer(int port)
        {
            _port = port;
        }

        private bool _cancelDiscovering;
        public void Cancel()
        {
            _cancelDiscovering = true;
        }
        
        public DiscoveryResult Discover()
        {
            var packetData = CreateRequestPacket();
            while (!_cancelDiscovering)
            {
                _client.Send(packetData, packetData.Length, new IPEndPoint(DestinationAddress, _port));
                
                IPEndPoint resEp = null;
                var buffer = _client.Receive(ref resEp);

                var p = PacketHandler.GetPacketInstance(buffer);
                if (p == null)
                    continue;

                switch (p.Id)
                {
                    case PacketIds.NoEndpointAvailable:
                        return DiscoveryResult.NoEndpointAvailableResult;
                    case PacketIds.OfferEndPoint:
                        return new DiscoveryResult(((OfferEndPointPacket)p).OfferedEndPoint);
                    default:
                        continue;
                }
            }
            return new DiscoveryResult(true);
        }

        public async Task<DiscoveryResult> DiscoverAsync()
        {
            var packetData = CreateRequestPacket();
            while (!_cancelDiscovering)
            {
                await _client.SendAsync(packetData, packetData.Length, new IPEndPoint(DestinationAddress, _port));

                UdpReceiveResult res = await _client.ReceiveAsync();
                
                var p = PacketHandler.GetPacketInstance(res.Buffer);
                if (p == null)
                    continue;

                switch (p.Id)
                {
                    case PacketIds.NoEndpointAvailable:
                        return DiscoveryResult.NoEndpointAvailableResult;
                    case PacketIds.OfferEndPoint:
                        return new DiscoveryResult(((OfferEndPointPacket)p).OfferedEndPoint);
                    default:
                        continue;
                }
            }
            return new DiscoveryResult(true);
        }

        private static byte[] CreateRequestPacket()
        {
            return PacketHandler.CreateData(new EndpointRequestPacket());
        }
    }
}