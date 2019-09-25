Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.UI
Imports BibliotecaAutomacaoFaturas.Utilidades
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
                SeleconarFatura(numeroConta)
                FazerDwonload(fatura)

                RaiseEvent FaturaBaixada(fatura)

                Exit Sub
            Catch
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
        esperaBotaoDownload.Until(ExpectedConditions.ElementIsVisible(By.Id("period-div")))

        Dim horaAtual = Now

        'centralizaa objeto por javascript
        Dim BtnDownload = driver.FindElementByClassName("online-account-download")

        Dim scrollElementIntoMiddle As String = "var viewPortHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);" _
                                            + "var elementTop = arguments[0].getBoundingClientRect().top;" _
                                            + "window.scrollBy(0, elementTop-(viewPortHeight/2));"

        CType(driver, IJavaScriptExecutor).ExecuteScript(scrollElementIntoMiddle, BtnDownload)
        '***************centralizaa objeto por javascript
        driver.FindElementByClassName("online-account-download").Click() ' clica no botão download


        If Not Utilidades.AguardaEConfirmaDwonload(30, horaAtual) Then
            Throw New FaturaNotDownloadedException(fatura, "Erro no download da fatura", True)
        End If


    End Sub

    Private Sub SeleconarFatura(numeroConta As String)
        Dim esperaFaturas As New WebDriverWait(driver, New TimeSpan(0, 0, 59))
        esperaFaturas.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='ng-app']/div[1]/div[2]/div/div/div[2]/div/div/div[2]/div[4]/div[1]")))

        Try
            Dim vencFormatado As String = ("Conta Fatura nº: " + numeroConta)
            Dim faturas = driver.FindElements(By.ClassName("online-account-subtitle"))

            For Each n In faturas

                If n.Text = vencFormatado Then
                    n.Click()

                End If
            Next
        Catch ex As Exception

        End Try

    End Sub

    Private Sub SelecionarMesEAno(dataFormatada As String)
        Dim esperaMesAno As New WebDriverWait(driver, New TimeSpan(0, 0, 59))
        esperaMesAno.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='period-div']/div[2]/select")))

        Try
            Dim opcoesDefaturas = driver.FindElementByXPath("//*[@id='period-div']/div[2]/select")
            Dim selecionadorMesVencimento As New SelectElement(opcoesDefaturas)
            selecionadorMesVencimento.SelectByText(dataFormatada)
        Catch ex As NoSuchElementException
            'NÃO FAZER NADA
        End Try

    End Sub

    Private Function OberDataFormatada(vencimento As Date) As String

        Dim DataPreFormatada = Format(vencimento, "MMMM \de yyyy")
        Dim DataFormatada = DataPreFormatada.Replace(DataPreFormatada.First, UCase(DataPreFormatada.First))

        Return DataFormatada

    End Function



End Class