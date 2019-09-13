Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports System.Threading
Imports OpenQA.Selenium.Interactions
Imports OpenQA.Selenium.Support.UI

Public Class posicionadorContaVivo
    Private driver As ChromeDriver
    Private fatura As Fatura
    Sub New(driver As ChromeDriver, fatura As Fatura)
        Me.driver = driver
        Me.fatura = fatura
    End Sub

    Public Sub PosicionarConta(fatura As Fatura)

        Dim NrDaConta As String = GerRelDB.EncontrarContaDeUmaFatura(fatura).NrDaConta

        Dim conta_atual

        'verifica novo tipo de erro
        If ChecarPresenca(driver, "//*[@id='faturamovel_portlet_1.formGridFaturas']/div/p") Then
            driver.Navigate.Refresh()
            'Thread.Sleep(3000)
        End If


        ' verifica erro de visualização de conta com botão tentar novamente
        If ChecarPresenca(driver, "//*[@id='btnTentarNovamente']") Then
            driver.FindElementByXPath("//*[@id='btnTentarNovamente']").Click()

            If ChecarPresenca(driver, "//*[@id='btnTentarNovamente']") Then ' verifica erro de visualização de conta com botão tentar novamente
                conta_atual = driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[3]/button/div/span[2]").Text
                If NrDaConta = conta_atual Then
                    Throw New FaturaNaoDisponivelException(fatura, "A Fatura não pode ser visualizada", True)
                End If
            End If
        End If
        '***************************************************************************************************************************


        'checa se a conta já está posicionada
        Try
            '           On Error GoTo erro
            conta_atual = driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[3]/button/div/span[2]").Text
            '            On Error GoTo handler

        Catch ex As Exception
            Throw New RoboFaturaException(fatura, "Impossível Selecionar contas para este cnpj, tente mais tarde", True)
        End Try

        If NrDaConta = conta_atual Then
            Exit Sub
        End If
        '***************************************************************************************************************************


        'TENTA SELECCIONAR A CONTA SE DER ERRO É PQ A CONTA NÃO TA LÁ.
        Try

100:        driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[3]/button/div/span[2]").Click()
            Dim DivFormBuscarConta = driver.FindElementByClassName("account_search")
            Dim CaixaPesquisarConta = DivFormBuscarConta.FindElement(By.ClassName("search_input"))
101:        CaixaPesquisarConta.SendKeys(fatura.NrConta)
            Dim Lupa = DivFormBuscarConta.FindElement(By.TagName("img"))
            Lupa.Click()

        Catch ex As Exception 'ElementNotInteractableException

            Stop

            Dim contas As IReadOnlyCollection(Of IWebElement)
            For x = 0 To 200

                Dim ele = driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[3]/div/div[2]")
                contas = ele.FindElements(By.ClassName("first_item"))
                If x > contas.Count Then Exit For '-1



                Dim actions As Actions = New Actions(driver)
                Dim element As IWebElement
                Try
                    element = contas(x)
                Catch ex2 As ArgumentException
                    Throw New ContaNaoCadasTradaException(fatura, "Esta conta não está cadastrada para este gestor
", False)
                End Try

                Try
                    actions.MoveToElement(element)
                    actions.Perform()

                    If contas(x).GetAttribute("data-value") = NrDaConta Then
                        contas(x).Click()
                        Exit Sub
                    End If

                Catch ex3 As Exception
                    Try
                        ' codigo Javascript 

                        element = contas(x - 1)
                        Dim je As IJavaScriptExecutor = CType(driver, IJavaScriptExecutor)
                        je.ExecuteScript("arguments[0].scrollIntoView(true)", element)
                        '*********************************
                        Thread.Sleep(2000)
                    Catch ex4 As Exception
                        Throw New ContaNaoCadasTradaException(fatura, "Fatura não cadastrada para este gestor", False)
                    End Try

                    If contas(x - 1).GetAttribute("data-value") = NrDaConta Then
                        contas(x - 1).Click()
                        Exit Sub
                    End If


                End Try



            Next x

            Throw New ContaNaoCadasTradaException(fatura, "Fatura não cadastrada para este gestor", False)


        End Try

        Dim wait As New WebDriverWait(driver, New TimeSpan(0, 0, 15))

        wait.Until(ExpectedConditions.TextToBePresentInElementLocated(
            By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[3]/button/div/span[2]"),
            fatura.NrConta))




        'verifica novo tipo de erro
        If ChecarPresenca(driver, "//*[@id='faturamovel_portlet_1.formGridFaturas']/div/p") Then
            driver.Navigate.Refresh()
            Thread.Sleep(3000)
        End If
        '*****************************************************************************


        'VERIFICA SE TEM AVISO DE CONTA INVÁLIDA
        If ChecarPresenca(driver, "//*[@id='formSelectedItem']/div[1]/span[2]") Then
            If driver.FindElementByXPath("//*[@id='formSelectedItem']/div[1]/span[2]").Displayed Then
                driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[3]/button/div/span[2]").Click() ' fecha o menu
                Throw New ContaNaoCadasTradaException(fatura, "Esta Conta não está cadastrada para este gestor", False)
            End If
        End If
        '***************************************************************************************************************************


        If ChecarPresenca(driver, "//*[@id='formSelectedItem']/div[1]/span[2]") Then
            If driver.FindElementByXPath("//*[@id='formSelectedItem']/div[1]/span[2]").Displayed Then
                Throw New ContaNaoCadasTradaException(fatura, "Esta conta não está cadastrada para este gestor", False)
            End If
        Else
            Exit Sub
        End If

    End Sub

End Class
