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
    Public Event FaturaBaixada(fatura As Fatura)
    Public Event FaturaChecada(fatura As Fatura)

    Friend Sub BuscarFatura(fatura As Fatura)



        Dim faturasFechadas = driver.FindElementByXPath("//*[@id='root']/main/div/div[2]/div[3]/section/section/div/form/table/tbody")
        Dim faturasAbertas = driver.FindElementByXPath("//*[@id='root']/main/div/div[2]/div[2]/section/section/div")

        For Each faturaTD In faturasFechadas.FindElements(By.TagName("tr"))
            If faturaTD.FindElement(By.XPath("td[1]/a")).Text = fatura.NrConta Then

                If fatura.Baixada = False Then
                    faturaTD.FindElement(By.XPath("td[3]/button")).Click()
                    If BaixarFatura(fatura) Then
                        RaiseEvent FaturaBaixada(fatura)
                    End If
                End If

                fatura.Pendente = False
                RaiseEvent FaturaChecada(fatura)
                Exit Sub
            End If
        Next

        For Each faturaTD In faturasAbertas.FindElements(By.TagName("tr"))
            If faturaTD.FindElement(By.XPath("td[1]/a")).Text = fatura.NrConta Then

                If fatura.Baixada = False Then
                    faturaTD.FindElement(By.XPath("td[3]/button")).Click()
                    If BaixarFatura(fatura) Then
                        RaiseEvent FaturaBaixada(fatura)
                    End If
                End If

                fatura.Pendente = False
                RaiseEvent FaturaChecada(fatura)
                Exit Sub
            End If
        Next

        Throw New FaturaNaoDisponivelException(fatura, "Fatura não Disponibilizada no site")

    End Sub


    Private Function BaixarFatura(fatura As Fatura) As Boolean

        Dim downloadtime = Now

        Try
            driver.FindElementByXPath("/html/body/div[6]/div/div/div/ul/li[2]/button").Click()
        Catch ex As ElementNotInteractableException
            driver.FindElementByXPath("/html/body/div[6]/div/div/div/ul/li[3]/button").Click()
        End Try



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
