Imports NetDiscovery
Imports System.Net
Imports System.Net.Sockets

Module Client

    Sub Main()
        Dim name As String
        Do
            Console.Write("Pick a name: ")
            name = Console.ReadLine()
        Loop Until Not String.IsNullOrWhiteSpace(name)

        Initialize(name)

        Console.WriteLine("Waiting...")
        While True
            Console.ReadKey()
        End While
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
        AddHandler cc.GotMessage, AddressOf GotMessage
        cc.Connect()
        While True
            Dim message As String
            Do
                Console.Write(">")
                message = Console.ReadLine()
            Loop Until Not String.IsNullOrWhiteSpace(message)
            cc.SendMessage(message)
        End While
    End Sub

    Private Sub GotMessage(sender As Object, p As GotMessagePacket)
        Console.WriteLine("{0}: {1}", p.From, p.Message)
    End Sub
End Module