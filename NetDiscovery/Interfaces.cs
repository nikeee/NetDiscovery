using System.Net;
using System.Threading.Tasks;

namespace NetDiscovery
{
    interface INetDiscoverable
    {
        IPEndPoint OfferedEndpoint { get; }
        void Listen();
#if TAP
        Task ListenAsync();
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