
Imports BibliotecaAutomacaoFaturas


Public Class RoboFaturasOI
    Inherits RoboBase

    Public Sub New(LoginPage As ILoginPageOI, ContaPage As IContaPageOI, TratadorDeFaturaPDF As TratadorDeFaturasPDF)
        MyBase.New(LoginPage, ContaPage, TratadorDeFaturaPDF, 3, 10)
    End Sub

    Protected Overrides Sub RealizarLogNasContasCorrespondentes(Conta As Conta)

        For Each Conta In Conta.Empresa.Contas.Where(Function(c) c.Operadora = OperadoraEnum.OI)
            GerRelDB.AtualizarContaComLogEmTodasAsFaturas(Conta, "logado corretamente")
        Next


    End Sub

    Protected Overrides Function GerenciarLogin(conta As Conta) As Boolean
        Dim Logado As Boolean


        'Try
        If ContaLogada Is Nothing Then
            LoginPage.Logar(conta)
        End If

        If ContaLogada IsNot Nothing Then
            Logado = ContaLogada.Empresa.Equals(conta.Empresa)
        End If

        If Logado Then
            Return True
        Else
            LoginPage.logout()
            LoginPage.Logar(conta)
            Return True

        End If
        'Catch ex As Exception
        Return False
        'End Try
    End Function

End Class
