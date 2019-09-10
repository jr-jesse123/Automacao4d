Imports System.Runtime.Serialization
Imports BibliotecaAutomacaoFaturas

<Serializable>
Public Class ContaNaoCadasTradaException
    Inherits RoboFaturaException

    Public Sub New()
    End Sub

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Public Sub New(fatura As Fatura, message As String, Optional dadosok As Boolean = True, Optional innerException As Exception = Nothing)
        MyBase.New(fatura, message, dadosok, innerException)
    End Sub

    Public Sub New(conta As Conta, message As String, Optional dadosok As Boolean = True, Optional innerException As Exception = Nothing)
        MyBase.New(conta, message, dadosok, innerException)
    End Sub

    Public Sub New(gestor As Gestor, message As String, operadora As OperadoraEnum, tipo As TipoFaturaEnum, Optional dadosok As Boolean = True, Optional innerException As Exception = Nothing)
        MyBase.New(gestor, message, operadora, tipo, dadosok, innerException)
    End Sub

    Public Sub New(empresa As Empresa, message As String, operadora As OperadoraEnum, tipo As TipoFaturaEnum, Optional dadosok As Boolean = True, Optional innerException As Exception = Nothing)
        MyBase.New(empresa, message, operadora, tipo, dadosok, innerException)
    End Sub

    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class
