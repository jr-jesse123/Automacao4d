Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium.Chrome


Public Class RoboFaturasVIVOMOVEL
    Inherits RoboBase

    Public Sub New(LoginPage As IloginPageVIVOMOVEL, ContaPage As IContaPageVIVOMOVEL, TratadorDeFaturaPDF As TratadorDeFaturasPDF)

        MyBase.New(LoginPage, ContaPage, TratadorDeFaturaPDF, 5, 10)


    End Sub


    Protected Overrides Function GerenciarLogin(conta As Conta) As Boolean



        Try

            If ContaLogada Is Nothing Then

                Try
                    LoginPage.logout()
                Catch ex As Exception

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


        Catch ex As Exception
            RoboBase.EnviarLog("Erro no Login")

        End Try

    End Function


End Class


