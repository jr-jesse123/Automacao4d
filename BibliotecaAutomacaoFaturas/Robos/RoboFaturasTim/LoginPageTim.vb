Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium.Support.UI

Public Class LoginPageTim
    Implements ILoginPage

    Private Driver As ChromeDriver
    Private AbaAdminCorp, CampoLogin, CampoSenha, BtnEntrar As IWebElement
    Public Resultado As ResultadoLogin
    Public Event LoginRealizado(conta As Conta) Implements ILoginPage.LoginRealizado

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

    Public Function Logar(conta As Conta) As ResultadoLogin Implements ILoginPage.Logar
        IrParaPaginaInicial()
        FieldSetup()

        AbaAdminCorp.Click()
        CampoLogin.SendKeys(conta.Empresa.LoginContaOnline)
        CampoSenha.SendKeys(conta.Empresa.SEnhaContaOnline)
        BtnEntrar.Click()

        If Driver.Url = "https://meutim.tim.com.br/novo" Then



            Resultado = ResultadoLogin.Logado
            RaiseEvent LoginRealizado(conta)

        ElseIf Driver.Url = "https://meutim.tim.com.br/portal16/system/" Then
            Throw New RoboFaturaException(conta.Faturas.First, "Portal Fora Do ar")

        ElseIf Driver.FindElementById("mensagem-erro-login").Displayed Then
            Resultado = ResultadoLogin.UsuarioOuSenhaInvalidos

            Throw New ErroLoginExcpetion($" Usuário ou senha inválidos {Now.ToShortTimeString}")

        Else
            Resultado = ResultadoLogin.PaginaForaDoar
            Throw New PortalForaDoArException($"Portal Fora do Ar {Now.ToShortTimeString}")
        End If



        Return Resultado
    End Function

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

    Public Sub Logout() Implements ILoginPage.Logout
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

    End Sub



End Class
