Imports NetDiscovery
Imports System.Net
Imports System.Net.Sockets
Imports System.Linq

Module Server

    Sub Main()

        Dim hostEntry = Dns.GetHostEntry(Dns.GetHostName())
        Dim myLanIp = hostEntry.AddressList.FirstOrDefault(Function(i) i.AddressFamily = AddressFamily.InterNetwork AndAlso Not IPAddress.IsLoopback(i))
        Dim servedEndpoint As New IPEndPoint(myLanIp, 1337) ' Chat runs on port 1337

        Initialize(servedEndpoint)

        Dim cs = New ChatServer(servedEndpoint)
        cs.Start()

        Console.WriteLine("Waiting for clients...")
        Dim command As String
        Do
            command = Console.ReadLine()
        Loop Until command.Equals("exit", StringComparison.OrdinalIgnoreCase) OrElse command.Equals("quit", StringComparison.OrdinalIgnoreCase)
    End Sub

    Private Sub Initialize(ep As IPEndPoint)
        Dim clientServer As Discoverable = New Discoverable(9007, ep) ' serve Endpoint to other clients
        clientServer.ListenAsync()
    End Sub

End Module