Imports System.Net.Sockets
Imports System.Net

Class ChatServer
    Private _port As Integer
    Private _listener As TcpListener

    Sub New(localIp As IPAddress, port As Integer)
        If port <= 1024 Then Throw New ArgumentException("do not mess with the standards")
        _port = port
        _listener = New TcpListener(localIp, _port)
    End Sub
    Sub New(localEndpoint As IPEndPoint)
        MyClass.New(localEndpoint.Address, localEndpoint.Port)
    End Sub

    Public Sub Start()
        _listener.Start()
    End Sub

End Class