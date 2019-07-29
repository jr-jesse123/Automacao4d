Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI
Imports SeleniumExtras.WaitHelpers
Imports BibliotecaAutomacaoFaturas.ErroLoginExcpetion
Imports BibliotecaAutomacaoFaturas

Public Class ContaPageTim
    Private driver As ChromeDriver

    Public Event FaturaBaixada(fatura As Fatura)


    Public Sub New()
        Me.driver = WebdriverCt.Driver
    End Sub


    Public Sub PrepararDownloadUltimaFatura(Fatura As Fatura)

  

        Dim QuadroUltimaFatura

        NavegarParaContas(Fatura)



        Try
            QuadroUltimaFatura = driver.FindElementById("listInvoicesMyLast")
        Catch ex As NoSuchElementException
            Throw New RoboFaturaException(Fatura, "Portal fora do ar")
        End Try

        Dim UltimaFaturaText = QuadroUltimaFatura.Text
        Dim regexer As New Regexer
        Dim Vencimento = regexer.PesquisarTexto("(\d+)/(\d+)/(\d+)", UltimaFaturaText)(0).Value



        If ChecarInformaCoesEValidarFAtura(Fatura, UltimaFaturaText) Then
            ExpandirQuadroUltimaFatura(QuadroUltimaFatura)
            If BaixarFatura(Fatura) Then
                RaiseEvent FaturaBaixada(Fatura)
            End If

        Else
            If Not ProcurarNasDemaisFaturas(Fatura) Then
                GerRelDB.EnviarLogFatura(Fatura, $"Fatura não disponibilizada, Ultimo Vencimento foi {Vencimento}", True)
            End If

        End If

    End Sub

    Private Sub ExpandirQuadroUltimaFatura(quadroUltimaFatura As IWebElement, Optional BuscaCompleta As Boolean = False)
        Dim QuadroUltimaFaturaText
        Dim regexer As New Regexer

        Dim ExpansorFaturaBtn = quadroUltimaFatura.FindElement(By.ClassName("icon-toggle"))
        ExpansorFaturaBtn.Click()

        Threading.Thread.Sleep(1000)
        QuadroUltimaFaturaText = quadroUltimaFatura.Text

        If Not BuscaCompleta Then
            Dim IdOpcoesFormatos As String = regexer.PesquisarTexto("\d{10}", QuadroUltimaFaturaText)(0).Value
            IdOpcoesFormatos = $"listInvoicesMyLastInvoice{IdOpcoesFormatos}DownloadDropdownMenu"
            quadroUltimaFatura.FindElement(By.Id(IdOpcoesFormatos)).Click()
        Else
            Dim IdOpcoesFormatos As String = regexer.PesquisarTexto("\d{10}", QuadroUltimaFaturaText)(0).Value
            IdOpcoesFormatos = $"listInvoicesMyInvoice{IdOpcoesFormatos}DownloadDropdownMenu"
            quadroUltimaFatura.FindElement(By.Id(IdOpcoesFormatos)).Click()

        End If



        Dim opcaoPDF = quadroUltimaFatura.FindElements(By.ClassName("text-uppercase"))
        opcaoPDF.Where(Function(x) x.Text = "PDF").First.Click()

        Threading.Thread.Sleep(500)
        Dim DetalhadoIlimitado = "//*[@id='modalInvoiceDownloadPdf']/div/div/div[2]/form/div[3]/label/input"
        driver.FindElementByXPath(DetalhadoIlimitado).Click()


    End Sub

    Private Function ChecarInformaCoesEValidarFAtura(fatura As Fatura, UltimaFaturaText As String) As Boolean



        Dim vencimento
        Dim regexer As New Regexer
        Dim PreVencimento = regexer.PesquisarTexto("(\d+)/(\d+)/(\d+)", UltimaFaturaText).FirstOrDefault

        If PreVencimento IsNot Nothing Then
            vencimento = PreVencimento.Value
        Else
            Return False
        End If





        If (fatura.Vencimento.ToString("dd/MM/yyyy") = Vencimento) Then
            fatura.Pendente = Not UltimaFaturaText Like "*Pago*"
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub ChecarInformaCoes(fatura As Fatura)
        Throw New NotImplementedException()
    End Sub

    Private Sub NavegarParaContas(fatura As Fatura)


        Try
            driver.Navigate.GoToUrl("https://meutim.tim.com.br/menu/minha-conta/conta-online")
            driver.SwitchTo.Frame(0)
        Catch ex As WebDriverException
            Try

                driver.Navigate.GoToUrl("https://meutim.tim.com.br/menu/minha-conta/conta-online")
                driver.SwitchTo.Frame(0)

            Catch ex2 As NoSuchElementException
                Throw New RoboFaturaException(fatura, "Portal fora do ar")



            End Try



        End Try


    End Sub

    Friend Sub RealizarChecagens(fatura As Fatura)
        Throw New NotImplementedException()
    End Sub

    Private Function BaixarFatura(fatura As Fatura) As Boolean
        Dim Downloadtime = Now

        Dim xpathBaixar = "//*[@id='modalInvoiceDownloadPdf']/div/div/div[2]/form/div[5]/button"
        driver.FindElementByXPath(xpathBaixar).Click()


        If AguardaEConfirmaDwonload(60, Downloadtime) Then

            Dim espera As New WebDriverWait(driver, New TimeSpan(0, 0, 59))
            Dim by As By = By.XPath("//*[@id='modalDownloadLabel']")

            espera.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(by))
            Return True
        Else

            Throw New FaturaNotDownloadedException(fatura, $"Falha no Download, fatura não encontrada {Now.ToShortTimeString}", True)

            Return False
        End If

    End Function

    Public Function ProcurarNasDemaisFaturas(fatura As Fatura) As Boolean

        Dim encontrado As Boolean = False

        Dim VerFaturasXpath = "//*[@id='allInvoicesForm']/button"
        driver.FindElementByXPath(VerFaturasXpath).Click()

        Threading.Thread.Sleep(2000)

        Dim QuadroFaturas = driver.FindElementsByClassName("invoices-list-item").ToList

        For Each Quadro In QuadroFaturas

            If ChecarInformaCoesEValidarFAtura(fatura, Quadro.Text) Then 'passar fatura como padarametro
                encontrado = True
                ExpandirQuadroUltimaFatura(Quadro, True)
                If BaixarFatura(fatura) Then
                    RaiseEvent FaturaBaixada(fatura)
                End If
            End If
        Next

        Return encontrado
    End Function


End Class

