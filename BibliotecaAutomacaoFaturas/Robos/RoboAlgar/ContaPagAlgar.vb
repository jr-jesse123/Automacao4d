Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI
Imports BibliotecaAutomacaoFaturas
Imports BibliotecaAutomacaoFaturas.ErroLoginExcpetion
Imports System.Text.RegularExpressions

Public Class ContaPagAlgar

    Private driver As ChromeDriver
    Private _seletorConta As SelectElement
    Public Event FaturaBaixada(fatura As Fatura, TipoFatura As TipoFaturaEnum)
    Public Event FaturaChecada(fatura As Fatura)

    Friend Sub BuscarFatura(fatura As Fatura)
        Dim faturasFechadas, faturasAbertas, faturasVencidas As IWebElement


        'exibe todas as faturas, checar se é a mesma estrutura quando tem faturas pendentes ou a vencer
        driver.FindElementByXPath("//*[@id='root']/main/div/div/div[1]/section/div/div/a").Click()

        'exibe contas disponiveis
        

        '        On Error Resume Next
        faturasFechadas = driver.FindElementByXPath("//*[@id='root']/main/div/div[2]/div[3]/section/section/div/form/table/tbody")
        faturasAbertas = driver.FindElementByXPath("//*[@id='root']/main/div/div[2]/div[2]/section/section/div")
        faturasVencidas = driver.FindElementByXPath("")
        '       On Error GoTo 0


        If fatura.Vencimento.Day = 22 Then
            fatura.Vencimento = fatura.Vencimento.AddDays(-2)
        End If

        For Each faturaTD In faturasFechadas.FindElements(By.TagName("tr"))

            Dim VencimentoCorreto As Boolean = VerificarVencimentoComDiasUteis(faturaTD, fatura.Vencimento)

            If VencimentoCorreto Then

                If fatura.Baixada = False Then
                    baixarFaturaPDF_CSV(faturaTD, fatura)
                End If

                fatura.Pendente = False
                RaiseEvent FaturaChecada(fatura)
                Exit Sub
            End If
        Next

        For Each faturaTD In faturasAbertas.FindElements(By.TagName("tr"))
            If faturaTD.FindElement(By.XPath("td[1]/a")).Text = fatura.NrConta Then

                If fatura.Baixada = False Then
                    baixarFaturaPDF_CSV(faturaTD, fatura)
                End If

                fatura.Pendente = False
                RaiseEvent FaturaChecada(fatura)
                Exit Sub
            End If
        Next


        For Each faturaTD In faturasVencidas.FindElements(By.TagName("tr"))

            If faturaTD.FindElement(By.XPath("td[1]/a")).Text = fatura.NrConta Then

                If fatura.Baixada = False Then
                    baixarFaturaPDF_CSV(faturaTD, fatura)
                End If

                fatura.Pendente = False
                RaiseEvent FaturaChecada(fatura)
                Exit Sub
            End If
        Next


        Throw New FaturaNaoDisponivelException(fatura, "Fatura não Disponibilizada no site")

    End Sub

    Private Sub baixarFaturaPDF_CSV(faturaTD As IWebElement, fatura As Fatura)

        faturaTD.FindElement(By.XPath("td[3]/button")).Click()
        If BaixarFatura(fatura) Then
            RaiseEvent FaturaBaixada(fatura, TipoFaturaEnum.CSV)
        End If

        faturaTD.FindElement(By.XPath("td[3]/button")).Click()
        If BaixarFatura(fatura) Then
            RaiseEvent FaturaBaixada(fatura, TipoFaturaEnum.PDF)
        End If


    End Sub

    Private Function VerificarVencimentoComDiasUteis(faturaTD As IWebElement, vencimento As Date) As Boolean

        Dim DataQueVamosVerificar As Date = faturaTD.FindElement(By.XPath("td[1]/span")).Text


        If vencimento.DayOfYear + 3 >= DataQueVamosVerificar.DayOfYear And
            DataQueVamosVerificar.DayOfYear >= vencimento.DayOfYear Then
            Return True
        Else
            Return False

        End If

    End Function

    Private Function BaixarFatura(fatura As Fatura, Optional tipofatura As TipoFaturaEnum = TipoFaturaEnum.PDF) As Boolean

        Dim downloadtime = Now

        If tipofatura = TipoFaturaEnum.PDF Then
            Try
                driver.FindElementByXPath("/html/body/div[6]/div/div/div/ul/li[2]/button").Click()
            Catch ex As ElementNotInteractableException
                driver.FindElementByXPath("/html/body/div[6]/div/div/div/ul/li[3]/button").Click()
            End Try
        Else
            Try
                driver.FindElementByXPath("/html/body/div[6]/div/div/div/ul/li[1]/button").Click()
            Catch ex As ElementNotInteractableException
                driver.FindElementByXPath("/html/body/div[6]/div/div/div/ul/li[2]/button").Click()
            End Try
        End If


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
