Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "Microsoft.Extensions.Primitives" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports Microsoft.Extensions.Primitives
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "Microsoft.Extensions.Primitives" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome

Public Class ContaPageVivoFixo
    Inherits DriverDependents
    Implements IContaPageVivoFixo

    Public Event FaturaBaixada(fatura As Fatura) Implements IContaPage.FaturaBaixada
    Public Event FaturaChecada(fatura As Fatura) Implements IContaPage.FaturaChecada
    Public Event FaturaBaixadaCSV(fatura As Fatura) Implements IContaPage.FaturaBaixadaCSV

    Public Sub BuscarFatura(fatura As Fatura) Implements IContaPage.BuscarFatura

        driver.Navigate.GoToUrl("https://meuvivoempresas.vivo.com.br/meuvivoempresas/appmanager/portal/fixo?_nfpb=true&_nfls=false&_pageLabel=empNegMVivo2FixoPymesBook&pFlutua=true")

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

        If conta.Subtipo = SubtipoEnum.InternetCorp Or conta.Subtipo = SubtipoEnum.SlnVoz Then
            If Not fatura.Baixada Then
                BaixarFaturaModelo1(fatura)
            Else
                checarFaturaModelo1(fatura)
            End If



        ElseIf conta.Subtipo = SubtipoEnum.VozFixa Then

            If Not fatura.Baixada Then

                SelecionarConta(fatura)

                AbrirAbaFaturas()

                PosicionarFatura(fatura)

                ChecarFaturaVozFixaPosicionada(fatura)

                BaixarFaturaVozFixaPosicionada(fatura)


            Else
                '                Stop
                SelecionarConta(fatura)

                AbrirAbaFaturas()

                PosicionarFatura(fatura)

                ChecarFaturaVozFixaPosicionada(fatura)



            End If

            'Stop


        Else
            Stop
        End If



    End Sub

    Private Sub ChecarFaturaVozFixaPosicionada(fatura As Fatura)

        fatura.Pendente = Not driver.FindElementById("titulo").Text = "Sua conta está paga"

    End Sub

    Private Sub checarFaturaModelo1(fatura As Fatura)

        driver.SwitchTo.Frame(0)

        driver.FindElementByXPath("//*[@id='#tabs-2']").Click()

        Dim x As Integer = ObterIndice(fatura)

        ChecarModelo1(fatura, x)

    End Sub

    Private Sub ChecarModelo1(fatura As Fatura, x As Integer)

        Dim QuadroFatura = driver.FindElementByXPath($"//*[@id='content']/div/div/div/div[1]/div/div/div/div[{x}]")

        Dim rows = QuadroFatura.FindElements(By.TagName("tr"))
        Dim hora = DateTime.Now

        For Each row In rows
            If row.Text.Contains(fatura.Vencimento.ToString("dd/MM/yyyy")) Then
                Dim tds = row.FindElements(By.TagName("td"))

                fatura.Pendente = Not tds(3).Text = "Pago"
                fatura.Total = tds(2).Text

            End If
        Next

    End Sub

    Private Sub SelecionarConta(fatura As Fatura)


        Dim contaAtual As String
        Dim _contaAtual As String
        _contaAtual = driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[3]/button/div/span[2]").Text
        Dim seletor As IWebElement



#Disable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.

        Dim x As Boolean

        Dim texto1 As String = _contaAtual.RemoverCaracter("(", ")", "-", " ")

        x = fatura.NrConta.ToString

        If x Then
#Enable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
            Exit Sub
        Else
            seletor = driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[3]/button")
            seletor.Click()

            Dim QuadroContas As IWebElement = driver.FindElementByXPath("/html/body/div[1]/div/div[2]/div/div[1]/div[2]/div[3]/div/div/form/ul")

            Dim contas As IReadOnlyCollection(Of IWebElement) = QuadroContas.FindElements(By.ClassName("first_item")).ToList

            For Each conta In contas

                Dim nr As String = conta.GetAttribute("data-value").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")

                If nr = fatura.NrConta Then

                    conta.FindElement(By.XPath("../..")).Click()
                    Exit Sub
                End If

            Next
            Dim _conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)
            Throw New ContaNaoCadasTradaException(_conta, "Conta não cadastrada para este gestor")

        End If


        Throw New NotImplementedException()
    End Sub

    Private Sub BaixarFaturaModelo1(fatura As Fatura)


        driver.SwitchTo.Frame(0)

        driver.FindElementByXPath("//*[@id='#tabs-2']").Click()

        Dim x As Integer = ObterIndice(fatura)

        BaixarResumoPdf(fatura, x)

    End Sub

    Private Sub BaixarFaturaVozFixaPosicionada(fatura As Fatura)

        Dim hora As DateTime = DateTime.Now

        driver.FindElementByXPath("//*[@id='opcoes-fatura-hover']").Click()
        Try
            driver.FindElementById("opcao-segunda-via-conta").Click()
        Catch ex As NoSuchElementException
            driver.FindElementById("btn-segunda-via").Click()
        End Try


        If Utilidades.AguardaEConfirmaDwonload(60, hora) Then
            RaiseEvent FaturaBaixada(fatura)
        Else
            Throw New FaturaNotDownloadedException(fatura, "Falha no download da fatura")
        End If



    End Sub

    Private Sub PosicionarFatura(fatura As Fatura)


        driver.SwitchTo.ParentFrame()
        driver.SwitchTo.Frame(0)



        Dim RawRef As String = fatura.Vencimento.ToString("MMM/yy")
        Dim Ref As String = RawRef.Replace(RawRef.First, UCase(RawRef.First))



        Dim ChartDiv As IWebElement = driver.FindElementById("chart_div")

        Dim grafico As IWebElement = ChartDiv.FindElement(By.TagName("svg"))

        Dim texts As IReadOnlyCollection(Of IWebElement) = grafico.FindElements(By.TagName("g"))

        Dim datasstr = texts(7).GetAttribute("innerHTML")

        Dim datas As MatchCollection = Regex.Matches(datasstr, "\D{3}/\d{2}")


        Dim i As Integer = 0
        For Each _Data As Match In datas

            If _Data.Value = Ref Then
                Exit For
            Else
                i += 1
            End If

        Next

        Threading.Thread.Sleep(1000)

        Dim seletores = grafico.FindElements(By.TagName("circle"))

        Dim SeletorFatura = grafico.FindElement(By.Id($"circle-interno{i }")) 'retirei o menos -1 verificar se esta funcionando




        If SeletorFatura.Displayed Then



            Try
                SeletorFatura.Click()
            Catch ex As ElementNotInteractableException

            End Try

        End If


    End Sub

    Private Function UCase(first As Char) As Char
        Return first.ToString.ToUpper
    End Function

    Private Sub AbrirAbaFaturas()


        Dim hover As New Interactions.Actions(driver)

        driver.SwitchTo.ParentFrame()
        driver.SwitchTo.Frame(0)

        Dim contasElement = driver.FindElementById("contas")

        hover.MoveToElement(contasElement).Build.Perform()

        driver.FindElementByXPath("//*[@id='menuItem2ViaContas']").Click()

    End Sub

    Private Sub BaixarFaturasSln(fatura As Fatura)

        driver.SwitchTo.Frame(0)

        driver.FindElementByXPath("//*[@id='#tabs-2']").Click()

        Dim x As Integer = ObterIndice(fatura)

        BaixarResumoPdf(fatura, x)

        '        Stop

        '//*[@id="content"]/div/div/div/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[1]


        '//*[@id="content"]/div/div/div/div[1]/div/div/div/p[2]


    End Sub

    Private Sub BaixarFaturasInternetCorp(fatura As Fatura)


        driver.SwitchTo.Frame(0)

        driver.FindElementByXPath("//*[@id='#tabs-2']").Click()

        Dim x As Integer = ObterIndice(fatura)

        BaixarResumoPdf(fatura, x)

        '        Stop

        '//*[@id="content"]/div/div/div/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[1]


        '//*[@id="content"]/div/div/div/div[1]/div/div/div/p[2]

        '//*[@id="content"]/div/div/div/div[1]/div/div/div/div[2]/table/tbody/tr[1]/td[1]
    End Sub

    Private Sub BaixarResumoPdf(fatura As Fatura, x As Integer)

        Dim QuadroFatura = driver.FindElementByXPath($"//*[@id='content']/div/div/div/div[1]/div/div/div/div[{x}]")

        Dim rows = QuadroFatura.FindElements(By.TagName("tr"))
        Dim hora = DateTime.Now

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
                    Throw New FaturaNotDownloadedException(fatura, "erro no donwnload")
                End If

            End If
        Next

        Throw New FaturaNaoDisponivelException(fatura, "fatura não disponível, o último vencimenot disponível foi " +
                                               QuadroFatura.Text.Substring(44, 10))

    End Sub

    Private Function ObterIndice(fatura As Fatura) As Integer

        Dim QuadroSegVia = driver.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div")

        Dim contas = QuadroSegVia.FindElements(By.TagName("p"))

        Dim contasstr As String

        For Each conta In contas
            Dim nrconta = Regex.Match(conta.Text, "\d+").Value
#Disable Warning BC42104 ' Variável "contasstr" é usada antes de receber um valor. Uma exceção de referência nula poderia resultar em runtime.
            contasstr += " " + nrconta
#Enable Warning BC42104 ' Variável "contasstr" é usada antes de receber um valor. Uma exceção de referência nula poderia resultar em runtime.
        Next


        For i = 0 To contas.Count - 1
            If contas(i).Text.Contains(fatura.NrConta) Then
                Return i + 1
            End If
        Next
        Dim _conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)
        Throw New ContaNaoCadasTradaException(_conta, "número de ocnta não encontrada, contas diposníveis são: " + contasstr)

    End Function

    Private Sub DesabilitarAvisoDeDadosCadastrais()

        Try
            driver.SwitchTo.Frame(0)
        Catch ex As Exception

        End Try


        If Utilidades.ChecarPresenca(driver, "//*[@id='dialog-msg']/a") Then
            If driver.FindElementByXPath("//*[@id='dialog-msg']/a").Displayed Then
                driver.FindElementByXPath("//*[@id='dialog-msg']/a").Click()
            End If
        End If

    End Sub

End Class



Public Class posicionadorContaVivoFixo
    Private driver As ChromeDriver
    Private fatura As Fatura

    Public Sub New(driver As ChromeDriver, fatura As Fatura)
        Me.driver = driver
        Me.fatura = fatura
    End Sub

    Friend Sub PosicionarConta(fatura As Fatura)
        Throw New NotImplementedException()
    End Sub
End Class
