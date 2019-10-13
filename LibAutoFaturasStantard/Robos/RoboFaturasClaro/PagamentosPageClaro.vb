
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Interactions

Public Class PagamentosPageClaro
    Private driver = WebdriverCt.Driver



    Private Sub AcessarPagamentos()
        Dim Action As New Actions(driver)
        Dim LabelPagamentos As IWebElement = driver.FindElement(By.XPath("/html/body/table/tbody/tr/td[1]/ul/table/tbody/tr/td[3]/li/a/img"))

        Action.MoveByOffset(200, 100).Perform()
        Action.MoveToElement(LabelPagamentos).Perform()
        driver.FindElementByXPath("/html/body/table/tbody/tr/td[1]/ul/table/tbody/tr/td[3]/li/ul/li[1]/a").Click() 'abrir pagina de pagamentos

    End Sub

    Private Function ConstruirDatatable() As DataTable
        Dim Pagamentos As New DataTable
        Pagamentos.Columns.Add("Data", Type.GetType("System.DateTime"))
        Pagamentos.Columns.Add("NrFatura", Type.GetType("System.String"))
        Pagamentos.Columns.Add("Pendente", Type.GetType("System.Boolean"))
        Pagamentos.Columns.Add("ValorOriginal", Type.GetType("System.Double"))
        Pagamentos.Columns.Add("Ajustes", Type.GetType("System.Double"))
        Pagamentos.Columns.Add("ValorPendente", Type.GetType("System.Double"))
        Return Pagamentos
    End Function


    Public Function ConsultarPagamentos(tabelaWEbElement As IWebElement) _
        As DataTable

        Dim PagamentosTable As DataTable = ConstruirDatatable()

        Dim ListaContas As New List(Of DataRow)

        Dim webrows As IReadOnlyList(Of IWebElement) = tabelaWEbElement.FindElements(By.TagName("tr"))

        For Each webrow In webrows
            Dim Pagamento As DataRow = PagamentosTable.NewRow

            For x = 1 To 6
                Pagamento(x) = webrow.FindElement(By.XPath("/td[" + (x + 1).ToString + "]")).Text
            Next

            PagamentosTable.Rows.Add(Pagamento)

        Next

        Return PagamentosTable
    End Function

End Class
