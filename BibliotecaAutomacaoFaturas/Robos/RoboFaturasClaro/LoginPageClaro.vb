Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.UI

Public Class LoginPageClaro
    Inherits DriverDependents
    Implements IloginPageClaro
    Public Event LoginRealizado(conta As Conta) Implements ILoginPage.LoginRealizado


    Sub New()
        Me.Driver = WebdriverCt.Driver
    End Sub


    Private Sub IrParaPaginaInicial()
        Driver.SwitchTo.Window(Driver.WindowHandles(0))
        Driver.Navigate.GoToUrl("https://contaonline.claro.com.br/webbow/login/initPJ_oqe.do")
    End Sub

    Public Sub Logar(conta As Conta) Implements ILoginPage.Logar
        IrParaPaginaInicial()
        FecharAbasSecundarias()

        Dim dadosAcesso = ObterDadosDeAcesso(conta)

        Dim CampoLogin = driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td[1]/input")
        CampoLogin.SendKeys(dadosAcesso.Login)

        Dim CampoSenha = driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td[2]/input")
        CampoSenha.SendKeys(dadosAcesso.Senha)

        Dim janelas = driver.WindowHandles.Count

        driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td[3]").Submit()

        If driver.WindowHandles.Count > janelas Then

            driver.SwitchTo.Window(driver.WindowHandles(janelas))
            If PosicionarConta(conta) Then
                RaiseEvent LoginRealizado(conta)
            Else
                Throw New ContaNaoCadasTradaException(conta.Faturas.First, "Esta conta não está cadastrada para esta empresa", False)
            End If
        Else
            Throw New ErroLoginExcpetion(conta.Faturas.First, "Login ou senha invalidos", False)
        End If

    End Sub

    Private Sub FecharAbasSecundarias()
        For Each janela In Driver.WindowHandles
            If Not Driver.WindowHandles.IndexOf(janela) = 0 Then
                Driver.SwitchTo.Window(janela)
                Driver.Close()
            End If
            Driver.SwitchTo.Window(Driver.WindowHandles(0))
        Next

    End Sub

    Private Function PosicionarConta(conta As Conta) As Boolean

        If Driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[5]").Text = conta.NrDaConta Then
            Return True
        Else

            Try
                Dim OpcoesContas = Driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[5]/select")
                Dim selectElement As New SelectElement(OpcoesContas)


                selectElement.SelectByText(conta.NrDaConta)
                Return True
            Catch ex As NoSuchElementException
                Return False
            End Try
        End If
    End Function

    Public Sub Logout() Implements ILoginPage.logout
        driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[7]/a").Click()
    End Sub


    Public Function ObterDadosDeAcesso(conta As Conta) As DadosDeAcesso Implements ILoginPage.ObterDadosDeAcesso
        Return ObtenedorDadosAcesso.ObterDadosAcesso(conta)
    End Function
End Class

Public Interface IloginPageClaro
    Inherits ILoginPage
End Interface
