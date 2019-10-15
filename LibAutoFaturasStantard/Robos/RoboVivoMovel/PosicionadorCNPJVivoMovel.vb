Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium
Imports LibAutoFaturasStantard.Utilidades

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

                Try
                    driver.FindElement(By.XPath("//*[@id='dialog-msg']/a")).Click()
                    driver.FindElement(By.XPath("/html/body/div[7]/div[1]/a/span")).Click()

                    '********************************************************************
                Catch ex As Exception

                End Try



            End If



        ElseIf ChecarPresenca(driver, "/html/body/div[7]/div[1]/a/span") Then
            If driver.FindElementByXPath("/html/body/div[7]/div[1]/a/span").Displayed Then
                driver.FindElementByXPath("/html/body/div[7]/div[1]/a/span").Click()
            End If

        End If



    End Sub

    Private Sub PosicionaCNPJRaiz(cNPJ_RAIZEncontrado As String, cNPJ As String, fatura As Fatura)

posiciona_cnpj_raiz:
        If cNPJ_RAIZEncontrado <> cNPJ.Substring(0, 8) Then
            Dim eles
            Dim eles2 As Object
            Dim ele As IWebElement

            'ABRE O MENU DE SELEÇÃO DE MATRIZES
            driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[1]/div/div[1]/button/span[1]")).Click()

            Try
150:            ele = driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[1]/div/div[1]/div"))
            Catch ex As Exception
                Throw New CNPJNaoCadastradoException(fatura, "CNPJ NÃO CADASTRADO PARA ESTE GESTOR")
            End Try

#Disable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
151:        eles = ele.FindElements(By.ClassName("second_item"))
#Enable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
#Disable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
            eles2 = ele.FindElements(By.TagName("li"))
#Enable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.

            'FAZ LOOP SOBRE AS OPÇÕES
            Dim indice_spam As Integer
            Dim CNPJ_RAIZ_MENU As String

            indice_spam = 0
            For Each ele In eles
#Disable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
                indice_spam = indice_spam + 1
#Enable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.

#Disable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
                Dim preCNPJ As String = ele.Text
#Enable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
                CNPJ_RAIZ_MENU = preCNPJ.Replace(".", "").Replace("/", "")


#Disable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
                If CNPJ_RAIZ_MENU = cNPJ.Substring(0, 8) Then Exit For
#Enable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
            Next ele

            'CONFIRMA SE O CNPJ FOI ENCONTRADO
#Disable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
            driver.FindElement(By.XPath("//*[@id='formSelectedItem']/ul/li[" & indice_spam & "]")).Click()
#Enable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
            cNPJ_RAIZEncontrado = driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]/div/span[2]")).Text
            cNPJ_RAIZEncontrado = cNPJ_RAIZEncontrado.Replace("/", "").Replace(".", "")
            If cNPJ_RAIZEncontrado <> cNPJ.Substring(0, 8) Then
                Throw New CNPJNaoCadastradoException(fatura, "CNPJ NÃO CADASTRADO PARA ESTE GESTOR")
            End If
        End If


    End Sub

    Private Sub PosicionCnpjFilial(cNPJ_RAIZEncontrado As String, cNPJ As String, fatura As Fatura)

        Dim ele As IWebElement

        'VERIFICA O CNPJ FILIAL
        Dim filial

        Dim CNPJ_FILIAL = driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]/div/span[3]")).Text
        CNPJ_FILIAL = CNPJ_FILIAL.Replace("-", "")


        If CNPJ_FILIAL <> cNPJ.Substring(cNPJ.Length - 6) Then  'posiciona cnpj filial
            filial = cNPJ.Substring(cNPJ.Length - 6) + "-" + cNPJ.Substring(cNPJ.Length - 3)

            Try
200:            driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]")).Click()
            Catch ex As Exception
                Throw New CNPJNaoCadastradoException(fatura, "CNPJ NÃO CADASTRADO PARA ESTE GESTOR")
            End Try


            Dim ele_filial As IWebElement


            'CONFIGURA O LOOPING SOBRE AS FILIAIS
            Dim eles_filial As IReadOnlyCollection(Of IWebElement)
            Dim eles2_filial As IReadOnlyCollection(Of IWebElement)

            Try
250:            ele_filial = driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/div[1]/div[2]"))
            Catch ex As Exception
                Throw New CNPJNaoCadastradoException(fatura, "CNPJ NÃO CADASTRADO PARA ESTE GESTOR")
            End Try

251:
            eles_filial = ele_filial.FindElements(By.ClassName("second_item"))
            eles2_filial = ele_filial.FindElements(By.TagName("li"))

            ' FAZ LOOPING SOBRE AS FILIAIS
            Dim CNPJ_FILIAL_MENU As String

            Dim indice_spam As Integer = 0
            For Each ele_filial In eles_filial
                indice_spam = indice_spam + 1

                Debug.Print(ele_filial.GetAttribute("data-value"))
                CNPJ_FILIAL_MENU = ele_filial.GetAttribute("data-value").Replace("-", "").Replace(".", "")
#Disable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
                If CNPJ_FILIAL_MENU = cNPJ.Substring(cNPJ.Length - 7) Then
#Enable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
                    ele = ele_filial
                    ele.Click()
                    Exit For
                End If
#Disable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
#Disable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
                If indice_spam = eles_filial.Count Then
#Enable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
#Enable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
                    Throw New CNPJNaoCadastradoException(fatura, "CNPJ NÃO CADASTRADO PARA ESTE GESTOR")

                End If

            Next ele_filial


        End If

    End Sub

    Private Function ConfirmaAparecimentoDoCNPJ() As String
        Dim CNPJ_RAIZ As String

        Dim wait As New Support.UI.WebDriverWait(driver, New TimeSpan(0, 0, 59))



        If ChecarPresenca(driver, "//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]/div/span[2]") Then ' checa se o cnpj apareceu 'ops'

            CNPJ_RAIZ = driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]/div/span[2]")).Text
            CNPJ_RAIZ = CNPJ_RAIZ.Replace("/", "").Replace(".", "")
        Else 'se naõ apareceu tenta reload

            driver.Navigate.Refresh()


            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]/div/span[2]")))

            If ChecarPresenca(driver, "//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]/div/span[2]") Then ' checa se o cnpj apareceu 'ops'
                CNPJ_RAIZ = driver.FindElement(By.XPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[1]/button[1]/div/span[2]")).Text
                CNPJ_RAIZ = CNPJ_RAIZ.Replace("/", "").Replace(".", "")
            Else
                Return ""
            End If
        End If

        Return CNPJ_RAIZ
    End Function

End Class

