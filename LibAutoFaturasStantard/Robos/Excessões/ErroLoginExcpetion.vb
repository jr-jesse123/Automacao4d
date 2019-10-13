
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
    Public Sub New(gestor As Gestor, message As String, operadora As OperadoraEnum, tipo As TipoContaEnum, Optional innerException As Exception = Nothing)
        MyBase.New(gestor, message, operadora, tipo)

    End Sub
    ''' <summary>
    ''' Envia Excessão de login para todas as faturas desta mesma conta
    ''' </summary>
    ''' <param name="conta"></param>
    ''' <param name="message"></param>
    ''' <param name="dadosok"></param>
    Public Sub New(conta As Conta, message As String)


        MyBase.New(conta, message)
    End Sub
    ''' <summary>
    ''' Envia Excessão de login para todas as faturas de cotas as contas 
    ''' do empresa da operadora e tipo discriminados
    ''' </summary>
    ''' <param name="empresa"></param>
    ''' <param name="message"></param>
    ''' <param name="dadosok"></param>
    Public Sub New(empresa As Empresa, message As String, operadora As OperadoraEnum, tipo As TipoContaEnum, Optional innerException As Exception = Nothing)
        MyBase.New(empresa, message, operadora, tipo)

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

