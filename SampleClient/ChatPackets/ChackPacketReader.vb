Imports System.IO

Class ChackPacketHandler
    Public Function GetPacketData(p As SendMessagePacket) As Byte()

    End Function
    Public Function GetPacketData(bytes As Byte()) As GotMessagePacket
        Using ms As New MemoryStream(bytes)
            Using rdr As New BinaryReader(ms)
                Dim from = rdr.ReadString()
                Dim msg = rdr.ReadString()
                Return New GotMessagePacket(from, msg)
            End Using
        End Using
    End Function
End Class

Interface IChatPacket
    Function GetContent() As Byte()
End Interface

Structure GotMessagePacket
    Implements IChatPacket

    Public Function GetContent() As Byte() Implements IChatPacket.GetContent
        Using ms As New MemoryStream()
            Using wtr As New BinaryWriter(ms)
                wtr.Write([From])
                wtr.Write(Message)
                wtr.Flush()
            End Using
            Return ms.ToArray()
        End Using
    End Function

    Public [From] As String
    Public Message As String
    Sub New(frm As String, msg As String)
        From = frm
        Message = msg
    End Sub
End Structure

Structure SendMessagePacket
    Implements IChatPacket

    Public Function GetContent() As Byte() Implements IChatPacket.GetContent
        Using ms As New MemoryStream()
            Using wtr As New BinaryWriter(ms)
                wtr.Write([From])
                wtr.Write(Message)
                wtr.Flush()
            End Using
            Return ms.ToArray()
        End Using
    End Function

    Public [From] As String
    Public Message As String
    Sub New(frm As String, msg As String)
        From = frm
        Message = msg
    End Sub
End Structure