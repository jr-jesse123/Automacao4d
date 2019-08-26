Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports System.Threading
Imports OpenQA.Selenium.Interactions

Public Class posicionadorContaVivo
    Private driver As ChromeDriver
    Private fatura As Fatura
    Sub New(driver As ChromeDriver, fatura As Fatura)
        Me.driver = driver
        Me.fatura = fatura
    End Sub

    Public Sub PosicionarConta(fatura As Fatura)

        Dim NrDaConta = GerRelDB.EncontrarContaDeUmaFatura(fatura).NrDaConta

        Dim conta_atual

        Thread.Sleep(1000)

        'verifica novo tipo de erro
        If ChecarPresenca(driver, "//*[@id='faturamovel_portlet_1.formGridFaturas']/div/p") Then
            driver.Navigate.Refresh()
            Thread.Sleep(3000)
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
101:        driver.FindElementByXPath("//*[@id='formSelectedItem']/div[1]/input").SendKeys(fatura.NrConta)
102:        driver.FindElementByXPath("//*[@id='formSelectedItem']/div[2]/i[2]/span[1]").Click()

        Catch ex As Exception


            Dim menuContas As IWebElement
            Dim contas As IReadOnlyCollection(Of IWebElement)
            Dim contas2 As IReadOnlyCollection(Of IWebElement)

            If Erl() = 100 Or Erl() = 101 Or Erl() = 102 Then
                Thread.Sleep(3000)

                Dim x
                For x = 1 To 200

                    Dim ele = driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[3]/div/div[2]")
                    contas = ele.FindElements(By.ClassName("first_item"))
                    If x > contas.Count Then Exit For


                    Dim actions As Actions = New Actions(driver)
                    actions.MoveToElement(contas(x))
                    actions.Perform()
                    Thread.Sleep(1000)

                    If contas(x).GetAttribute("data-value") = NrDaConta Then
                        contas(x).Click()
                        Exit Sub
                    End If
                Next x

                Throw New ContaNaoCadasTradaException(fatura, "Fatura não cadastrada para este gestor", False)
            End If

        End Try


        Thread.Sleep(3000)


        'verifica novo tipo de erro
        If ChecarPresenca(driver, "//*[@id='faturamovel_portlet_1.formGridFaturas']/div/p") Then
            driver.Navigate.Refresh()
            Thread.Sleep(3000)
        End If
        '*****************************************************************************


        'VERIFICA SE TEM AVISO DE CONTA INVÁLIDA
        If ChecarPresenca(driver, "//*[@id='formSelectedItem']/div[1]/span[2]") Then
            driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[3]/button/div/span[2]").Click() ' fecha o menu
            Throw New ContaNaoCadasTradaException(fatura, "Esta Conta não está cadastrada para este gestor", False)
        End If
        '***************************************************************************************************************************

        'On Error GoTo erro
        driver.FindElementByXPath("//*[@id='formSelectedItem']/div[2]").Click()
        Thread.Sleep(4000)


        If ChecarPresenca(driver, "//*[@id='formSelectedItem']/div[1]/span[2]") Then
            Throw New ContaNaoCadasTradaException(fatura, "Esta conta não está cadastrada para este gestor", False)
        Else
            Exit Sub
        End If

    End Sub

End Class
