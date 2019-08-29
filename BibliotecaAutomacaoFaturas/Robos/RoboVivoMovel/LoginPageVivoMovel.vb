Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium
    Imports OpenQA.Selenium.Chrome
    Imports OpenQA.Selenium.Support.UI

Public Class LoginPageVivoMovel
    Inherits DriverDependents
    Implements IloginPageVIVOMOVEL

    Public Event LoginRealizado(conta As Conta) Implements ILoginPage.LoginRealizado



    Private Sub IrParaPaginaInicial(cPF As String)



        driver.Navigate.GoToUrl($"https://login.vivo.com.br/loginmarca/appmanager/marca/publico?acesso=empresas&_ga=1.215903191.1168325316.1480354519&documento={cPF}&app=legacy#")
        If Utilidades.ChecarPresenca(driver, "/html/body/div[5]/div/div[2]/div[1]/div[3]/a[1]") Then Call fixaLocal()

    End Sub

    Public Sub Logar(conta As Conta) Implements ILoginPage.Logar
        IrParaPaginaInicial(conta.Gestores.First.CPF)


        Dim DadosDeAcesso
        Try
            DadosDeAcesso = ObtenedorDadosAcesso.ObterDAdosAcessoGestor(conta)
        Catch ex As InvalidOperationException
            Throw New ErroLoginExcpetion(conta, "Login/senha não cadastrados", False, OperadoraEnum.VIVO, TipoContaEnum.MOVEL)
        End Try


        If Utilidades.ChecarPresenca(driver, "//*[@id='msg_cpf_cnpj']") Then
            If driver.FindElementByXPath("//*[@id='msg_cpf_cnpj']").Displayed Then
                GoTo ErroDadosAcesso
            End If
        End If

        preencherLinhaGestor(DadosDeAcesso.Login)



        driver.FindElementById("senha_movel").SendKeys(DadosDeAcesso.Senha)

        driver.FindElementById("loginMovel").Click()


        Utilidades.longaEsperaPorInvisibilidade(driver, 10, "//*[@id='loadingMaster']")


        If driver.Url.Contains("meuvivoempresas") Then
            RaiseEvent LoginRealizado(conta)
        Else
ErroDadosAcesso:
            Throw New ErroLoginExcpetion(conta.Faturas.First, "Login ou senha invalidos", False)
        End If


    End Sub

    Private Sub preencherLinhaGestor(login As String)

linhagestor:

        Dim linhapreenchida
        Dim linhacorreta

        Do Until linhacorreta
            Try
                driver.FindElement(By.XPath("//*[@id='loginPJ_vivo_movel']/div[2]/div/input")).Click() ' verifica se aparece opção de fixo e móvel e escolhe móvel
            Catch ex As WebDriverException
            End Try


            driver.FindElementById("linha_gestor").Clear()
            driver.FindElementById("linha_gestor").SendKeys(login)  ' preenche linha cadastrada
            linhapreenchida = driver.FindElementById("linha_gestor").GetAttribute("value")
            linhapreenchida = Utilidades.lfRetiraNumeros_linhacadastro(linhapreenchida)
            linhacorreta = linhapreenchida = login
        Loop  ' verficica se deu erro no preenchimento da linha e preenche novamente


    End Sub



    Friend Sub Logout() Implements IloginPageVIVOMOVEL.logout

        driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[1]/div/div[3]/button")).Click()
        driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[1]/div/div[3]/div/div/ul/li/a/span")).Click()

    End Sub



    Private Function isAlertPresent() As Boolean

        Try
            driver.SwitchTo().Alert()
            Return True
        Catch ex As NoAlertPresentException
            Return False
        End Try

    End Function

    Sub fixaLocal()

        Try
            driver.FindElement(By.Id("estadoHeader")).Click()
            driver.FindElement(By.ClassName("seta_drop")).Click()
            driver.FindElement(By.XPath("//*[@id=""scrollbox4""]/div[2]/ul/li[1]/a")).Click()

        Catch ex As Exception

            driver.FindElement(By.Id("campoRegional")).Click()
            driver.FindElement(By.Id("campoRegional")).SendKeys("Acre")
            driver.FindElement(By.XPath("//*[@id=""box_scroll""]/div/ul/li")).Click()
            driver.FindElement(By.ClassName("bt_cliente_s")).Click()

        End Try

    End Sub

End Class




Public Interface IloginPageVIVOMOVEL
    Inherits ILoginPage

End Interface
