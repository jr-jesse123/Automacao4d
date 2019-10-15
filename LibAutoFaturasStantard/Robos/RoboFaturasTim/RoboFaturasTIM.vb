#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports OpenQA.Selenium.Chrome


Public Class RoboFaturasTIM
    Inherits RoboBase

    Public Sub New(LoginPage As ILoginPageTim, ContaPage As IContaPageTim, TratadorPdf As TratadorDeFaturasPDF)

        MyBase.New(LoginPage, ContaPage, TratadorPdf, 2, 10)


    End Sub

    Protected Overrides Sub RealizarLogNasContasCorrespondentes(Conta As Conta)

        For Each Conta In Conta.Empresa.Contas
            GerRelDB.AtualizarContaComLogEmTodasAsFaturas(Conta, "Login Realizado", True)
        Next



    End Sub

    Protected Overrides Function GerenciarLogin(conta As Conta) As Boolean



        Try

            If ContaLogada Is Nothing Then

                Try
                    LoginPage.logout()
                Catch ex As Exception

                End Try

                LoginPage.Logar(conta)
            End If

            If ContaLogada.Equals(conta) Then
                Return True
            Else
                LoginPage.logout()
                LoginPage.Logar(conta)
                Return True
            End If


        Catch ex As Exception
            RoboBase.EnviarLog("Erro no Login")

        End Try

#Disable Warning BC42353 ' Função "GerenciarLogin" não retorna um valor em todos os caminhos de código. Está faltando uma instrução "Return"?
    End Function
#Enable Warning BC42353 ' Função "GerenciarLogin" não retorna um valor em todos os caminhos de código. Está faltando uma instrução "Return"?


End Class