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
        Driver.Navigate.GoToUrl("https://contaonline.claro.com.br/webbow/login/initPJ_oqe.do")
    End Sub

    Public Function Logar(conta As Conta) As ResultadoLogin
        IrParaPaginaInicial()

        CampoLogin = Driver.FindElementById("/html/body/form/table/tbody/tr[2]/td[1]/input")
        CampoSenha = Driver.FindElementById("/html/body/form/table/tbody/tr[2]/td[2]/input")
        Driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td[3]/input").Click()

        Dim janelas = Driver.WindowHandles.Count

        If Driver.Url = "https://contaonline.claro.com.br/webbow/login/logonPJ.do" Then
            _resultado = ResultadoLogin.UsuarioOuSenhaInvalidos
        ElseIf Driver.WindowHandles.Count > janelas Then
            Driver.SwitchTo.Window(Driver.WindowHandles(janelas))
            If PosicionarConta(conta) Then
                _resultado = ResultadoLogin.Logado
                RaiseEvent LoginRealizado(conta)
            End If

        End If

        Return GetResultado()
    End Function

    Private Function PosicionarConta(conta As Conta) As Boolean

        Dim OpcoesContas = Driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[5]/select")
        Dim selectElement As New SelectElement(OpcoesContas)

        Try
            selectElement.SelectByText(conta.NrDaConta)
            Return True
        Catch ex As NoSuchElementException
            Return False
        End Try

    End Function

    Private Function GetResultado() As ResultadoLogin
        Return _resultado
    End Function

    Friend Sub Logout()
        Driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[7]/a").Click()
    End Sub
End Class
