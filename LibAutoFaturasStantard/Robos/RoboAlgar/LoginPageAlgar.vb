#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.UI

Public Class LoginPageAlgar
    Inherits DriverDependents
    Implements IloginPageAlgar

    Public Event LoginRealizado(conta As Conta) Implements IloginPage.LoginRealizado



    Private Sub IrParaPaginaInicial()
        '        Driver.SwitchTo.Window(Driver.WindowHandles(0))
        Driver.Navigate.GoToUrl("https://algartelecom.com.br/AreaClienteCorporativo/login")
    End Sub

    Public Sub Logar(conta As Conta) Implements IloginPage.Logar
        IrParaPaginaInicial()


        Dim DadosDeAcesso As DadosDeAcesso = ObtenedorDadosAcesso.ObterDAdosAcessoEmpresa(conta)

        driver.FindElementById("user").SendKeys(DadosDeAcesso.Login)
        Driver.FindElementById("password").SendKeys(DadosDeAcesso.Senha)
        Driver.FindElementByTagName("button").Click()


        Dim wait As New WebDriverWait(Driver, New TimeSpan(0, 0, 30))

#Disable Warning BC40000 ' '"ExpectedConditions" está obsoleto: "The ExpectedConditions implementation in the .NET bindings is deprecated and will be removed in a future release. This portion of the code has been migrated to the DotNetSeleniumExtras repository on GitHub (https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras)".
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='main']/div[2]/div/div/img")))
#Enable Warning BC40000 ' '"ExpectedConditions" está obsoleto: "The ExpectedConditions implementation in the .NET bindings is deprecated and will be removed in a future release. This portion of the code has been migrated to the DotNetSeleniumExtras repository on GitHub (https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras)".

        If Driver.Url = "https://algartelecom.com.br/AreaClienteCorporativo/" Then
            RaiseEvent LoginRealizado(conta)
        Else
            Throw New LoginOuSenhaInvalidosException(conta.Faturas.First, "Login ou senha invalidos")
        End If


    End Sub




    Friend Sub Logout() Implements IloginPageAlgar.logout
        driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[7]/a").Click()
    End Sub



    Private Function isAlertPresent() As Boolean

        Try
            Driver.SwitchTo().Alert()
            Return True
        Catch ex As NoAlertPresentException
            Return False
        End Try

    End Function

End Class

Public Interface IloginPageAlgar
    Inherits IloginPage

End Interface
