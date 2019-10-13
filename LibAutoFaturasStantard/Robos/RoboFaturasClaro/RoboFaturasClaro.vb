Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium.Chrome


Public Class RoboFaturasClaro
    Inherits RoboBase

    Public Sub New(LoginPage As IloginPageClaro, ContaPage As IContaPageClaro, tratadorpdf As TratadorDeFaturasPDF)
        MyBase.New(LoginPage, ContaPage, tratadorpdf, 1, 10)

    End Sub

    Protected Overrides Sub RealizarLogNasContasCorrespondentes(Conta As Conta)

        For Each Conta In Conta.Empresa.Contas
            GerRelDB.AtualizarContaComLogEmTodasAsFaturas(Conta, "login realizado com sucesso", True)
        Next



    End Sub

    Protected Overrides Function GerenciarLogin(conta As Conta) As Boolean

        Dim Logado As Boolean

        Try

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

        Catch ex As Exception
            ContaLogada = Nothing
            Return False

        End Try


    End Function



End Class


