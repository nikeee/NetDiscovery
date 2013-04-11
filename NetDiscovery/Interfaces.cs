using System.Net;
using System.Threading.Tasks;

namespace NetDiscovery
{
    public interface INetDiscoverable
    {
        IPEndPoint OfferedEndpoint { get; }
        void Listen();
#if TAP
        void ListenAsync();
#endif
    }

    public interface INetDiscoverer
    {
        DiscoveryResult Discover();
#if TAP
        Task<DiscoveryResult> DiscoverAsync();
#endif
    }

}