Imports System.Runtime.Serialization
Imports BibliotecaAutomacaoFaturas

<Serializable>
Public Class ContaNaoCadasTradaException
    Inherits RoboFaturaException



    Public Sub New(conta As Conta, message As String, Optional innerException As Exception = Nothing)
        MyBase.New(conta, message, False)
    End Sub



    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class
