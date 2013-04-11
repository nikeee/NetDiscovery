using System;
using System.Net;

namespace NetDiscovery
{
    public class DiscoveryResult
    {
        public bool Error { get; private set; }
        public bool Canceled { get; private set; }
        public bool NoEndPointAvailable { get; private set; }
        public Exception Exception { get; private set; }
        public IPEndPoint OfferedEndPoint { get; private set; }

        public DiscoveryResult(bool error, bool canceled, bool noEndPointAvailable, Exception exception, IPEndPoint endPoint)
        {
            Error = error;
            Canceled = canceled;
            Exception = exception;
            NoEndPointAvailable = noEndPointAvailable;
            OfferedEndPoint = endPoint;
        }
        public DiscoveryResult(bool error, Exception exception)
            : this(error, false, false, exception, null)
        { }
        public DiscoveryResult(IPEndPoint endpoint)
            : this(false, false, false, null, endpoint)
        { }
        public DiscoveryResult(bool canceled)
            : this(false, canceled, false, null, null)
        { }

        public readonly static DiscoveryResult NoEndpointAvailableResult = new DiscoveryResult(false, false, true, null,null);
    }
}