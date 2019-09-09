Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI
Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas

Public Class ContaPageClaro
    Inherits DriverDependents
    Implements IContaPageClaro

    Private driver As ChromeDriver
    Private _seletorConta As SelectElement
    Public Event FaturaBaixada(fatura As Fatura) Implements IContaPage.FaturaBaixada
    Public Event FaturaChecada(fatura As Fatura) Implements IContaPage.FaturaChecada
    Public Event FaturaBaixadaPDF(fatura As Fatura) Implements IContaPage.FaturaBaixadaPDF

    Public Sub BuscarFatura(fatura As Fatura) Implements IContaPage.BuscarFatura
        driver.Navigate.GoToUrl("https://contaonline.claro.com.br/webbow/downloadPDF/init.do")

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

    Private Sub ChecharFatura(fatura As Fatura)
        Throw New NotImplementedException()
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


    Public Sub BaixarUltimaFatura(Conta As String)
        _seletorConta.SelectByText(Conta)
        driver.FindElementByXPath("/html/body/center/form/table/tbody/tr[6]/td/input").Click()
    End Sub

End Class

Public Interface IContaPageClaro
    Inherits IContaPage
End Interface
