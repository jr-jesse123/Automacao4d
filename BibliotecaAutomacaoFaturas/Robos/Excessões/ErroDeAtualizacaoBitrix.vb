Imports System.Runtime.Serialization
Imports BibliotecaAutomacaoFaturas

<Serializable>
Public Class ErroDeAtualizacaoBitrix
    Inherits RoboFaturaException



    Public Sub New(fatura As Fatura, message As String)
        MyBase.New(fatura, message)
    End Sub

End Class
