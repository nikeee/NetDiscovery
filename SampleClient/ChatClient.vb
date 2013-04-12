Imports System.Net.Sockets
Imports System.Net

Class ChatClient

    Private _c As TcpClient
    Private _name As String
    Private _ep As IPEndPoint

    Sub New(name As String, endPoint As IPEndPoint)
        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException("name")
        If endPoint Is Nothing Then Throw New ArgumentNullException("endPoint")

        _name = name
        _ep = endPoint
        _c = New TcpClient()
    End Sub

    Sub Connect()
        _c.Connect(_ep)
    End Sub

    Public Sub SendMessage()
        'TODO: Send a message
    End Sub

End Class