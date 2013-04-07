namespace NetDiscovery.Packets
{
    interface IPacket
    {
        PacketIds Id { get; }

        byte[] GetContent();
    }
}