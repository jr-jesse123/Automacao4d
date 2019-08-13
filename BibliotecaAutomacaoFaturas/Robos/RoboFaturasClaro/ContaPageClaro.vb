Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI
Imports BibliotecaAutomacaoFaturas
Imports BibliotecaAutomacaoFaturas.ErroLoginExcpetion
Imports System.Text.RegularExpressions

Public Class ContaPageClaro
    Private driver As ChromeDriver
    Private _seletorConta As SelectElement
    Public Event FaturaBaixada(fatura As Fatura)
    Public Event FaturaChecada(fatura As Fatura)

    Private Property SeletorConta() As SelectElement
        Get
            If _seletorConta Is Nothing Then
                _seletorConta = New SelectElement(driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[5]/select"))
            End If
            Return _seletorConta
        End Get
        Set(ByVal value As SelectElement)
            _seletorConta = value
        End Set
    End Property

    Private _seletorFatura As SelectElement
    Private Property SeletorFatura() As SelectElement
        Get
            If _seletorFatura Is Nothing Then
                _seletorFatura = New SelectElement(driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[5]/select"))
            End If
            Return _seletorFatura
        End Get
        Set(ByVal value As SelectElement)
            _seletorFatura = value
        End Set
    End Property

    Friend Sub BuscarFatura(fatura As Fatura)
        driver.Navigate.GoToUrl("https://contaonline.claro.com.br/webbow/downloadPDF/init.do")

        SelecionarFatura(fatura)

        If fatura.Baixada = False Then
            If BaixarFatura(fatura) Then
                RaiseEvent FaturaBaixada(fatura)
            End If
        End If

        ChecarFatura(fatura)

    End Sub

    Private Sub ChecarFatura(fatura As Fatura)
        driver.Navigate.GoToUrl("https://contaonline.claro.com.br/webbow/payment/slipImageOneMultipleFatures.do")
        Dim SituacaoPagamentos = driver.FindElementById("tableId").FindElements(By.TagName("tr"))

        For Each situcao In SituacaoPagamentos
            If situcao.Text.StartsWith("Data") Or situcao.Text.StartsWith("Selecionar") Then Continue For
            If situcao.Text.Length > 0 Then
                Dim VencimentoSituacao As Date = situcao.FindElement(By.XPath("td[2]")).Text
                Dim FaturaCorreta = fatura.Vencimento.DayOfYear + 4 >= VencimentoSituacao.DayOfYear And
                   VencimentoSituacao.DayOfYear >= fatura.Vencimento.DayOfYear

                If FaturaCorreta Then
                    fatura.Pendente = situcao.FindElement(By.XPath("td[4]")).Text = "Aberta"
                    fatura.Total = situcao.FindElement(By.XPath("td[5]")).Text.Replace(".", ",")
                    fatura.Ajuste = situcao.FindElement(By.XPath("td[6]")).Text.Replace(".", ",")
                End If
            End If
        Next
        RaiseEvent FaturaChecada(fatura)

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

        If AguardaEConfirmaDwonload(60, downloadtime) Then
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
