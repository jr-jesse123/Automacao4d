Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI
Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium.Interactions

Public Class ContaPageClaro
    Inherits DriverDependents
    Implements IContaPageClaro

    Private driver As ChromeDriver
    Private _seletorConta As SelectElement
    Public Event FaturaBaixada(fatura As Fatura) Implements IContaPage.FaturaBaixada
    Public Event FaturaChecada(fatura As Fatura) Implements IContaPage.FaturaChecada
    Public Event FaturaBaixadaCSV(fatura As Fatura) Implements IContaPage.FaturaBaixadaCSV

    Public Sub BuscarFatura(fatura As Fatura) Implements IContaPage.BuscarFatura
        driver.Navigate.GoToUrl("https://contaonline.claro.com.br/webbow/downloadPDF/init.do")

        SelecionarConta(fatura)

        SelecionarFatura(fatura)

        If fatura.Baixada = False Then
            If BaixarFatura(fatura) Then
                RaiseEvent FaturaBaixada(fatura)
            End If
        Else
            ChecharFatura(fatura)
            RaiseEvent FaturaChecada(fatura)
        End If

    End Sub

    Private Sub SelecionarConta(fatura As Fatura)
        Dim ElementoSeletorConta
        Try
            ElementoSeletorConta = driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[5]/select")
        Catch ex As NoSuchElementException
            If driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[5]").Text _
                = fatura.NrConta Then
                Exit Sub
            Else
                Throw New ContaNaoCadasTradaException(GerRelDB.EncontrarContaDeUmaFatura(fatura), "Conta não cadastrada para este cliente")
            End If

        End Try


        Dim SeletorConta = New SelectElement(ElementoSeletorConta)

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)

        Try
            SeletorConta.SelectByText(fatura.NrConta)
        Catch ex As NoSuchElementException
            Throw New ContaNaoCadasTradaException(conta, "conta não cadastrada para este cliente")
        End Try

    End Sub

    Private Sub ChecharFatura(fatura As Fatura)

        Dim hover As New Actions(driver)

        Dim pagamentos = driver.FindElementByXPath("/html/body/table/tbody/tr/td[1]/ul/table/tbody/tr/td[3]")

        hover.MoveToElement(pagamentos).Build.Perform()

        'clica em boletos
        driver.FindElementByXPath("/html/body/table/tbody/tr/td[1]/ul/table/tbody/tr/td[3]/li/ul/li[1]/a").Click()

        Dim TabelaBoletos = driver.FindElementById("tableId")

        Dim boletos = TabelaBoletos.FindElements(By.TagName("tr"))

        For Each boleto As IWebElement In boletos
            If boleto.Text.Contains(fatura.Vencimento.ToString("dd/MM/yyyy")) _
                Or boleto.Text.Contains(fatura.Vencimento.AddDays(1).ToString("dd/MM/yyyy")) _
                Or boleto.Text.Contains(fatura.Vencimento.AddDays(2).ToString("dd/MM/yyyy")) Then


                If boleto.Text.Contains("Fechada") Then
                    fatura.Pendente = False
                End If
            End If
        Next


    End Sub

    Private Sub SelecionarFatura(fatura As Fatura)

        Dim OpcoesFaturas = driver.FindElementByXPath("/html/body/center/form/table/tbody/tr[4]/td/select")
        Dim selectFatura As New SelectElement(OpcoesFaturas)

        Try
            selectFatura.SelectByText(fatura.Vencimento.ToString("dd/MM/yyyy"), True)
            fatura.Referencia = ObterReferenciaDoSeletor(selectFatura.SelectedOption.Text)
        Catch ex As NoSuchElementException
            Throw New FaturaNaoDisponivelException(fatura, "Fatura não disponível")
        End Try

    End Sub

    Private Function ObterReferenciaDoSeletor(text As String) As String

        Dim datas() As Match = Utilidades.Regex("\d{1,2}\/\d{4}", text)

        Dim rawReferencia = datas.Last.Value.Split("/")

        If rawReferencia(0).Length = 1 Then
            rawReferencia(0) = "0" + rawReferencia(0)
        End If

        rawReferencia(1) = rawReferencia(1).Substring(2, 2)

        Return rawReferencia(0) + rawReferencia(1)

    End Function

    Private Function BaixarFatura(fatura As Fatura) As Boolean

        Dim downloadtime = Now
        driver.FindElementByXPath("/html/body/center/form/table/tbody/tr[6]/td/input").Click()

        If Utilidades.AguardaEConfirmaDwonload(60, downloadtime) Then
            Return True
        Else
            Throw New FaturaNotDownloadedException(fatura, $"Falha no Download, fatura não encontrada {Now.ToShortTimeString}", True)
            Return False
        End If

    End Function

    Public Sub New()
        Me.driver = WebdriverCt.Driver
    End Sub

End Class

Public Interface IContaPageClaro
    Inherits IContaPage
End Interface
