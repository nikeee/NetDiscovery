using System;
using System.Security.Cryptography;

namespace DiscoverLib
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

            byte[] checksum = new[] { data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10], data[11], data[12], data[13], data[14], data[15] };
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