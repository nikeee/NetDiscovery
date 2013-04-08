NetDiscovery
============

A simple, small library for local area networks server discovery (DHCP-Like). Currently only supports IPv4 via Broadcast (no IPv6 Multicast).

Sample usage:
=============

#Client:
```C#
Discoverer severDiscoverer = new Discoverer(9007);
DiscoveryResult res = severDiscoverer.Discover();
if(!res.Canceled && res.Error)
{
	var receivedEndpoint = res.OfferedEndPoint;
	// The sample client-server-application can now operate via the reveivedEndpoint (for example a chat)
}

```

#Server:
```C#
// endPointForChat is the IPEndPoint the client-server-application actually uses (a chat, for example)
Discoverable clientHandler = new Discoverable(9007, endPointForChat);
clientHandler.ListenAsync() // handle client broadcast requests

// now the server has to liten for connections on endPointForChat

```