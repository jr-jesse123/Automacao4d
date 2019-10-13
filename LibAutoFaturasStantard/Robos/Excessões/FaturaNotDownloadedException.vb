Imports System.Runtime.Serialization
Imports BibliotecaAutomacaoFaturas

<Serializable>
Public Class FaturaNotDownloadedException
    Inherits RoboFaturaException


    Public Sub New(fatura As Fatura, message As String)
        MyBase.New(fatura, message, True)
    End Sub

    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class
