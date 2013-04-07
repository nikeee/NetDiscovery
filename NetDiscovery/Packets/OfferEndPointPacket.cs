using System.Net;

namespace NetDiscovery.Packets
{
    class OfferEndPointPacket : IPacket
    {
        public PacketIds Id { get { return PacketIds.OfferEndPoint; } }
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
        public OfferEndPointPacket()
        { }
    }
}