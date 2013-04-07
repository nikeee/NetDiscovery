using System.Net;

namespace NetDiscovery
{
    interface INetDiscoverable
    {
        IPEndPoint OfferedEndpoint { get; }
        void Listen();
    }

    interface INetDiscoverer
    {
        IPEndPoint Discover();
    }

    /*

    byte[16] checksum; //MD5 // 16 byte -> better use CRC32
    byte packetId; // 1 byte
    int contentLength; // 4 byte
    byte[contentLength] content; // contentLength byte

    */

    interface IPacket
    {
        byte Id { get; }

        byte[] GetContent();
    }

    struct EndpointRequestPacket : IPacket
    {
        public byte Id { get { return 0; } }

        public byte[] GetContent()
        {
            return new byte[0];
        }

        public EndpointRequestPacket(byte[] content)
        { }
    }

    struct OfferEndPointPacket : IPacket
    {
        public byte Id {get { return 1; }}
        public readonly IPEndPoint OfferedEndPoint;

        public byte[] GetContent()
        {
            var ep = new IPEndPoint(0, 0);
            if (OfferedEndPoint != null)
                ep = OfferedEndPoint;
            return ep.SerializeToBytes();
        }

        public OfferEndPointPacket(byte[] content)
        {
            OfferedEndPoint = content.DeserializeFromBytes<IPEndPoint>();
        }
    }

    struct NoEndpointAvailablePacket : IPacket
    {
        public byte Id { get { return 2; } }

        public byte[] GetContent()
        {
            return new byte[0];
        }

        public NoEndpointAvailablePacket(byte[] content)
        { }
    }
}