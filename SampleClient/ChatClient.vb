Imports System.Net.Sockets
Imports System.Net
Imports System.IO

Class ChatClient

    Private _c As UdpClient
    Private _name As String
    Private _ep As IPEndPoint

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
        If p.From <> _name Then
            RaiseEvent GotMessage(Me, p)
        End If
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
        If Not _c.Client.Connected Then Throw New InvalidOperationException()
        Dim bytes = p.GetContent()
        Await _c.SendAsync(bytes, bytes.Length)
    End Sub

End Class