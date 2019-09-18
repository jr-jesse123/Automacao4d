Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium

Friend Class PosicionadorCNPJVivoMovel
    Private driver As ChromeDriver
    Private fatura As Fatura

    Public Sub New(driver As ChromeDriver, fatura As Fatura)
        Me.driver = driver
        Me.fatura = fatura
    End Sub
    Public Sub poscionarCNPJ(fatura As Fatura)

        Dim conta = GerRelDB.Contas.Where(Function(c) c.Faturas.Contains(fatura)).First

        FecharJanelaDeAvisosSeAparecer()

        Dim CNPJ_RAIZEncontrado = ConfirmaAparecimentoDoCNPJ()
        PosicionaCNPJRaiz(CNPJ_RAIZEncontrado, conta.Empresa.CNPJ, fatura)
        PosicionCnpjFilial(CNPJ_RAIZEncontrado, conta.Empresa.CNPJ, fatura)

    End Sub

    Private Sub FecharJanelaDeAvisosSeAparecer()

        'fecha janela de avisos da tela inicial se existir
        If ChecarPresenca(driver, "//*[@id='dialog-msg']/a") Then
            If driver.FindElement(By.XPath("//*[@id='dialog-msg']/a")).Displayed Then

                On Error Resume Next
                driver.FindElement(By.XPath("//*[@id='dialog-msg']/a")).Click()
                driver.FindElement(By.XPath("/html/body/div[7]/div[1]/a/span")).Click()
                On Error GoTo 0
                '********************************************************************


            End If



        ElseIf ChecarPresenca(driver, "/html/body/div[7]/div[1]/a/span") Then
            If driver.FindElementByXPath("/html/body/div[7]/div[1]/a/span").Displayed Then
                driver.FindElementByXPath("/html/body/div[7]/div[1]/a/span").Click()
            End If

        End If



    End Sub

    Private Sub PosicionaCNPJRaiz(cNPJ_RAIZEncontrado As String, cNPJ As String, fatura As Fatura)

posiciona_cnpj_raiz:
        If cNPJ_RAIZEncontrado <> Left(cNPJ, 8) Then
            Dim eles
            Dim eles2
            Dim ele

            'ABRE O MENU DE SELEÇÃO DE MATRIZES
            driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[1]/div/div[1]/button/span[1]")).Click()

            Try
150:            ele = driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[1]/div/div[1]/div"))
            Catch ex As Exception
                Throw New CNPJNaoCadastradoException(fatura, "CNPJ NÃO CADASTRADO PARA ESTE GESTOR", False)
            End Try

151:        eles = ele.FindElements(By.ClassName("second_item"))
            eles2 = ele.FindElements(By.TagName("li"))

            'FAZ LOOP SOBRE AS OPÇÕES
            Dim indice_spam
            Dim CNPJ_RAIZ_MENU

            indice_spam = 0
            For Each ele In eles
                indice_spam = indice_spam + 1
                CNPJ_RAIZ_MENU = Replace(Replace(ele.Text, "/", ""), ".", "")
                If CNPJ_RAIZ_MENU = Left(cNPJ, 8) Then Exit For
            Next ele

            'CONFIRMA SE O CNPJ FOI ENCONTRADO
            driver.FindElement(By.XPath("//*[@id='formSelectedItem']/ul/li[" & indice_spam & "]")).Click()
            cNPJ_RAIZEncontrado = driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]/div/span[2]")).Text
            cNPJ_RAIZEncontrado = Replace(Replace(cNPJ_RAIZEncontrado, "/", ""), ".", "")
            If cNPJ_RAIZEncontrado <> Left(cNPJ, 8) Then
                Throw New CNPJNaoCadastradoException(fatura, "CNPJ NÃO CADASTRADO PARA ESTE GESTOR", False)
            End If
        End If


    End Sub

    Private Sub PosicionCnpjFilial(cNPJ_RAIZEncontrado As String, cNPJ As String, fatura As Fatura)

        Dim ele As IWebElement

        'VERIFICA O CNPJ FILIAL
        Dim filial

        Dim CNPJ_FILIAL = driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]/div/span[3]")).Text
        CNPJ_FILIAL = Replace(CNPJ_FILIAL, "-", "")
        If CNPJ_FILIAL <> Right(cNPJ, 6) Then  'posiciona cnpj filial
            filial = Mid(Right(cNPJ, 6), 1, 4) + "-" + Right(cNPJ, 2)

            Try
200:            driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]")).Click()
            Catch ex As Exception
                Throw New CNPJNaoCadastradoException(fatura, "CNPJ NÃO CADASTRADO PARA ESTE GESTOR", False)
            End Try


            Dim ele_filial As IWebElement


            'CONFIGURA O LOOPING SOBRE AS FILIAIS
            Dim eles_filial
            Dim eles2_filial

            Try
250:            ele_filial = driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/div[1]/div[2]"))
            Catch ex As Exception
                Throw New CNPJNaoCadastradoException(fatura, "CNPJ NÃO CADASTRADO PARA ESTE GESTOR", False)
            End Try

251:
            eles_filial = ele_filial.FindElements(By.ClassName("second_item"))
            eles2_filial = ele_filial.FindElements(By.TagName("li"))

            ' FAZ LOOPING SOBRE AS FILIAIS
            Dim CNPJ_FILIAL_MENU

            Dim indice_spam = 0
            For Each ele_filial In eles_filial
                indice_spam = indice_spam + 1

                Debug.Print(ele_filial.GetAttribute("data-value"))
                CNPJ_FILIAL_MENU = Replace(Replace(ele_filial.GetAttribute("data-value"), "-", ""), ".", "")
                If CNPJ_FILIAL_MENU = Right(cNPJ, 6) Then
                    ele = ele_filial
                    ele.Click()
                    Exit For
                End If
                If indice_spam = eles_filial.Count Then
                    Throw New CNPJNaoCadastradoException(fatura, "CNPJ NÃO CADASTRADO PARA ESTE GESTOR", False)

                End If

            Next ele_filial


        End If

    End Sub

    Private Function ConfirmaAparecimentoDoCNPJ() As String
        Dim CNPJ_RAIZ

        If ChecarPresenca(driver, "//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]/div/span[2]") Then ' checa se o cnpj apareceu 'ops'
            CNPJ_RAIZ = driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]/div/span[2]")).Text
            CNPJ_RAIZ = Replace(Replace(CNPJ_RAIZ, "/", ""), ".", "")
        Else 'se naõ apareceu tenta reload

            driver.Navigate.Refresh()

            If ChecarPresenca(driver, "//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]/div/span[2]") Then ' checa se o cnpj apareceu 'ops'
                CNPJ_RAIZ = driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]/div/span[2]")).Text
                CNPJ_RAIZ = Replace(Replace(CNPJ_RAIZ, "/", ""), ".", "")
            Else
                Return ""
            End If
        End If

        Return CNPJ_RAIZ
    End Function

End Class

