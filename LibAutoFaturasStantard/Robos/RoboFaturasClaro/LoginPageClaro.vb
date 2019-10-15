#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
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

        Dim dadosAcesso = ObtenedorDadosAcesso.ObterDAdosAcessoEmpresa(conta)

        Dim CampoLogin = driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td[1]/input")
        CampoLogin.SendKeys(dadosAcesso.Login)

        Dim CampoSenha = driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td[2]/input")
        CampoSenha.SendKeys(dadosAcesso.Senha)

        Dim janelas = driver.WindowHandles.Count

        driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td[3]").Submit()

        If driver.WindowHandles.Count > janelas Then

            driver.SwitchTo.Window(driver.WindowHandles(janelas))

            RaiseEvent LoginRealizado(conta)

        Else
            Throw New LoginOuSenhaInvalidosException(conta, "Login ou senha invalidos")
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


    Public Sub Logout() Implements ILoginPage.logout
        driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[7]/a").Click()
        FecharAbasSecundarias()
    End Sub


    Public Function ObterDadosDeAcesso(conta As Conta) As DadosDeAcesso
        Return ObtenedorDadosAcesso.ObterDAdosAcessoEmpresa(conta)
    End Function
End Class

Public Interface IloginPageClaro
    Inherits ILoginPage
End Interface
