Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium

Public Class ContaPageVivoFixo
    Inherits DriverDependents
    Implements IContaPageVivoFixo

    Public Event FaturaBaixada(fatura As Fatura) Implements IContaPage.FaturaBaixada
    Public Event FaturaChecada(fatura As Fatura) Implements IContaPage.FaturaChecada
    Public Event FaturaBaixadaCSV(fatura As Fatura) Implements IContaPage.FaturaBaixadaCSV

    Public Sub BuscarFatura(fatura As Fatura) Implements IContaPage.BuscarFatura

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)

        DesabilitarAvisoDeDadosCadastrais()

        Dim posicionadorCNPj As New PosicionadorCNPJVivoMovel(driver, fatura)
        posicionadorCNPj.poscionarCNPJ(fatura)

        Dim posicionadorproduto As New PosicionadorProdutoVivo(driver, fatura)

        Dim produto As String

        Try
            produto = Dicionarios.ProdutosVivo(conta.Subtipo)
        Catch ex As KeyNotFoundException
            Throw New ProdutoNaoCadastradoException(conta, "Produto não mapeado: " + conta.Subtipo.ToString, False)
        End Try

        posicionadorproduto.posicionarProduto(produto, fatura)

        If conta.Subtipo = SubtipoEnum.InternetCorp Then
            BaixarFaturasInternetCorp(fatura)

        End If

        Stop





    End Sub

    Private Sub BaixarFaturasInternetCorp(fatura As Fatura)

        Stop
        driver.SwitchTo.Frame(0)

        driver.FindElementByXPath("//*[@id='#tabs-2']").Click()

        Dim x As Integer = ObterIndice(fatura)

        BaixarResumoPdf(fatura, x)

        Stop

        '//*[@id="content"]/div/div/div/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[1]


        '//*[@id="content"]/div/div/div/div[1]/div/div/div/p[2]

        '//*[@id="content"]/div/div/div/div[1]/div/div/div/div[2]/table/tbody/tr[1]/td[1]
    End Sub

    Private Sub BaixarResumoPdf(fatura As Fatura, x As Integer)

        Dim QuadroFatura = driver.FindElementByXPath($"//*[@id='content']/div/div/div/div[1]/div/div/div/div[{x}]")

        Dim rows = QuadroFatura.FindElements(By.TagName("tr"))
        Dim hora = Now

        For Each row In rows
            If row.Text.Contains(fatura.Vencimento.ToString("dd/MM/yyyy")) Then

                Dim tds = row.FindElements(By.TagName("td"))

                fatura.Pendente = Not tds(3).Text = "Pago"
                fatura.Total = tds(2).Text
                tds(4).Click()

                If Utilidades.AguardaEConfirmaDwonload(30, hora) Then
                    RaiseEvent FaturaBaixada(fatura)
                    Exit Sub
                Else
                    Throw New FaturaNotDownloadedException(fatura, "erro no donwnload", True)
                End If

            End If
        Next

        Throw New FaturaNaoDisponivelException(fatura, "fatura não disponível, o último vencimenot disponível foi " +
                                               QuadroFatura.FindElement(By.XPath("/table/tbody/tr[1]/td[2]")).Text)

    End Sub

    Private Function ObterIndice(fatura As Fatura) As Integer

        Dim QuadroSegVia = driver.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div")

        Dim contas = QuadroSegVia.FindElements(By.TagName("p"))

        Dim contasstr As String

        For Each conta In contas
            Dim nrconta = Regex.Match(conta.Text, "\d+").Value
            contasstr += " " + nrconta
        Next


        For i = 0 To contas.Count - 1
            If contas(i).Text.Contains(fatura.NrConta) Then
                Return i + 1
            End If
        Next

        Throw New ContaNaoCadasTradaException(fatura, "número de ocnta não encontrada, contas diposníveis são: " + contasstr)

    End Function

    Private Sub DesabilitarAvisoDeDadosCadastrais()


        driver.SwitchTo.Frame(0)

        If Utilidades.ChecarPresenca(driver, "//*[@id='dialog-msg']/a") Then
            If driver.FindElementByXPath("//*[@id='dialog-msg']/a").Displayed Then
                driver.FindElementByXPath("//*[@id='dialog-msg']/a").Click()
            End If
        End If

    End Sub
End Class