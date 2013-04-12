Imports NetDiscovery
Imports System.Net
Imports System.Net.Sockets

Module Server

    Sub Main()
        Dim myLanIp As New IPAddress({192, 168, 178, 12}) ' todo get ip dynamically
        Dim servedEndpoint As New IPEndPoint(myLanIp, 1337) ' Chat runs on 192.168.178.12:1337

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