using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using NetDiscovery.Packets;

// May not work (hangover while coding)

namespace NetDiscovery
{
    static class PacketHandler
    {
        private static readonly List<IPacket> RegisteredPackets = new List<IPacket>() { new OfferEndPointPacket(), new NoEndpointAvailablePacket(), new EndpointRequestPacket() };

        private const int ChecksumWidth = 16;

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

            int contentLength = BitConverter.ToInt32(data, ChecksumWidth + 1);

            var packetContent = new byte[contentLength];
            for (int i = ChecksumWidth + 1 + 4; i < data.Length; ++i)
                packetContent[i - (ChecksumWidth + 1 + 4)] = data[i];

            var newPacketInstance = Activator.CreateInstance(packet.GetType(), packetContent) as IPacket;

            return newPacketInstance;
        }

        public static byte[] CreateData(IPacket packet)
        {
            if (packet == null)
                throw new ArgumentNullException("packet");

            var content = packet.GetContent();
            var idPlusContent = new byte[content.Length + 1];

            idPlusContent[0] = (byte)packet.Id;
            for (int i = 1; i < idPlusContent.Length; ++i)
                idPlusContent[i] = content[i - 1];

            byte[] checksum;
            using (var provider = new MD5CryptoServiceProvider()) //TODO: Change to CRC32
                checksum = provider.ComputeHash(idPlusContent, 0, idPlusContent.Length);

            using (var ms = new MemoryStream())
            {
                ms.Write(checksum, 0, checksum.Length);
                ms.Write(idPlusContent, 0, idPlusContent.Length);
                return ms.ToArray();
            }
        }

        private static bool CheckPacketDataIntegrity(byte[] data)
        {
            if (data == null || data.Length < ChecksumWidth + 1 + 4)
                return false;

            var checksum = new byte[ChecksumWidth];
            for (int i = 0; i < checksum.Length; i++)
                checksum[i] = data[i];

            byte[] computedHash;
            using (var provider = new MD5CryptoServiceProvider()) //TODO: Change to CRC32
                computedHash = provider.ComputeHash(data, ChecksumWidth - 1, data.Length - ChecksumWidth);

            if (checksum != computedHash)
                return false;

            int contentLength = BitConverter.ToInt32(data, ChecksumWidth + 1);

            return contentLength + ChecksumWidth == data.Length;
        }
    }
}

/*

byte[16] checksum; //MD5 // 16 bytes -> better use CRC32
byte packetId; // 1 byte
int contentLength; // 4 byte
byte[contentLength] content; // contentLength bytes

*/