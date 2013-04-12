Imports NetDiscovery
Imports System.Net
Imports System.Net.Sockets

Module Client

    Sub Main()
        Dim name As String
        Do
            Console.WriteLine("Pick a name:")
            name = Console.ReadLine()
        Loop Until Not String.IsNullOrWhiteSpace(name)

        Initialize(name)

        Console.WriteLine("Waiting...")
        Console.ReadKey()
    End Sub

    Private Async Sub Initialize(name As String)
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

        Dim ep As IPEndPoint = res.OfferedEndPoint()
        Console.WriteLine("Connecting to Chat Endpoint: {0}", ep) ' Got endpoint for chat, should now connect to this endpoint

        Dim cc = New ChatClient(name, ep)
        cc.Connect()

    End Sub
End Module