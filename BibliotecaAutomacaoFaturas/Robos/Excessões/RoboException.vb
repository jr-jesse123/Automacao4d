
Imports System.Runtime.Serialization
Imports BibliotecaAutomacaoFaturas

<Serializable>
Public Class RoboFaturaException
    Inherits Exception

    Private gestor As Gestor
    Private message As String
    Private dadosok As Boolean
    Private operadora As OperadoraEnum
    Private tipo As TipoFaturaEnum

    Public Sub New()
    End Sub

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(fatura As Fatura, message As String, dadosok As Boolean)
        MyBase.New(message)

        GerRelDB.AtualizarContaComLogNaFatura(fatura, message, dadosok)
    End Sub

    Public Sub New(fatura As Fatura, message As String)
        MyBase.New(message)

        GerRelDB.AtualizarContaComLogNaFatura(fatura, message)
    End Sub

    Public Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Sub New(gestor As Gestor, message As String, dadosok As Boolean, operadora As OperadoraEnum, tipo As TipoFaturaEnum)

        Dim contas = GerRelDB.Contas.Where(Function(c) c.Gestores.Contains(gestor) And
            c.Operadora = operadora And c.TipoDeConta = tipo)


        For Each conta In contas
            For Each fatura In conta.Faturas
                fatura.LogRobo.Add(message)
            Next
            GerRelDB.UpsertConta(conta)
        Next

    End Sub

    Public Sub New(empresa As Empresa, message As String, dadosok As Boolean, operadora As OperadoraEnum, tipo As TipoFaturaEnum)

        Dim contas = GerRelDB.Contas.Where(Function(c) c.Empresa.Equals(empresa) And
            c.Operadora = operadora And c.TipoDeConta = tipo)


        For Each conta In contas
            For Each fatura In conta.Faturas
                fatura.LogRobo.Add(message)
            Next
            GerRelDB.UpsertConta(conta)
        Next

    End Sub

    Public Sub New(conta As Conta, message As String, dadosok As Boolean, operadora As OperadoraEnum, tipo As TipoFaturaEnum)


        For Each fatura In conta.Faturas
            fatura.LogRobo.Add(message)
        Next
        GerRelDB.UpsertConta(conta)

    End Sub
End Class

