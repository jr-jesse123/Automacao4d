
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.UI
Imports LibAutoFaturasStantard.Utilidades
Imports OpenQA.Selenium.Interactions

Public Class ContaPageOi
    Implements IContaPageOI

    Private driver As ChromeDriver
    Public Event FaturaBaixada As IContaPage.FaturaBaixadaEventHandler Implements IContaPage.FaturaBaixada
    Public Event FaturaChecada As IContaPage.FaturaChecadaEventHandler Implements IContaPage.FaturaChecada
    Public Event FaturaBaixadaCSV(fatura As Fatura) Implements IContaPage.FaturaBaixadaCSV

    Public Sub New()
        Me.driver = WebdriverCt.Driver
    End Sub

    Public Sub BuscarFatura(fatura As Fatura) Implements IContaPage.BuscarFatura

        Dim dataFormatada As String = OberDataFormatada(fatura.Vencimento)
        Dim numeroConta = fatura.NrConta


        For contador = 0 To 20
            Console.WriteLine(contador.ToString)
            Try
                SelecionarMesEAno(dataFormatada)
                SeleconarFatura(fatura)
                FazerDwonload(fatura)

                RaiseEvent FaturaBaixada(fatura)

                Exit Sub
            Catch ex As Exception
                driver.Navigate.Refresh()
            End Try
        Next


        GerRelDB.AtualizarContaComLogNaFatura(fatura, "impossível fazer download")

        Try

        Catch ex As Exception
            Stop
        End Try


    End Sub

    Private Sub FazerDwonload(fatura As Fatura)
        Dim esperaBotaoDownload As New WebDriverWait(driver, New TimeSpan(0, 0, 59))
#Disable Warning BC40000 ' '"ExpectedConditions" está obsoleto: "The ExpectedConditions implementation in the .NET bindings is deprecated and will be removed in a future release. This portion of the code has been migrated to the DotNetSeleniumExtras repository on GitHub (https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras)".
        esperaBotaoDownload.Until(ExpectedConditions.ElementIsVisible(By.Id("period-div")))
#Enable Warning BC40000 ' '"ExpectedConditions" está obsoleto: "The ExpectedConditions implementation in the .NET bindings is deprecated and will be removed in a future release. This portion of the code has been migrated to the DotNetSeleniumExtras repository on GitHub (https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras)".

        Dim horaAtual = DateTime.Now

        'centralizaa objeto por javascript
        Dim BtnDownload = driver.FindElementByClassName("online-account-download")

        Dim scrollElementIntoMiddle As String = "var viewPortHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);" _
                                            + "var elementTop = arguments[0].getBoundingClientRect().top;" _
                                            + "window.scrollBy(0, elementTop-(viewPortHeight/2));"

        CType(driver, IJavaScriptExecutor).ExecuteScript(scrollElementIntoMiddle, BtnDownload)
        '***************centralizaa objeto por javascript
        driver.FindElementByClassName("online-account-download").Click() ' clica no botão download


        If Not Utilidades.AguardaEConfirmaDwonload(30, horaAtual) Then
            Throw New FaturaNotDownloadedException(fatura, "Erro no download da fatura")
        End If


    End Sub

    Private Sub SeleconarFatura(fatura As Fatura)
        Dim esperaFaturas As New WebDriverWait(driver, New TimeSpan(0, 0, 59))
#Disable Warning BC40000 ' '"ExpectedConditions" está obsoleto: "The ExpectedConditions implementation in the .NET bindings is deprecated and will be removed in a future release. This portion of the code has been migrated to the DotNetSeleniumExtras repository on GitHub (https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras)".
        esperaFaturas.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='ng-app']/div[1]/div[2]/div/div/div[2]/div/div/div[2]/div[4]/div[1]")))
#Enable Warning BC40000 ' '"ExpectedConditions" está obsoleto: "The ExpectedConditions implementation in the .NET bindings is deprecated and will be removed in a future release. This portion of the code has been migrated to the DotNetSeleniumExtras repository on GitHub (https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras)".

        Threading.Thread.Sleep(1000)


        Dim vencFormatado As String = ("Conta Fatura nº: " + fatura.NrConta)
        Dim faturas = driver.FindElements(By.ClassName("online-account-subtitle"))

        For Each n In faturas

            If n.Text = vencFormatado Then
                Utilidades.CentralizarElementoComJs(driver, n)
                n.Click()

                Dim wait As New WebDriverWait(driver, New TimeSpan(0, 0, 59))
#Disable Warning BC40000 ' '"ExpectedConditions" está obsoleto: "The ExpectedConditions implementation in the .NET bindings is deprecated and will be removed in a future release. This portion of the code has been migrated to the DotNetSeleniumExtras repository on GitHub (https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras)".
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("sk-cube-grid")))
#Enable Warning BC40000 ' '"ExpectedConditions" está obsoleto: "The ExpectedConditions implementation in the .NET bindings is deprecated and will be removed in a future release. This portion of the code has been migrated to the DotNetSeleniumExtras repository on GitHub (https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras)".
                Exit Sub
            End If
        Next


        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)

        Throw New ContaNaoCadasTradaException(conta, "conta não cadastrada para este cliente")


    End Sub

    Private Sub SelecionarMesEAno(dataFormatada As String)
        Dim esperaMesAno As New WebDriverWait(driver, New TimeSpan(0, 0, 59))
#Disable Warning BC40000 ' '"ExpectedConditions" está obsoleto: "The ExpectedConditions implementation in the .NET bindings is deprecated and will be removed in a future release. This portion of the code has been migrated to the DotNetSeleniumExtras repository on GitHub (https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras)".
        esperaMesAno.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='period-div']/div[2]/select")))
#Enable Warning BC40000 ' '"ExpectedConditions" está obsoleto: "The ExpectedConditions implementation in the .NET bindings is deprecated and will be removed in a future release. This portion of the code has been migrated to the DotNetSeleniumExtras repository on GitHub (https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras)".

        Try
            Dim opcoesDefaturas = driver.FindElementByXPath("//*[@id='period-div']/div[2]/select")
            Dim selecionadorMesVencimento As New SelectElement(opcoesDefaturas)
            selecionadorMesVencimento.SelectByText(dataFormatada)
        Catch ex As NoSuchElementException
            'NÃO FAZER NADA
        End Try

    End Sub

    Private Function OberDataFormatada(vencimento As Date) As String

        Dim DataPreFormatada = String.Format(vencimento, "MMMM \de yyyy")
        Dim DataFormatada = DataPreFormatada.Replace(DataPreFormatada.First, Char.ToUpper((DataPreFormatada.First)))

        Return DataFormatada

    End Function



End Class