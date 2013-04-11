using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using NetDiscovery.Packets;

// May not work (hangover while coding)

namespace NetDiscovery
{
    internal static class PacketHandler
    {
        private static readonly List<IPacket> RegisteredPackets = new List<IPacket>() { new OfferEndPointPacket(), new NoEndpointAvailablePacket(), new EndpointRequestPacket() };

        private const int ChecksumWidth = 4;
        private const int ContentLengthFieldWidth = 4;
        private const int PacketIdFieldWidth = 1;

        public static IPacket GetPacketInstance(byte[] data)
        {
            if (!CheckPacketDataIntegrity(data))
                return null;
            return GetPacket(data);
        }

        private static IPacket GetPacket(byte[] data)
        {
            var packetId = (PacketIds)data[ChecksumWidth];
            var packet = RegisteredPackets.FirstOrDefault(p => p.Id == packetId);
            if (packet == null)
                return null;

            int contentLength = BitConverter.ToInt32(data, ChecksumWidth + PacketIdFieldWidth - 1);

            var packetContent = new byte[contentLength];
            for (int i = ChecksumWidth + PacketIdFieldWidth + ContentLengthFieldWidth; i < data.Length; ++i)
                packetContent[i - (ChecksumWidth + PacketIdFieldWidth + ContentLengthFieldWidth)] = data[i];

            var newPacketInstance = Activator.CreateInstance(packet.GetType(), packetContent) as IPacket;

            return newPacketInstance;
        }

        public static byte[] CreateData(IPacket packet)
        {
            if (packet == null)
                throw new ArgumentNullException("packet");

            var content = packet.GetContent();
            var idLengthContent = new byte[content.Length + PacketIdFieldWidth + ContentLengthFieldWidth];

            idLengthContent[0] = (byte)packet.Id;

            var contentLength = BitConverter.GetBytes(content.Length);
            idLengthContent[1] = contentLength[0]; // Not in the mood for a for loop
            idLengthContent[2] = contentLength[1];
            idLengthContent[3] = contentLength[2];
            idLengthContent[4] = contentLength[3];

            for (int i = (PacketIdFieldWidth + ContentLengthFieldWidth); i < idLengthContent.Length; ++i)
                idLengthContent[i] = content[i - (PacketIdFieldWidth + ContentLengthFieldWidth)];

            byte[] checksum;
            using (var provider = new Crc32())
                checksum = provider.ComputeHash(idLengthContent, 0, idLengthContent.Length);

            using (var ms = new MemoryStream())
            {
                ms.Write(checksum, 0, ChecksumWidth);
                ms.Write(idLengthContent, 0, idLengthContent.Length);
                return ms.ToArray();
            }
        }

        private static bool CheckPacketDataIntegrity(byte[] data)
        {
            if (data == null || data.Length < ChecksumWidth + PacketIdFieldWidth + ContentLengthFieldWidth)
                return false;

            var checksum = new byte[ChecksumWidth];
            for (int i = 0; i < checksum.Length; i++)
                checksum[i] = data[i];

            byte[] computedHash;
            using (var provider = new Crc32())
                computedHash = provider.ComputeHash(data, ChecksumWidth - 1, data.Length - ChecksumWidth);

            if (checksum != computedHash)
                return false;

            int contentLength = BitConverter.ToInt32(data, ChecksumWidth + PacketIdFieldWidth);

            return contentLength + ChecksumWidth + PacketIdFieldWidth == data.Length;
        }
    }
}

/*

byte[16] checksum; //CRC32
byte packetId; // 1 byte
int contentLength; // 4 byte
byte[contentLength] content; // contentLength byte

*/