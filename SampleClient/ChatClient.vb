Imports System.Net.Sockets
Imports System.Net
Imports System.IO

Class ChatClient

    Private _c As UdpClient
    Private _name As String
    Private _ep As IPEndPoint

    Private _ns As NetworkStream

    Sub New(name As String, endPoint As IPEndPoint)
        If String.IsNullOrWhiteSpace(name) Then Throw New ArgumentNullException("name")
        If endPoint Is Nothing Then Throw New ArgumentNullException("endPoint")

        _name = name
        _ep = endPoint
        _c = New UdpClient()
    End Sub

    Public Event GotMessage(sender As Object, e As GotMessagePacket) ' Not in the mood of creatign own event args

    Sub Connect()
        _c.Connect(_ep)
        ReadIncomingPacketsAsync()
    End Sub

    Private Sub InvokeMessagePacket(p As GotMessagePacket)
        RaiseEvent GotMessage(Me, p)
    End Sub

    Private Async Sub ReadIncomingPacketsAsync()
        While True
            Dim res = Await _c.ReceiveAsync()
            Dim packet = ChatPacketHandler.GetPacketInstance(res.Buffer)
            InvokeMessagePacket(packet)
        End While
    End Sub

    Public Sub SendMessage(message As String)
        SendPacket(New SendMessagePacket(_name, message))
    End Sub

    Private Async Sub SendPacket(p As IChatPacket)
        If Not _c.Client.Connected OrElse _ns Is Nothing Then Throw New InvalidOperationException()
        Dim bytes = p.GetContent()
        Await _c.SendAsync(bytes, bytes.Length)
    End Sub

End Class