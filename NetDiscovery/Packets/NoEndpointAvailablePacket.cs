namespace NetDiscovery.Packets
{
    class NoEndpointAvailablePacket : NoContentPacket
    {
        public override PacketIds Id { get { return PacketIds.NoEndpointAvailable; } }
        public NoEndpointAvailablePacket(byte[] content)
            : base()
        { }
        public NoEndpointAvailablePacket()
            : base()
        { }
    }
}