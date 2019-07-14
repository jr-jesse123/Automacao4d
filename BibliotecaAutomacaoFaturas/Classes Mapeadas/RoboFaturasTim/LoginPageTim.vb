Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium.Support.UI

Public Class LoginPageTim

    Private Driver As ChromeDriver
    Private AbaAdminCorp, CampoLogin, CampoSenha As IWebElement
    Public Resultado As ResultadoLogin

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
        CampoLogin.SendKeys(conta.Gestores.First.Login)
        CampoSenha.SendKeys(conta.Gestores.First.SenhaContaOnline)
        Resultado = ResultadoLogin.Logado

        If Driver.Url = "https://meutim.tim.com.br/novo/" Then

            'Dim MinhaConxpath = "//*[@id='link-primeiro-nivel0']/strong/span"
            'Driver.FindElementByXPath(MinhaConxpath).Click()

            Driver.Navigate.GoToUrl("https://meutim.tim.com.br/menu/minha-conta/conta-online")

            Dim espera As New WebDriverWait(Driver, New TimeSpan(0, 0, 15))
            Dim by As By = By.XPath("/html/body/div[1]/section[2]/h1")
            Dim txtEsperado = "MINHA CONTA"

            espera.Until(Function(driver) driver.FindElement(by).Text = txtEsperado)
            Resultado = ResultadoLogin.Logado

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
    End Sub

    Public Sub Logout()
        Driver.FindElementByClassName("sair").Click()
    End Sub



End Class
