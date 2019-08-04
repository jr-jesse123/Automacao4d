Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI
Imports SeleniumExtras.WaitHelpers
Imports BibliotecaAutomacaoFaturas.ErroLoginExcpetion
Imports BibliotecaAutomacaoFaturas

Public Class ContaPageTim
    Implements IContaPage

    Private driver As ChromeDriver

    Public Event FaturaBaixada(fatura As Fatura) Implements IContaPage.FaturaBaixada
    Public Event FaturaChecada(fatura As Fatura) Implements IContaPage.FaturaChecada

    Public Sub New()
        Me.driver = WebdriverCt.Driver
    End Sub


    Public Sub BuscarFatura(Fatura As Fatura) Implements IContaPage.BuscarFatura
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
            If Fatura.Baixada = False Then
                If ExpandirQuadroUltimaFatura(QuadroUltimaFatura) Then
                    If BaixarFatura(Fatura) Then
                        RaiseEvent FaturaBaixada(Fatura)
                    Else
                        RaiseEvent FaturaChecada(Fatura)
                    End If
                Else
                    Throw New FaturaNaoDisponivelException(Fatura, "Fatura não aparece entre as faturas disponíveis, pode ainda não estar disponível, ter sido cancelada ou ser muito antiga", False)
                End If

            End If

        Else
            If Not ProcurarNasDemaisFaturas(Fatura) Then
                GerRelDB.AtualizarContaComLog(Fatura, $"Fatura não disponibilizada, Ultimo Vencimento foi {Vencimento}", True)
            End If

        End If

    End Sub

    Private Function ExpandirQuadroUltimaFatura(quadroUltimaFatura As IWebElement,
                                                Optional BuscaCompleta As Boolean = False) As Boolean
        Dim QuadroUltimaFaturaText
        Dim regexer As New Regexer

        Dim ExpansorFaturaBtn = quadroUltimaFatura.FindElement(By.ClassName("icon-toggle"))
        ExpansorFaturaBtn.Click()

        Threading.Thread.Sleep(1000)
        QuadroUltimaFaturaText = quadroUltimaFatura.Text


        If Not BuscaCompleta Then
            Dim IdOpcoesFormatos As String = regexer.PesquisarTexto("\d{10}", QuadroUltimaFaturaText)(0).Value
            IdOpcoesFormatos = $"listInvoicesMyLastInvoice{IdOpcoesFormatos}DownloadDropdownMenu"

            Try
                quadroUltimaFatura.FindElement(By.Id(IdOpcoesFormatos)).Click()
            Catch ex As NoSuchElementException
                Return False
            End Try

        Else
            Dim IdOpcoesFormatos As String = regexer.PesquisarTexto("\d{10}", QuadroUltimaFaturaText)(0).Value
            IdOpcoesFormatos = $"listInvoicesMyInvoice{IdOpcoesFormatos}DownloadDropdownMenu"
            '                    listInvoicesOthersInvoice3893502012DownloadDropdownMenu

            Try
                quadroUltimaFatura.FindElement(By.Id(IdOpcoesFormatos)).Click()
            Catch ex As NoSuchElementException
                IdOpcoesFormatos = $"listInvoicesOthersInvoice{regexer.PesquisarTexto("\d{10}", QuadroUltimaFaturaText)(0).Value}DownloadDropdownMenu"
                '                    listInvoicesOthersInvoice3893502012DownloadDropdownMenu
                '                    listInvoicesOthersInvoicelistInvoicesMyInvoice3893502012DownloadDropdownMenuDownloadDropdownMenu
                Try
                    quadroUltimaFatura.FindElement(By.Id(IdOpcoesFormatos)).Click()
                Catch ex2 As NoSuchElementException
                    Return False
                End Try

            End Try


        End If



        Dim opcaoPDF = quadroUltimaFatura.FindElements(By.ClassName("text-uppercase"))
        opcaoPDF.Where(Function(x) x.Text = "PDF").First.Click()

        Threading.Thread.Sleep(500)
        Dim DetalhadoIlimitado = "//*[@id='modalInvoiceDownloadPdf']/div/div/div[2]/form/div[3]/label/input"
        driver.FindElementByXPath(DetalhadoIlimitado).Click()

        Return True

    End Function

    Private Function ChecarInformaCoesEValidarFAtura(fatura As Fatura, UltimaFaturaText As String) As Boolean



        Dim vencimento
        Dim regexer As New Regexer
        Dim PreVencimento = regexer.PesquisarTexto("(\d+)/(\d+)/(\d+)", UltimaFaturaText).FirstOrDefault

        If PreVencimento IsNot Nothing Then
            vencimento = PreVencimento.Value
        Else
            Return False
        End If





        If (fatura.Vencimento.ToString("dd/MM/yyyy") = vencimento) Then
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

    Public Function ProcurarNasDemaisFaturas(fatura As Fatura) As Boolean Implements IContaPage.ProcurarNasDemaisFaturas
        Dim encontrado As Boolean = False

        Dim VerFaturasXpath = "//*[@id='allInvoicesForm']/button"
        driver.FindElementByXPath(VerFaturasXpath).Click()

        Threading.Thread.Sleep(3000)

        Dim QuadroFaturas = driver.FindElementsByClassName("invoices-list-item").ToList

        For Each Quadro In QuadroFaturas

            If ChecarInformaCoesEValidarFAtura(fatura, Quadro.Text) Then 'passar fatura como padarametro
                encontrado = True
                If fatura.Baixada = False Then
                    If ExpandirQuadroUltimaFatura(Quadro, True) Then
                        If BaixarFatura(fatura) Then
                            RaiseEvent FaturaBaixada(fatura)
                        End If
                    Else
                        Throw New FaturaNaoDisponivelException(fatura, "fatura não localizada, pode ser muito antiga ou ainda não estar disponível ou ter sido cancelada")
                    End If

                Else
                    RaiseEvent FaturaChecada(fatura)
                End If
            End If
        Next

        Return encontrado
    End Function


End Class

