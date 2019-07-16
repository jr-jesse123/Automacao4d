Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI
Imports SeleniumExtras.WaitHelpers

Public Class ContaPageTim
    Private driver As ChromeDriver
    Property Fatuas As List(Of Fatura)
    Public Event FaturaBaixada(conta As Conta)


    Public Sub New()
        Me.driver = WebdriverCt.Driver
    End Sub


    Public Sub PrepararDownloadUltimaFatura(conta As Conta)

        driver.Navigate.GoToUrl("https://meutim.tim.com.br/menu/minha-conta/conta-online")
        driver.SwitchTo.Frame(0)

        Dim QuadroUltimaFatura = driver.FindElementById("listInvoicesMyLast")
        Dim Vencimento = QuadroUltimaFatura.FindElement(By.XPath("/div/div[2]/div[1]/div[1]/div/div[3]")).Text



        Dim ExpansorFaturaBtn = QuadroUltimaFatura.FindElement(By.ClassName("icon-toggle"))
        ExpansorFaturaBtn.Click()
        Dim fatura = driver.FindElementsByTagName("strong")
        Dim IdOpcoesFormatos As String

        Try
            IdOpcoesFormatos = $"listInvoicesMyLastInvoice{fatura(2).Text}DownloadDropdownMenu"
            QuadroUltimaFatura.FindElement(By.Id(IdOpcoesFormatos)).Click()
        Catch ex As NoSuchElementException
            IdOpcoesFormatos = $"listInvoicesMyLastInvoice{fatura(3).Text}DownloadDropdownMenu"
            QuadroUltimaFatura.FindElement(By.Id(IdOpcoesFormatos)).Click()
        End Try

        Dim opcaoPDF = QuadroUltimaFatura.FindElements(By.ClassName("text-uppercase"))
        opcaoPDF(2).Click()

        Threading.Thread.Sleep(500)
        Dim DetalhadoIlimitado = "//*[@id='modalInvoiceDownloadPdf']/div/div/div[2]/form/div[3]/label/input"
        driver.FindElementByXPath(DetalhadoIlimitado).Click()

        BaixarFatura(conta)
    End Sub

    Private Sub BaixarFatura(conta As Conta)
        Dim Downloadtime = Now

        Dim xpathBaixar = "//*[@id='modalInvoiceDownloadPdf']/div/div/div[2]/form/div[5]/button"
        driver.FindElementByXPath(xpathBaixar).Click()


        If AguardaEConfirmaDwonload(60, Downloadtime) Then

            Dim espera As New WebDriverWait(driver, New TimeSpan(0, 0, 59))
            Dim by As By = By.XPath("//*[@id='modalDownloadLabel']")

            espera.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(by))


            RaiseEvent FaturaBaixada(conta)
        Else
            Throw New FalhaDownloadExcpetion
        End If

    End Sub

    Public Sub BaixarTodasFaturas()
        Throw New NotImplementedException

        AbrirnovoLink(driver, By.XPath(""))
        driver.FindElementById("download").Click()

    End Sub

    Public Sub VerTodasAsFAturas()
        Dim VerFaturasXpath = "//*[@id='allInvoicesForm']/button"
        driver.FindElementByXPath(VerFaturasXpath).Click()
    End Sub

    Public Function ChecarTodasFaturas() As List(Of Fatura)
        Dim faturas As New List(Of Fatura)

        Dim faturasForm = driver.FindElementsByClassName("invoices-list-item has-invoice-selector pb-2")

        For Each fatura In faturasForm
            Dim faturafow As New Fatura With {
            .Vencimento = fatura.FindElement(By.XPath("/div[2]/dl/dd")).Text,
            .Pendente = fatura.FindElement(By.XPath("/div/dl/dd")).Text,
            .ValorOriginal = fatura.FindElement(By.TagName("h2")).Text}
            faturas.Add(faturafow)
        Next

        Return faturas
    End Function

End Class

