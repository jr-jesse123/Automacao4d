Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium

Friend Class PosicionadorProdutoVivo
    Private driver As ChromeDriver
    Private fatura As Fatura
    Private movel As ProdutosVivo

    Public Sub New(driver As ChromeDriver, fatura As Fatura)
        Me.driver = driver
        Me.fatura = fatura

    End Sub

    Friend Sub posicionarProduto(movel As ProdutosVivo, fatura As Fatura)

        If driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[2]/button/div/span[2]").Text = "Móvel" Then
            Exit Sub
        End If

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)
        Try
            driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[2]/button").Click()
        Catch ex As WebDriverException
            Stop
            Exit Sub
        End Try

        Dim produtosContainer = driver.FindElement(By.XPath("//*[@id='formSelectedItem']/ul"))

        Dim produto As IWebElement
        Try
            produto = driver.FindElementByLinkText("Móvel")
            produto.Click()
        Catch ex As WebDriverException

            driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[2]/button").Click()
            Throw New ProdutoNaoCadastradoException(conta, "Este tipo de produto não está cadastrado para este gestor", False)

        End Try



    End Sub
End Class


