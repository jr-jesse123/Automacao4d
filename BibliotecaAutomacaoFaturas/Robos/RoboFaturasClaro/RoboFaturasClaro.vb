﻿Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium.Chrome


Public Class RoboFaturasClaro
    Inherits RoboBase

    Public Sub New(LoginPage As IloginPageClaro, ContaPage As IContaPageClaro, TratadorDeFaturaPDF As TratadorDeFaturasPDF)
        MyBase.New(LoginPage, ContaPage, TratadorDeFaturaPDF, 1, 10)

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
                LoginPage.Logout()
                LoginPage.Logar(conta)
                Return True

            End If

        Catch ex As Exception
            Return False
        End Try


    End Function



End Class


