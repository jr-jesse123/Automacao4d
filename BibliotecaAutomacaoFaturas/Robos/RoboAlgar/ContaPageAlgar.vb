Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI
Imports BibliotecaAutomacaoFaturas
Imports BibliotecaAutomacaoFaturas.ErroLoginExcpetion
Imports System.Text.RegularExpressions

Public Class ContaPageAlgar
    Inherits DriverDependents
    Implements IContaPageAlgar

    Private driver As ChromeDriver
    Public Event FaturaBaixada(fatura As Fatura) Implements IContaPageAlgar.FaturaBaixada
    Public Event FaturaChecada(fatura As Fatura) Implements IContaPageAlgar.FaturaChecada
    Public Event FaturaBaixadaPDF(fatura As Fatura) Implements IContaPageAlgar.FaturaBaixadaPdf
    Private Event IContaPage_FaturaBaixadaPDF(fatura As Fatura) Implements IContaPage.FaturaBaixadaPDF

    Public Sub BuscarFatura(fatura As Fatura) Implements IContaPage.BuscarFatura
        Dim faturasFechadas, faturasAbertas, faturasVencidas As IWebElement

        PosicionarConta(fatura)

        Dim wait = New WebDriverWait(driver, New TimeSpan(0, 0, 30))
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("styles__loader___1bPp3")))


        On Error Resume Next
        'exibe todas as faturas, qnd todas estão pagas

        driver.FindElementByXPath("//*[@id='root']/main/div/div/div[1]/section/div/div/a").Click()

        ' exibe todas as faturas, quando existem faturas pendentes 
        driver.FindElementByXPath("//*[@id='root']/main/div/div/div[1]/section/footer/a").Click()

        faturasFechadas = driver.FindElementByXPath("//*[@id='root']/main/div/div[2]/div[3]/section/section/div/form/table/tbody")
        faturasAbertas = driver.FindElementByXPath("//*[@id='root']/main/div/div[2]/div[2]/section/section/div/form/table/tbody")
        faturasVencidas = driver.FindElementByXPath("")
        On Error GoTo 0

        If faturasFechadas IsNot Nothing Then
            If ProcurarFaturasnoBloco(faturasFechadas, fatura, False) Then Exit Sub
        End If

        If faturasAbertas IsNot Nothing Then
            If ProcurarFaturasnoBloco(faturasAbertas, fatura, True) Then Exit Sub
        End If

        If faturasVencidas IsNot Nothing Then
            Throw New NotImplementedException("SITUAÇÃO PREVISTA PORÉM NÃO VISTA EM DESENVOLVIMENTO")
            If ProcurarFaturasnoBloco(faturasVencidas, fatura, True) Then Exit Sub
        End If

        Throw New FaturaNaoDisponivelException(fatura, "Fatura não Disponibilizada no site")

    End Sub

    Private Function ProcurarFaturasnoBloco(BlocoDeFaturas As IWebElement, fatura As Fatura, FaturaPendente As Boolean) As Boolean

        For Each faturaTD In BlocoDeFaturas.FindElements(By.TagName("tr"))
            Dim VencimentoCorreto As Boolean = VerificarVencimentoComDiasUteis(faturaTD, fatura.Vencimento)

            If VencimentoCorreto Then

                fatura.Pendente = FaturaPendente

                fatura.Total = faturaTD.FindElements(By.TagName("td"))(1).Text.Replace(".", ",")

                If fatura.Baixada = False Then
                    AbrirModalEBaixarFatura(faturaTD, fatura)
                Else
                    RaiseEvent FaturaChecada(fatura)
                End If

                Return True
            End If
        Next

        Return False
    End Function

    Private Sub AbrirModalEBaixarFatura(faturaTD As IWebElement, fatura As Fatura)

        faturaTD.FindElement(By.XPath("td[3]/button")).Click()

        Dim ModalOpcoesArquivos = driver.FindElementByXPath("/html/body/div[6]/div/div/div/ul")

        If ModalOpcoesArquivos.Text.Contains("EXCEL") Then
            If BaixarFaturaPdfCSV(fatura) Then
                RaiseEvent FaturaBaixada(fatura)
            End If
        Else
            If BaixarFaturaPdf(fatura) Then
                RaiseEvent FaturaBaixadaPDF(fatura)
            End If
        End If




    End Sub

    Private Function BaixarFaturaPdf(fatura As Fatura) As Boolean


        Dim downloadtime = Now

        'baixar 2ª VIA
        driver.FindElementByXPath($"/html/body/div[6]/div/div/div/ul/li[1]/button").Click()

        'espera o load desaparecer
        Dim wait As New WebDriverWait(driver, New TimeSpan(0, 0, 59))
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='main']/div[2]/div/div/img")))


        If AguardaEConfirmaDwonload(60, downloadtime) Then
            Return True
        Else
            Throw New FaturaNotDownloadedException(fatura, $"Falha no Download, fatura não encontrada {Now.ToShortTimeString}", True)
        End If


    End Function

    Private Function VerificarVencimentoComDiasUteis(faturaTD As IWebElement, vencimento As Date) As Boolean

        Dim DataQueVamosVerificar As Date = faturaTD.FindElement(By.XPath("td[1]/span")).Text


        If vencimento.DayOfYear + 3 >= DataQueVamosVerificar.DayOfYear And
            DataQueVamosVerificar.DayOfYear >= vencimento.DayOfYear Then
            Return True
        Else
            Return False

        End If

    End Function

    Private Function BaixarFaturaPdfCSV(fatura As Fatura) As Boolean

        Dim downloadtime = Now
        Dim ModalOpcoesArquivos = driver.FindElementByXPath("/html/body/div[6]/div/div/div/ul")
        Dim NrDeOpcoes = ModalOpcoesArquivos.FindElements(By.TagName("li")).Count



        downloadtime = Now
        Try
            driver.FindElementByXPath("/html/body/div[6]/div/div/div/ul/li[1]/button").Click()
        Catch ex As ElementNotInteractableException
            driver.FindElementByXPath("/html/body/div[6]/div/div/div/ul/li[2]/button").Click()
        End Try

        'espera o load desaparecer
        Dim wait As New WebDriverWait(driver, New TimeSpan(0, 0, 59))
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='main']/div[2]/div/div/img")))

        Dim AvisoPraVoltarDepois = VerificarSeExisteModal()

        'checa se existe aviso pra voltar mais tarde
        If AvisoPraVoltarDepois Then
            Throw New FaturaNaoDisponivelException(fatura, "O Detalhamento precisa ser gerado e ficará disponível em uma hora")
        End If

        If AguardaEConfirmaDwonload(60, downloadtime) Then
            GoTo BaixarCsv
        Else
            Throw New FaturaNotDownloadedException(fatura, $"Falha no Download, fatura não encontrada {Now.ToShortTimeString}", True)
        End If

BaixarCsv:

        'baixar csv
        driver.FindElementByXPath($"/html/body/div[6]/div/div/div/ul/li[{NrDeOpcoes - 1}]/button").Click()

        'espera o load desaparecer
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='main']/div[2]/div/div/img")))

        AvisoPraVoltarDepois = VerificarSeExisteModal()
        'checa se existe aviso pra voltar mais tarde
        If AvisoPraVoltarDepois Then
            Throw New FaturaNaoDisponivelException(fatura, "O Detalhamento precisa ser gerado e ficará disponível em uma hora")
        End If


        If AguardaEConfirmaDwonload(60, downloadtime) Then
            Return True
        Else
            Throw New FaturaNotDownloadedException(fatura, $"Falha no Download, fatura não encontrada {Now.ToShortTimeString}", True)
        End If


    End Function

    Private Function VerificarSeExisteModal() As Object

        'checa se existe aviso
        If Utilidades.ChecarPresenca(driver, "//*[@id='main']/div[5]/div/div/div/div[2]/div/form/p") Then
            'checa se o aviso está visível
            If driver.FindElementByXPath("//*[@id='main']/div[5]/div/div/div/div[2]/div/form/p").Displayed Then
                'checa se o texto do aviso manda voltar depois
                If driver.FindElementByXPath("//*[@id='main']/div[5]/div/div/div/div[2]/div/form/p").Text _
                    .Contains("Sua fatura será gerada e logo mais estará disponível") Then
                    'fecha o modal e lança excessão
                    driver.FindElementByXPath("//*[@id='main']/div[5]/div/div/div/div[1]/div[2]/button").Click()
                    Return True
                End If
            End If
        End If

        Return False
    End Function

    Private Sub PosicionarConta(fatura As Fatura)

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)

        Try
            Dim selectEmpresa = driver.FindElementById("unit")
            Dim SeletorEmpresa = New SelectElement(selectEmpresa)
            SeletorEmpresa.SelectByText(conta.Empresa.CNPJ)
        Catch ex As NoSuchElementException
            Throw New ContaNaoCadasTradaException(conta.Faturas.First, "Este cnpj não está cadastrado para esta senha")
        End Try

        Dim wait As New WebDriverWait(driver, New TimeSpan(0, 0, 59))
        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='main']/div[2]/div/div/img")))

        If driver.FindElementById("account-billing-switcher").Text = conta.NrDaConta Then
            Exit Sub
        Else
            driver.FindElementById("account-billing-switcher").Click()
            Dim ContasContaner = driver.FindElementById("account-billing-switcher__listbox")
            Dim ListaDeContas As New Dictionary(Of String, IWebElement)

            For Each contaLI In ContasContaner.FindElements(By.TagName("li"))
                ListaDeContas.Add(contaLI.GetAttribute("innerHTML"), contaLI)
            Next

            If ListaDeContas.Keys.Contains(conta.NrDaConta) Then

                If ListaDeContas(conta.NrDaConta).Displayed Then
                    ListaDeContas(conta.NrDaConta).Click()
                    Exit Sub
                Else
                    Stop
                End If
            Else
                Throw New ContaNaoCadasTradaException(fatura, "Esta conta não está cadastrada para esta empresa", False)

            End If

        End If


        Dim vencimento
        Try
            vencimento = driver.FindElementByXPath("//*[@id='root']/main/div/div/div[1]/section/header/ul/li[3]/button/span").Text
        Catch ex As Exception
            vencimento = driver.FindElementByXPath("//*[@id='root']/main/div/div[1]/ul/li[3]/button/span").Text
        End Try


        vencimento = vencimento.Replace("dia ", "")

        conta.Vencimento = vencimento
        fatura.Vencimento = New Date(2019, 8, vencimento)


    End Sub


End Class


Public Interface IContaPageAlgar
    Inherits IContaPage

    Event FaturaBaixadaPdf(fatura As Fatura)
End Interface