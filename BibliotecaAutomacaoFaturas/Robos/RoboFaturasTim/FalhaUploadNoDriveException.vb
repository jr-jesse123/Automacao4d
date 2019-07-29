Imports System.Runtime.Serialization

<Serializable>
Friend Class FalhaUploadNoDriveException
    Inherits RoboFaturaException

    Public Sub New()
    End Sub


    Public Sub New(fatura As Fatura, message As String)
        MyBase.New(message)

        GerRelDB.EnviarLogFatura(fatura, message)
    End Sub

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class
