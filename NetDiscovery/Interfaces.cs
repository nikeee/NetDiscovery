using System.Net;
using System.Threading.Tasks;

namespace NetDiscovery
{
    interface INetDiscoverable
    {
        IPEndPoint OfferedEndpoint { get; }
        void Listen();
#if TAP
        void ListenAsync();
#endif
    }

    interface INetDiscoverer
    {
        DiscoveryResult Discover();
#if TAP
        Task<DiscoveryResult> DiscoverAsync();
#endif
    }

}