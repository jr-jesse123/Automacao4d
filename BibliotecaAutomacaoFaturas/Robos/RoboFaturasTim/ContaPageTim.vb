Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI

Public Class ContaPageTim
    Inherits DriverDependents
    Implements IContaPageTim
    Public Event FaturaBaixada(fatura As Fatura) Implements IContaPage.FaturaBaixada
    Public Event FaturaChecada(fatura As Fatura) Implements IContaPage.FaturaChecada
    Public Event FaturaBaixadaCSV(fatura As Fatura) Implements IContaPage.FaturaBaixadaCSV

    Public Sub BuscarFatura(Fatura As Fatura) Implements IContaPage.BuscarFatura
        Dim QuadroUltimaFatura
inicio:
        NavegarParaContas(Fatura)

        Try

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
                        End If
                    Else
                        Throw New FaturaNaoDisponivelException(Fatura, "Fatura não aparece entre as faturas disponíveis, pode ainda não estar disponível, ter sido cancelada ou ser muito antiga", False)
                    End If
                Else
                    Fatura.Pendente = Not UltimaFaturaText Like "*Pago*" And Not UltimaFaturaText Like "*Parcelado*"
                    RaiseEvent FaturaChecada(Fatura)
                End If

            Else
                ProcurarNasDemaisFaturas(Fatura)




            End If

        Catch ex As StaleElementReferenceException
            GoTo inicio
        End Try

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

        If UltimaFaturaText = ""
            Return false
        End If

        Dim vencimento
        Dim regexer As New Regexer
        Dim PreVencimento = regexer.PesquisarTexto("(\d+)/(\d+)/(\d+)", UltimaFaturaText).FirstOrDefault
        Dim PreConta = regexer.PesquisarTexto("\d\.\d{4,}(\.\d+)?", UltimaFaturaText).FirstOrDefault
        Dim contaNrPagina = PreConta.Value.Replace(".", "")


        If PreVencimento IsNot Nothing Then
            vencimento = PreVencimento.Value
        Else
            Return False
        End If

        If (fatura.Vencimento.ToString("dd/MM/yyyy") = vencimento) And
            contaNrPagina = fatura.NrConta Then

            fatura.Pendente = Not UltimaFaturaText Like "*Pago*" And Not UltimaFaturaText Like "*Parcelado*"
            Return True
        Else
            Return False
        End If
    End Function


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

            Throw New FalhaDownloadExcpetion(fatura, $"Falha no Download, fatura não encontrada {Now.ToShortTimeString}")

            Return False
        End If

    End Function

    Public Sub ProcurarNasDemaisFaturas(fatura As Fatura)
        Dim encontrado As Boolean = False

        Dim VerFaturasXpath = "//*[@id='allInvoicesForm']/button"
        driver.FindElementByXPath(VerFaturasXpath).Click()


        'ecm-loading
        '/html/body/div[13]/div/div/div

        Dim wait As New WebDriverWait(driver, New TimeSpan(0, 0, 59))

        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("ecm-loading")))

        Dim QuadroFaturas = driver.FindElementsByClassName("invoices-list-item").ToList

        Dim Faturastr As String = fatura.NrConta.First.ToString + "." + fatura.NrConta.Substring(1, 7)

        'tenta adicionar o sufixo se houver 
        Try
            Faturastr += "." + fatura.NrConta.Substring(9).ToString
        Catch ex As ArgumentOutOfRangeException

        End Try


        Dim QuadroFaturasConta = QuadroFaturas.Where(Function(qf) qf.Text.Contains($"{Faturastr}
Vencimento:")).ToList

        If QuadroFaturasConta.Count = 0 Then
            Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)
            Throw New ContaNaoCadasTradaException(conta, "Conta não cadastratada para esta empresa", False)
        End If



        For Each Quadro In QuadroFaturasConta

            If ChecarInformaCoesEValidarFAtura(fatura, Quadro.Text) Then 'passar fatura como padarametro
                encontrado = True
                If fatura.Baixada = False Then
                    If ExpandirQuadroUltimaFatura(Quadro, True) Then
                        If BaixarFatura(fatura) Then
                            RaiseEvent FaturaBaixada(fatura)
                            Exit Sub
                        End If
                    End If

                Else
                    RaiseEvent FaturaChecada(fatura)
                    Exit Sub
                End If
            End If
        Next

        Dim regexer As New Regexer
        Dim ultimoVencimentoVencimento = regexer.PesquisarTexto("(\d+)/(\d+)/(\d+)", QuadroFaturasConta.First.Text)(0).Value
        Throw New FaturaNaoDisponivelException(fatura, $"Fatura não disponibilizada, Ultimo Vencimento foi {ultimoVencimentoVencimento}", True)

    End Sub

End Class

Public Interface IContaPageTim
    Inherits IContaPage
End Interface
