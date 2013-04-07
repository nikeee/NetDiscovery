using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetDiscovery
{

    class DiscoveryResult
    {
        public bool Error { get; private set; }
        public bool Canceled { get; private set; }
        public bool NoEndPointAvailable { get; private set; }
        public Exception Exception { get; private set; }
        public IPEndPoint OfferedEndPoint { get; private set; }

        public DiscoveryResult(bool error, bool canceled, bool noEndPointAvailable, Exception exception, IPEndPoint endPoint)
        {
            Error = error;
            Canceled = canceled;
            Exception = exception;
            NoEndPointAvailable = noEndPointAvailable;
            OfferedEndPoint = endPoint;
        }
        public DiscoveryResult(bool error, Exception exception)
            : this(error, false, false, exception, null)
        { }
        public DiscoveryResult(IPEndPoint endpoint)
            : this(false, false, false, null, endpoint)
        { }

        public DiscoveryResult(bool canceled)
            : this(false, canceled, false, null, null)
        { }

        public readonly static DiscoveryResult NoEndpointAvailableResult = new DiscoveryResult(false, false, true, null,null);

    }

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

        private bool _cancelDiscovering;
        public void Cancel()
        {
            _cancelDiscovering = true;
        }

        public DiscoveryResult Discover()
        {
            var packet = CreateRequestPacket();
            while (!_cancelDiscovering)
            {
                _client.Send(packet, packet.Length, new IPEndPoint(DestinationAddress, _port));

                var serverEndpoint = new IPEndPoint(SourceAddress, _port);
                var serverResponsePacket = _client.Receive(ref serverEndpoint);

                var p = PacketHandler.GetPacketInstance(serverResponsePacket);
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