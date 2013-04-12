Imports System.Net.Sockets
Imports System.Net
Imports System.IO

Class ChatClient

    Private _c As TcpClient
    Private _name As String
    Private _ep As IPEndPoint

    Private _ns As NetworkStream
    Private _writer As BinaryWriter

    Sub New(name As String, endPoint As IPEndPoint)
        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException("name")
        If endPoint Is Nothing Then Throw New ArgumentNullException("endPoint")

        _name = name
        _ep = endPoint
        _c = New TcpClient()
    End Sub

    Sub Connect()
        _c.Connect(_ep)
        _ns = _c.GetStream()
        _writer = New BinaryWriter(_ns)
    End Sub

    Public Sub SendMessage(message As String)
        SendPacket(New SendMessagePacket(_name, message))
    End Sub

    Private Sub SendPacket(p As IChatPacket)
        If Not _c.Client.Connected OrElse _ns Is Nothing Then Throw New InvalidOperationException()
        'TODO: Send packet
        Dim bytes = p.GetContent()
        _writer.Write(bytes)
    End Sub

End Class