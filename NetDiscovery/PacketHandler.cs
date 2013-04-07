using System;
using System.Security.Cryptography;

namespace NetDiscovery
{
    static class PacketHandler
    {
        public static void HandlePacket(byte[] data)
        {
            if(!CheckPacketDataIntegrity(data))
                return;
            
            var p = Parse(data);
        }
        private static bool CheckPacketDataIntegrity(byte[] data)
        {
            if (data == null || data.Length < 9)
                return false;
            
            var checksum = new byte[16];
            for (int i = 0; i < checksum.Length; i++)
                checksum[i] = data[i];

            byte[] computedHash;
            using(var md5 = new MD5CryptoServiceProvider())
                computedHash = md5.ComputeHash(data, 15, data.Length - 15);

            return checksum == computedHash;
        }

        public static IPacket Parse(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] CreateData(IPacket packet)
        {
            throw new NotImplementedException();
        }
    }


}
/*

byte[16] checksum; //MD5 // 16 byte
byte packetId; // 1 byte
int contentLength; // 4 byte
byte[contentLength] content; // contentLength byte

*/