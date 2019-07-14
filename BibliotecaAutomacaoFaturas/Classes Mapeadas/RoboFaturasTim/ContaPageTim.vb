﻿Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI
Imports SeleniumExtras.WaitHelpers

Public Class ContaPageTim
    Private driver As ChromeDriver
    Property Fatuas As List(Of Fatura)
    Public Event FaturaBaixada(conta As Conta)

    Public Sub New()
        Me.driver = ContainerWebdriver.Driver
    End Sub

    Public Sub BaixarUltimaFatura(conta As Conta)
        Dim setaUltimaFatura = "//*[@id='listInvoicesMyLast']/div/div[2]/div[1]/div[3]/div"
        driver.FindElementByXPath(setaUltimaFatura).Click()

        Dim IdOpcoesFormatos = "listInvoicesMyLastInvoice3865609223DownloadDropdownMenu"
        driver.FindElementById(IdOpcoesFormatos).Click()

        Dim opcaoPDF = "//*[@id='listInvoicesMyLastInvoice3865609223CollapseContext']/div[1]/div[3]/div[2]/ul/li[2]"
        driver.FindElementByXPath(opcaoPDF).Click()

        Dim DetalhadoIlimitado = "//*[@id='modalInvoiceDownloadPdf']/div/div/div[2]/form/div[3]/label/input"
        driver.FindElementByXPath(DetalhadoIlimitado).Click()

        Dim xpathBaixar = "//*[@id='modalInvoiceDownloadPdf']/div/div/div[2]/form/div[5]/button"
        driver.FindElementByXPath(xpathBaixar).Click()

        If AguardaEConfirmaDwonload(60, Now) Then

            Dim espera As New WebDriverWait(driver, New TimeSpan(0, 0, 59))
            Dim by As By = By.XPath("//*[@id='modalDownloadLabel']")

            espera.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By))


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

