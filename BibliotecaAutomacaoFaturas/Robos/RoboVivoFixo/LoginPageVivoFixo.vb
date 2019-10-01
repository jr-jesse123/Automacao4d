Imports OpenQA.Selenium

Public Class LoginPageVivoFixo
    Inherits DriverDependents
    Implements IloginPageVivoFixo


    Public Event LoginRealizado(conta As Conta) Implements ILoginPage.LoginRealizado

    Private Sub IrParaPaginaInicial(CNPJ As String)

        Try
            driver.Navigate.GoToUrl($"https://login.vivo.com.br/loginmarca/appmanager/marca/publico?acesso=empresas&_ga=1.215903191.1168325316.1480354519&documento={CNPJ}&app=legacy#")
        Catch ex As WebDriverTimeoutException
            Utilidades.longaEsperaPorNavegacao(driver, "https://login.vivo.com.br/loginmarca/appmanager/marca/publico?acesso=empresas&_ga=1.215903191.1168325316.1480354519&documento={cPF}&app=legacy#", 5)
        End Try


        If Utilidades.ChecarPresenca(driver, "/html/body/div[5]/div/div[2]/div[1]/div[3]/a[1]") Then Call fixaLocal()

    End Sub


    Public Sub Logar(conta As Conta) Implements ILoginPage.Logar

        IrParaPaginaInicial(conta.Empresa.CNPJ)


        Dim DadosDeAcesso = ObtenedorDadosAcesso.ObterDAdosAcessoEmpresa(conta)

        Try
            'opta por acessar como fixo
            driver.FindElementByXPath("//*[@id='loginPJ_vivo_fixo']/div[2]/div/input").Click()
        Catch ex As ElementNotInteractableException

            'verifica se ele está dando a opção de acessar somente o fixo
            If driver.FindElementById("acessar_como_fixo").Displayed Then
                driver.FindElementByXPath("//*[@id='acessar_como_fixo']/a").Click()
            End If


        End Try

        Try
            driver.FindElementByXPath("//*[@id='senha_fixo']").SendKeys(DadosDeAcesso.Senha)
        Catch ex As ArgumentNullException
            Throw New LoginOuSenhaInvalidosException(conta.Gestores.First, "Senha não cadastrada", conta.Operadora, conta.TipoDeConta)
        End Try


        driver.FindElementByXPath("//*[@id='loginFixo']").Click()



        Utilidades.longaEsperaPorInvisibilidade(driver, 10, "//*[@id='loadingMaster']")


        If driver.Url.Contains("meuvivoempresas") Then
            RaiseEvent LoginRealizado(conta)
        Else
            Throw New LoginOuSenhaInvalidosException(conta.Gestores.First, "Login ou senha invalidos", conta.Operadora, conta.TipoDeConta)
        End If




    End Sub

    Sub fixaLocal()


        driver.FindElement(By.Id("campoRegional")).Click()
        driver.FindElement(By.Id("campoRegional")).SendKeys("Acre")
        driver.FindElement(By.XPath("//*[@id=""box_scroll""]/div/ul/li")).Click()
        driver.FindElement(By.ClassName("bt_cliente_s")).Click()

    End Sub

    Public Sub logout() Implements ILoginPage.logout
   

            driver.SwitchTo.ParentFrame
            driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[1]/div/div[3]/button")).Click()
            Utilidades.longaEsperaAposClicar(driver, 10, "//*[@id='headerSubmenu_1_2']/div/div[1]/div[1]/div/div[3]/div/div/ul/li/a/span")

    End Sub
End Class

