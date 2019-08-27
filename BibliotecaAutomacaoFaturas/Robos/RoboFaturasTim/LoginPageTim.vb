﻿Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium.Support.UI

Public Class LoginPageTim
    Implements IloginPageTim

    Private Driver As ChromeDriver
    Private AbaAdminCorp, CampoLogin, CampoSenha, BtnEntrar As IWebElement
    Public Resultado As ResultadoLogin
    Public Event LoginRealizado(conta As Conta) Implements IloginPage.LoginRealizado

    Sub New()
        Me.Driver = WebdriverCt.Driver
    End Sub

    Private Sub IrParaPaginaInicial()
        Try
            Driver.Navigate.GoToUrl("https://meutim.tim.com.br/novo")
        Catch ex As WebDriverTimeoutException
            Driver.Navigate.GoToUrl("https://meutim.tim.com.br/novo")
             End Try

    End Sub

    Public Sub Logar(conta As Conta) Implements IloginPage.Logar
        IrParaPaginaInicial()
        FieldSetup()
        AbaAdminCorp.Click()

        Dim dadosAcesso As DadosDeAcesso
        Try
            dadosAcesso = ObterDadosDeAcesso(conta)
        Catch ex As Exception
            Throw New ErroLoginExcpetion(conta.Faturas.Last, "Erro na senha")
        End Try


        CampoLogin.SendKeys(dadosAcesso.Login)
        CampoSenha.SendKeys(dadosAcesso.Senha)
        BtnEntrar.Click()

        If Driver.Url = "https://meutim.tim.com.br/novo" Then

            RaiseEvent LoginRealizado(conta)

        ElseIf Driver.Url = "https://meutim.tim.com.br/portal16/system/" Then
            Throw New PortalForaDoArException(conta.Faturas.Last, "Portal Fora Do ar")

        ElseIf Driver.FindElementById("mensagem-erro-login").Displayed Then
            Throw New ErroLoginExcpetion(conta.Faturas.Last, $" Usuário ou senha inválidos {Now.ToShortTimeString}", False)

        Else
            Throw New PortalForaDoArException(conta.Faturas.Last, $"Portal Fora do Ar {Now.ToShortTimeString}")
        End If

    End Sub

    Private Sub FieldSetup()
        Try
            AbaAdminCorp = Driver.FindElementById("btn-aba-admin")
        Catch ex As NoSuchElementException
            Driver.FindElementByXPath("/html/body/a").Click()
        Finally
            AbaAdminCorp = Driver.FindElementById("btn-aba-admin")
            CampoLogin = Driver.FindElementById("campo-login")
            CampoSenha = Driver.FindElementById("campo-senha-corporativo")
            BtnEntrar = Driver.FindElementById("btn-entrar-corporativo")
        End Try

    End Sub

    Public Sub Logout() Implements ILoginPage.logout

        If Not Driver.Url.ToString.Contains("login") Then

            Driver.Navigate.GoToUrl("https://meutim.tim.com.br/novo")
            Try
                Driver.FindElementByClassName("sair").Click()
            Catch ex As ElementNotInteractableException
                Driver.FindElementByClassName("deslogar").Click()
            End Try

            If Utilidades.ChecarPresenca(Driver, id:="box-logout") Then
                If Driver.FindElementById("box-logout").Displayed Then

                    Driver.FindElementByXPath("//*[@id='box-logout']/div[2]/div/div[2]/div/span").Click()
                    Driver.FindElementByXPath("//*[@id='box-logout']/div[2]/div/div[1]/a[2]").Click()

                End If
            End If
        End If
    End Sub

    Public Function ObterDadosDeAcesso(conta As Conta) As DadosDeAcesso
        Dim x = ObtenedorDadosAcesso.ObterDAdosAcessoEmpresa(conta)

        RoboBase.EnviarLog($"login: {x.Login} senha: {x.Senha}")

        Return x
    End Function

End Class


Public Interface ILoginPageTim
    Inherits IloginPage
End Interface