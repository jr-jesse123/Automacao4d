Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Firefox
Imports OpenQA.Selenium.Support.UI
Imports SeleniumExtras.WaitHelpers
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions

Public Class Utilidades


    Public Shared Sub LongaEsperaPorXpath(driver As IWebDriver, Xpath As String, MaxTentativas As Integer)

        Dim nrDeTentativas As Integer

        Try
espera:
            Dim WAIT As New WebDriverWait(driver, New TimeSpan(0, 0, 1))

            WAIT.Until(ExpectedConditions.ElementIsVisible(By.XPath(Xpath)))

        Catch ex As WebDriverTimeoutException
            nrDeTentativas = nrDeTentativas + 1
            If nrDeTentativas > MaxTentativas Then Throw
            GoTo espera
        End Try


    End Sub

    Shared Function ChecarPresenca(ByRef drive As IWebDriver, Optional xpath As String = "", Optional ClassName As String = "",
                              Optional id As String = "", Optional texto As String = "",
                              Optional PartialText As String = "") As Boolean
        Dim by As By

        If xpath.Length > 0 Then by = By.XPath(xpath)
        If ClassName.Length > 0 Then by = By.ClassName(ClassName)
        If id.Length > 0 Then by = By.Id(id)
        If texto.Length > 0 Then by = By.LinkText(texto)
        If PartialText.Length > 0 Then by = By.PartialLinkText(PartialText)

        Try
#Disable Warning BC42104 ' A variável é usada antes de receber um valor
            drive.FindElement(by)
#Enable Warning BC42104 ' A variável é usada antes de receber um valor
            Return True
        Catch ex As NoSuchElementException
            Return False
        End Try

    End Function

    Friend Shared Sub AbrirnovoLink(driver As ChromeDriver, by As By)

        Dim NrDeJanelasAtuais = driver.WindowHandles.Count
        driver.FindElement(by).Click()
        WaitForNewWindow(driver, 60, NrDeJanelasAtuais)
        driver.SwitchTo.Window(driver.WindowHandles(NrDeJanelasAtuais))

    End Sub

    Public Shared Function Regex(ByVal padrao As String, texto As String) As Match()
        'cria o padrão regex
        Dim padraoRegex As String = padrao
        Dim verifica As New Regex(padraoRegex)

        'Dim nrResultados = verifica.Matches(texto).Count

        Dim resultados(verifica.Matches(texto).Count - 1) As Match


        verifica.Matches(texto).CopyTo(resultados, 0)

        Return resultados
    End Function

    Public Shared Sub LongaEsperaPorLinkTextComOpcoes(driver As FirefoxDriver, MaxTentativas As Integer, ParamArray Textos As String())

        Throw New NotImplementedException("função longaesperaporlinkcomopções não implementada")

    End Sub



    Shared Sub WaitForNewWindow(driver As IWebDriver, timeout As Integer, NrDeJanelasAtuais As Integer)

        Dim flag As Boolean = False
        Dim counter As Integer = 0

        timeout = timeout * 10

        Do While Not flag
            Try
                Dim winId = driver.WindowHandles
                If winId.Count > NrDeJanelasAtuais Then
                    flag = True
                Else
                    Threading.Thread.Sleep(1000)
                    counter = counter + 1
                End If
                If counter > timeout Then
                    Throw New WebDriverTimeoutException("Nova Janela não apareceu após " + counter.ToString + " tentativas")
                End If

            Catch e As Exception
                Stop
                Throw
            End Try
        Loop

    End Sub


    Public Shared Function generateXPATH(ByVal childElement As IWebElement, ByVal current As String) As String
        Dim childTag As String = childElement.GetAttribute("tag")

        If childTag.Equals("html") Then
            Return "/html[1]" & current
        End If

        Dim parentElement As IWebElement = childElement.FindElement(By.XPath(".."))
        Dim childrenElements As IReadOnlyCollection(Of IWebElement) = parentElement.FindElements(By.XPath("*"))
        Dim count As Integer = 0

        For i As Integer = 0 To childrenElements.Count - 1
            Dim childrenElement As IWebElement = childrenElements(i)
            Dim childrenElementTag As String = childrenElement.GetAttribute("tag")

            If childTag.Equals(childrenElementTag) Then
                count += 1
            End If

            If childElement.Equals(childrenElement) Then
                'Return parentElement + "/" + childTag + "[" + count + "]" + current
            End If
        Next

        Return Nothing
    End Function


    Public Shared Sub LongaEsperaPorLinkText(driver As IWebDriver, LinkText As String, MaxTentativas As Integer)

        Dim nrDeTentativas As Integer

        Try
espera:
            Dim WAIT As New WebDriverWait(driver, New TimeSpan(0, 0, 59))

            WAIT.Until(ExpectedConditions.InvisibilityOfElementLocated(By.LinkText(LinkText)))

        Catch ex As WebDriverTimeoutException
            nrDeTentativas = nrDeTentativas + 1
            If nrDeTentativas > MaxTentativas Then Throw
            GoTo espera
        End Try
    End Sub

    Public Shared Function AguardaEConfirmaDwonload(TempoLimiteEmSegundos As Integer, HoraInicial As Date) As Boolean
        Dim Cronometro As New TimeSpan(0, 0, 0)
        Dim arquivos = IO.Directory.EnumerateFiles(WebdriverCt._folderContas).ToList
        Dim contador As Integer

        Do While contador < TempoLimiteEmSegundos
            arquivos = IO.Directory.EnumerateFiles(WebdriverCt._folderContas).ToList
            For Each arquivo In arquivos
                If File.GetCreationTime(arquivo) > HoraInicial Then Return True
            Next
            contador += 1
            Threading.Thread.Sleep(1000)
        Loop

        Return False

    End Function

    Shared Function ObterFrames(driver As IWebDriver) As List(Of String)


        Dim eles As IReadOnlyCollection(Of IWebElement) = driver.FindElements(By.TagName("Frame"))

        Return eles
    End Function



End Class



