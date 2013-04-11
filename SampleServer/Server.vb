Imports NetDiscovery
Imports System.Net

Module Server

    Sub Main()
        Dim myLanIp As New IPAddress({192, 168, 178, 12})
        Dim servedEndpoint As New IPEndPoint(myLanIp, 1337) ' Chat runs on 192.168.178.12:1337

        Initialize(servedEndpoint)

        Console.WriteLine("Waiting...")
        Console.ReadKey()
    End Sub

    Private Sub Initialize(ep As IPEndPoint)
        Dim clientServer As Discoverable = New Discoverable(9007, ep) ' serve Endpoint to other clients
        clientServer.ListenAsync()
    End Sub

End Module
