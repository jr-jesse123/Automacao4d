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
        Me.Driver = ContainerWebdriver.Driver
    End Sub


    Private Sub IrParaPaginaInicial()
        Driver.Navigate.GoToUrl("https://meutim.tim.com.br/novo")
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


        ElseIf Driver.FindElementById("mensagem-erro-login").Displayed Then
            Resultado = ResultadoLogin.UsuarioOuSenhaInvalidos
        Else
            Resultado = ResultadoLogin.PaginaForaDoar
        End If

        Return Resultado
    End Function

    Private Sub FieldSetup()
        AbaAdminCorp = Driver.FindElementById("btn-aba-admin")
        CampoLogin = Driver.FindElementById("campo-login")
        CampoSenha = Driver.FindElementById("campo-senha-corporativo")
        BtnEntrar = Driver.FindElementById("btn-entrar-corporativo")
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
