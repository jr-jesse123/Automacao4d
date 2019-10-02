Imports System.Runtime.Serialization

<Serializable>
Public Class FalhaUploadNoDriveException
    Inherits RoboFaturaException


    Public Sub New(fatura As Fatura, message As String)
        MyBase.New(message)

        GerRelDB.AtualizarContaComLogNaFatura(fatura, message)
    End Sub



    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class
