using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetDiscovery
{
    class Discoverer : INetDiscoverer
    {
        private static readonly IPAddress SourceAddress = IPAddress.Any;
        private static readonly IPAddress DestinationAddress = IPAddress.Broadcast;

        private readonly UdpClient _client = new UdpClient();
        private readonly int _port;

        public Discoverer(int port)
        {
            _port = port;
        }
        public IPEndPoint Discover()
        {
            var packet = CreateRequestPacket();
            _client.Send(packet, packet.Length, new IPEndPoint(DestinationAddress, _port));
            
            var serverEndpoint = new IPEndPoint(SourceAddress, _port);
            var serverResponse = _client.Receive(ref serverEndpoint);

            return serverResponse.DeserializeFromBytes<IPEndPoint>();
        }

        private static byte[] CreateRequestPacket()
        {
            return PacketHandler.CreateData(new EndpointRequestPacket());
        }
    }
}