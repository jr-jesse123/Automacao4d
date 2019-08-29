
Imports System.Runtime.Serialization

Partial Public Class ErroLoginExcpetion
    Inherits RoboFaturaException

    Public Sub New()
    End Sub


    ''' <summary>
    ''' Envia Excessão de login para todas as faturas de cotas as
    ''' contas do gestor da operadora e tipo discriminados
    ''' </summary>
    ''' <param name="gestor"></param>
    ''' <param name="message"></param>
    ''' <param name="dadosok"></param>
    Public Sub New(gestor As Gestor, message As String, dadosok As Boolean, operadora As OperadoraEnum, tipo As TipoFaturaEnum)
        MyBase.New(gestor, message, dadosok, operadora, tipo)

    End Sub
    ''' <summary>
    ''' Envia Excessão de login para todas as faturas desta mesma conta
    ''' </summary>
    ''' <param name="gestor"></param>
    ''' <param name="message"></param>
    ''' <param name="dadosok"></param>
    Public Sub New(conta As Conta, message As String, dadosok As Boolean, operadora As OperadoraEnum, tipo As TipoFaturaEnum)
        MyBase.New(conta, message, dadosok, operadora, tipo)
    End Sub
    ''' <summary>
    ''' Envia Excessão de login para todas as faturas de cotas as contas 
    ''' do empresa da operadora e tipo discriminados
    ''' </summary>
    ''' <param name="gestor"></param>
    ''' <param name="message"></param>
    ''' <param name="dadosok"></param>
    Public Sub New(empresa As Empresa, message As String, dadosok As Boolean, operadora As OperadoraEnum, tipo As TipoFaturaEnum)
        MyBase.New(empresa, message, dadosok, operadora, tipo)

    End Sub
    Public Sub New(fatura As Fatura, message As String, dadosok As Boolean)
        MyBase.New(fatura, message, dadosok)

    End Sub

    Public Sub New(fatura As Fatura, message As String)
        MyBase.New(fatura, message)

    End Sub

    Public Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub


End Class

