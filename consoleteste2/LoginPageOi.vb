Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.UI


Public Class LoginPageOi
    Inherits DriverDependents
    Implements ILoginPageOI
    Public Event LoginRealizado(conta As Conta) Implements ILoginPage.LoginRealizado

    Sub New()
        Me.driver = WebdriverCt.Driver
    End Sub

    Private Sub irParaPaginaInicial()
        driver.Navigate.GoToUrl("https://autenticacao.oi.com.br/nidp/app/login?id=PortalOIContract&sid=0&option=credential&sid=0")
    End Sub


    Public Sub Logar(conta As Conta) Implements ILoginPage.Logar

        irParaPaginaInicial()

        Dim dadosAcesso = ObtenedorDadosAcesso.ObterDAdosAcessoEmpresa(conta)

        Dim CampoLogin = driver.FindElementByXPath("//*[@id='usernameinput']")
        CampoLogin.SendKeys(dadosAcesso.Login)

        Dim CamposSenha = driver.FindElementByXPath("//*[@id='passwordinput']")
        CamposSenha.SendKeys(dadosAcesso.Senha)

        driver.FindElementByXPath("//*[@id='login']/div[1]/div[2]/form/button").Submit()

        Dim Entrou = Not driver.Url = "https://autenticacao.oi.com.br/nidp/app/login?sid=0&sid=0"

        If Entrou Then
            RaiseEvent LoginRealizado(conta)

        Else
            If Utilidades.ChecarPresenca(driver, "//*[@id='aviso_info']") Then
                Throw New ErroLoginExcpetion(conta.Faturas.First, "Login ou Senha inválidos!", False)

            End If

            Throw New ErroLoginExcpetion(conta.Faturas.First, "Sistema Indisponível!", False)

        End If



    End Sub


    Private Function PosicionarConta(conta As Conta) As Boolean

        If driver.FindElementByXPath("").Text = conta.NrDaConta Then
            Return True
        Else
            Try
                Dim OpcoesContas = driver.FindElementByXPath("")
                Dim selectElement As New SelectElement(OpcoesContas)

                selectElement.SelectByText(conta.NrDaConta)
                Return True

            Catch ex As NoSuchElementException
                Return False
            End Try
        End If
    End Function


    Public Sub logout() Implements ILoginPage.logout
        driver.FindElementByXPath("//*[@id='ng-app']/div[1]/div[2]/div/div/div[1]/section/div/a[6]").Click()
    End Sub

    Public Function ObterdadosDeAcesso(conta As Conta) As DadosDeAcesso
        Return ObtenedorDadosAcesso.ObterDAdosAcessoEmpresa(conta)
    End Function



End Class
