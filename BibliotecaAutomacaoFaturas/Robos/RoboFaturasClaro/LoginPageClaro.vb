Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.UI

Public Class LoginPageClaro

    Private Driver As ChromeDriver
    Private AbaAdminCorp, CampoLogin, CampoSenha As IWebElement
    Private _resultado As ResultadoLogin
    Public Event LoginRealizado(conta As Conta)

    Sub New()
        Me.Driver = WebdriverCt.Driver
    End Sub


    Private Sub IrParaPaginaInicial()
        Driver.SwitchTo.Window(Driver.WindowHandles(0))
        Driver.Navigate.GoToUrl("https://contaonline.claro.com.br/webbow/login/initPJ_oqe.do")
    End Sub

    Public Function Logar(conta As Conta) As ResultadoLogin
        IrParaPaginaInicial()
        FecharAbasSecundarias

        CampoLogin = Driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td[1]/input")
        CampoLogin.SendKeys(conta.Empresa.LoginContaOnline)

        CampoSenha = Driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td[2]/input")
        CampoSenha.SendKeys(conta.Empresa.SEnhaContaOnline)

        Dim janelas = Driver.WindowHandles.Count

        Driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td[3]").Submit()



        If Driver.WindowHandles.Count > janelas Then

            Driver.SwitchTo.Window(Driver.WindowHandles(janelas))
            If PosicionarConta(conta) Then
                _resultado = ResultadoLogin.Logado
                RaiseEvent LoginRealizado(conta)
            Else
                Throw New ContaNaoCadasTradaException(conta.Faturas.First, "Esta conta não está cadastrada para esta empresa", False)

            End If
        Else
            _resultado = ResultadoLogin.UsuarioOuSenhaInvalidos
            Throw New ErroLoginExcpetion(conta.Faturas.First, "Login ou senha invalidos")
        End If

        Return GetResultado()
    End Function

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

    Private Function GetResultado() As ResultadoLogin
        Return _resultado
    End Function

    Friend Sub Logout()
        Driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[7]/a").Click()
    End Sub



    Public Function isAlertPresent() As Boolean

        Try

            Driver.SwitchTo().Alert()
            Return True

        Catch ex As NoAlertPresentException

            Return False

        End Try

    End Function
End Class
