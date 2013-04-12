Imports System.Net.Sockets
Imports System.Net

Class ChatServer
    Private _port As Integer
    Private _c As UdpClient

    Sub New(port As Integer)
        If port <= 1024 Then Throw New ArgumentException("do not mess with the standards")
        _port = port
        _c = New UdpClient(_port)
    End Sub
    Sub New(localEndpoint As IPEndPoint)
        MyClass.New(localEndpoint.Port)
    End Sub

    Public Sub Start()
        ListenForMessages()
    End Sub

    Private Async Sub ListenForMessages()
        While True
            Dim res = Await _c.ReceiveAsync()
            Await _c.SendAsync(res.Buffer, res.Buffer.Length, New IPEndPoint(IPAddress.Broadcast, _port))
            'TODO: Add packet handler
        End While
    End Sub

End Class