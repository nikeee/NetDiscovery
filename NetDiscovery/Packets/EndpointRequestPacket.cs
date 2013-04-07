namespace NetDiscovery.Packets
{
    class EndpointRequestPacket : NoContentPacket
    {
        public override PacketIds Id { get { return PacketIds.EndpointRequest; } }
        public EndpointRequestPacket(byte[] content)
            : base()
        { }
        public EndpointRequestPacket()
            : base()
        { }
    }
}