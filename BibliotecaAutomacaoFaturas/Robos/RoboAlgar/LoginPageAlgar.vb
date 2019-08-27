Imports BibliotecaAutomacaoFaturas
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

        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='main']/div[2]/div/div/img")))

        If Driver.Url = "https://algartelecom.com.br/AreaClienteCorporativo/" Then
            RaiseEvent LoginRealizado(conta)
        Else
            Throw New ErroLoginExcpetion(conta.Faturas.First, "Login ou senha invalidos", False)
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
