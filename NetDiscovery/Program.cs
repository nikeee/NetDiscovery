using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace DiscoverLib
{
    class Program
    {
        static void Main(string[] args)
        {
            var myIpEndpoint = new IPEndPoint(new IPAddress(new byte[] { 123, 21, 13, 37 }), 1339);
            var discoverable = new Discoverable(10000, myIpEndpoint);
            new Thread(discoverable.Listen).Start();
            var discoverer = new Discoverer(10000);
            var myEndpoint = discoverer.Discover();
            Console.ReadKey();
        }
    }
}
