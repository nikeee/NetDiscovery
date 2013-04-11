Imports NetDiscovery

Module Client

    Sub Main()
        Initialize()

        Console.WriteLine("Waiting...")
        Console.ReadKey()
    End Sub

    Async Sub Initialize()
        Dim serverDiscoverer As New Discoverer(9007)

        Dim res As DiscoveryResult = Await serverDiscoverer.DiscoverAsync()

        If res.Error Then
            Console.WriteLine("An error occurred: {0}", res.Exception.Message)
            Return
        End If

        If res.Canceled Then
            Console.WriteLine("The operation was canceled.")
            Return
        End If

        Dim ep = res.OfferedEndPoint()
        Console.WriteLine("Connecting to Chat Endpoint: {0}", ep)

    End Sub
End Module
