Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium.Chrome


Public Class RoboFaturasTIM
    Inherits RoboBase

    Public Sub New(LoginPage As ILoginPageTim, ContaPage As IContaPageTim, TratadorDeFaturaPDF As TratadorDeFaturasPDF)

        MyBase.New(LoginPage, ContaPage, TratadorDeFaturaPDF, 2, 10)


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

    End Function


End Class


