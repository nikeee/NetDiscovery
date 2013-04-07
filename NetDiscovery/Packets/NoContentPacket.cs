namespace NetDiscovery.Packets
{
    abstract class NoContentPacket : IPacket
    {
        public abstract PacketIds Id { get; }
        public byte[] GetContent()
        {
            return new byte[0];
        }
    }
}