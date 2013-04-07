using System.Net;

namespace DiscoverLib
{
    interface INetDiscoverable
    {
        IPEndPoint OfferedEndpoint { get; }
        void Listen();
    }

    interface INetDiscoverer
    {
        IPEndPoint Discover();
    }

    /*

    byte[16] checksum; //MD5 // 16 byte
    byte packetId; // 1 byte
    int contentLength; // 4 byte
    byte[contentLength] content; // contentLength byte

    */

    interface IPacket
    {

    }
}