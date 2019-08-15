Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.UI

Public Class LoginPageAlgar

    Private Driver As ChromeDriver
    Private AbaAdminCorp, CampoLogin, CampoSenha As IWebElement
    Private _resultado As ResultadoLogin
    Public Event LoginRealizado(conta As Conta)

    Sub New()
        Me.Driver = WebdriverCt.Driver
    End Sub


    Private Sub IrParaPaginaInicial()
        '        Driver.SwitchTo.Window(Driver.WindowHandles(0))
        Driver.Navigate.GoToUrl("https://contaonline.claro.com.br/webbow/login/initPJ_oqe.do")
    End Sub

    Public Function Logar(conta As Conta) As ResultadoLogin
        IrParaPaginaInicial()
        FecharAbasSecundarias()

        Dim DadosDeAcesso As DadosDeAcesso = ObterDadosDeAcesso(conta)


        CampoLogin = Driver.FindElementById("usuario")
        CampoLogin.SendKeys(DadosDeAcesso.Login)

        CampoSenha = Driver.FindElementById("senha")
        CampoSenha.SendKeys(DadosDeAcesso.Senha)

        'Dim janelas = Driver.WindowHandles.Count

        Driver.FindElementByXPath("/html/body/div[4]/div[2]/div[1]/div[2]/form/div/div[4]/div/div/button").Submit()



        If Driver.Url = "https://algartelecom.com.br/AreaClienteCorporativo/" Then


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

    Private Function ObterDadosDeAcesso(conta As Conta) As DadosDeAcesso

        Dim output As DadosDeAcesso
        output = conta.Empresa.ListaSenhas.Where(Function(lista)
                                                     Return lista.Operadora = OperadoraEnum.ALGAR And
                                                     lista.Tipo = TipoContaEnum.FIXA
                                                 End Function)


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

        If Driver.FindElementById("account-billing-switcher").Text = conta.NrDaConta Then
            Return True
        Else
            Dim ContasContaner = Driver.FindElementById("account-billing-switcher__listbox")
            Dim ListaDeContas As New Dictionary(Of String, IWebElement)

            For Each contaLI In ContasContaner.FindElements(By.TagName("li"))
                ListaDeContas.Add(contaLI.GetAttribute("innerHTML"), contaLI)
            Next

            If ListaDeContas.Keys.Contains(conta.NrDaConta) Then

                Driver.FindElementById("account-billing-switcher").Click()

                If ListaDeContas(conta.NrDaConta).Displayed Then
                    ListaDeContas(conta.NrDaConta).Click()
                    Return True
                Else
                    Stop
                End If
            Else
                Return False

            End If


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
