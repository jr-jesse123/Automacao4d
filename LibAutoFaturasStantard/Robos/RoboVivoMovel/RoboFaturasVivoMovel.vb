﻿#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome


Public Class RoboFaturasVIVOMOVEL
    Inherits RoboBase

    Public Sub New(LoginPage As IloginPageVIVOMOVEL, ContaPage As IContaPageVivoMovel, tratadorpdf As TratadorDeFaturasPDF)

        MyBase.New(LoginPage, ContaPage, tratadorpdf, 5, 10)


    End Sub


    Protected Overrides Function GerenciarLogin(conta As Conta) As Boolean

        Try

            If ContaLogada Is Nothing Then

                Try
                    LoginPage.logout()
                Catch ex As WebDriverException

                Finally
                    LoginPage.Logar(conta)
                End Try

            End If

            If ContaLogada.Gestores.First.CPF = conta.Gestores.First.CPF Then
                Return True
            Else
                LoginPage.logout()
                LoginPage.Logar(conta)
                Return True
            End If


        Catch ex As WebDriverException
            ContaLogada = Nothing
            RoboBase.EnviarLog("Erro no Login")
            RoboBase.EnviarLog(ex.Message)
            RoboBase.EnviarLog(Environment.NewLine + ex.StackTrace)
            WebdriverCt.ResetarWebdriver()

            Throw


        Catch ex As RoboFaturaException
            ContaLogada = Nothing
            RoboBase.EnviarLog("Erro no Login")
            RoboBase.EnviarLog(ex.Message)
            RoboBase.EnviarLog(Environment.NewLine + ex.StackTrace)
            Throw
        End Try

    End Function

    Protected Overrides Sub RealizarLogNasContasCorrespondentes(conta As Conta)

        Dim contas = GerRelDB.Contas.Where(Function(c) c.Gestores.Contains(conta.Gestores.First))

        For Each _conta In contas
            For Each fatura In _conta.Faturas
                GerRelDB.AtualizarContaComLogNaFatura(fatura, $"Gestor Logado corretamente ", True)
            Next
        Next

    End Sub



End Class


