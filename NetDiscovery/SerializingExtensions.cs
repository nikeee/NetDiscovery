using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetDiscovery
{
    internal static class SerializingExtensions
    {
        public static byte[] SerializeToBytes(this object obj)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public static T DeserializeFromBytes<T>(this byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                var bf = new BinaryFormatter();
                return (T)bf.Deserialize(ms);
            }
        }
    }
}