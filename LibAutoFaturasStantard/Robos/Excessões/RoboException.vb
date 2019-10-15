
Imports System.Runtime.Serialization
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.

<Serializable>
Public Class RoboFaturaException
    Inherits Exception

    Public Sub New()
    End Sub

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(fatura As Fatura, message As String, dadosok As Boolean, Optional innerException As Exception = Nothing)
        MyBase.New(message, innerException)

        GerRelDB.AtualizarContaComLogNaFatura(fatura, message, dadosok)
    End Sub

    Public Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub

    Public Sub New(gestor As Gestor, message As String, operadora As OperadoraEnum, tipo As TipoFaturaEnum, Optional dadosok As Boolean = True, Optional innerException As Exception = Nothing)
        MyBase.New(message, innerException)


        Dim contas = GerRelDB.Contas.Where(Function(c) c.Gestores.Contains(gestor) And
            c.Operadora = operadora And c.TipoDeConta = tipo)


        For Each conta In contas
            For Each fatura In conta.Faturas
                fatura.LogRobo.Add(message)
            Next
            GerRelDB.AtualizarContaComLogEmTodasAsFaturas(conta, message, dadosok)
        Next

    End Sub

    Public Sub New(empresa As Empresa, message As String, operadora As OperadoraEnum, tipo As TipoFaturaEnum, Optional dadosok As Boolean = True, Optional innerException As Exception = Nothing)

        MyBase.New(message, innerException)

        Dim contas = GerRelDB.Contas.Where(Function(c) c.Empresa.Equals(empresa) And
            c.Operadora = operadora And c.TipoDeConta = tipo)


        For Each conta In contas
            For Each fatura In conta.Faturas
                fatura.LogRobo.Add(message)

            Next
            GerRelDB.AtualizarContaComLogEmTodasAsFaturas(conta, message, dadosok)
        Next

    End Sub

    Public Sub New(conta As Conta, message As String, Optional dadosok As Boolean = True)

        MyBase.New(message)

        For Each fatura In conta.Faturas
            fatura.LogRobo.Add(message)
        Next
        GerRelDB.AtualizarContaComLogEmTodasAsFaturas(conta, message, dadosok)

    End Sub

    Public Sub New(fatura As Fatura, message As String, Optional innerException As Exception = Nothing)
        MyBase.New(message, innerException)

        GerRelDB.AtualizarContaComLogNaFatura(fatura, message)
    End Sub
End Class

