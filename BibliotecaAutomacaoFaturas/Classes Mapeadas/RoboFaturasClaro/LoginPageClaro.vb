Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome

Public Class LoginPageClaro

    Private Driver As ChromeDriver
    Private AbaAdminCorp, CampoLogin, CampoSenha As IWebElement
    Private _resultado As ResultadoLogin

    Sub New()
        Me.Driver = ContainerWebdriver.Driver
    End Sub


    Private Sub IrParaPaginaInicial()
        Driver.Navigate.GoToUrl("https://meutim.tim.com.br/")
    End Sub

    Public Function Logar(conta As Conta) As ResultadoLogin
        IrParaPaginaInicial()
        FieldSetup()

        AbaAdminCorp.Click()
        CampoLogin.SendKeys(conta.Gestores.First.Login)
        CampoSenha.SendKeys(conta.Gestores.First.SenhaContaOnline)
        _resultado = ResultadoLogin.Logado

        Return GetResultado()
    End Function

    Private Sub FieldSetup()
        AbaAdminCorp = Driver.FindElementById("btn-aba-admin")
        CampoLogin = Driver.FindElementById("campo-login")
        CampoSenha = Driver.FindElementById("campo-senha-corporativo")
    End Sub

    Private Function GetResultado() As ResultadoLogin
        Return _resultado
    End Function

End Class
