Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium.Support.UI

Public Class LoginPageTim

    Private Driver As ChromeDriver
    Private AbaAdminCorp, CampoLogin, CampoSenha, BtnEntrar As IWebElement
    Public Resultado As ResultadoLogin
    Public Event LoginRealizado(conta As Conta)


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

    Public Function Logar(conta As Conta) As ResultadoLogin


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
            Throw New ErroLoginExcpetion($"Portal Fora do Ar {Now.ToShortTimeString}")
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

    Public Sub Logout()


        Driver.Navigate.GoToUrl("https://meutim.tim.com.br/novo")

        

        Driver.FindElementByClassName("sair").Click()

        If Utilidades.ChecarPresenca(Driver, id:="box-logout") Then
            If Driver.FindElementById("box-logout").Displayed Then
                Driver.FindElementByClassName("btn-do-logout").Click()
            End If
        End If

    End Sub



End Class
