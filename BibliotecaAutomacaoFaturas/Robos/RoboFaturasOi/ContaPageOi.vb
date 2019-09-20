Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.UI
Imports BibliotecaAutomacaoFaturas.Utilidades



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


        For contador = 0 To 50
            Console.WriteLine(contador.ToString)
            Try
                SelecionarMesEAno(dataFormatada)
                SeleconarFatura(numeroConta)
                FazerDwonload()

                BaixarFatura(fatura)

                GerRelDB.AtualizarContaComLogNaFatura(fatura, "Fatura Baixada", True)


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

    Private Sub FazerDwonload()
        Dim esperaBotaoDownload As New WebDriverWait(driver, New TimeSpan(0, 0, 59))
        esperaBotaoDownload.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='ng-app']/div[1]/div[2]/div/div/div[2]/div/div/div[2]/div[4]/div[2]/div[2]/div[2]/div/button[1]")))

        Dim horaAtual = Now
        Try
            driver.FindElementByClassName("online-account-download").Click() ' clica no botão download

        Catch ex As Exception

        End Try

        If Utilidades.AguardaEConfirmaDwonload(30, horaAtual) Then

        Else

            'não tem arquivo lá 
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

    Private Function BaixarFatura(fatura As Fatura) As Boolean

        Dim horaatual = Now
        If Utilidades.AguardaEConfirmaDwonload(60, horaatual) Then
            Return True
        Else
            Throw New FaturaNotDownloadedException(fatura, $"Falha no Download, fatura não encontrada {Now.ToShortTimeString}", True)
            Return False
        End If
    End Function


End Class