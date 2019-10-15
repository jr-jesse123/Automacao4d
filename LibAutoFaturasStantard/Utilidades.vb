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

    Friend Shared Sub longaEsperaPorNavegacao(driver As ChromeDriver, url As String, MaxTentativas As Integer)

        Dim nrDeTentativas As Integer

        Try
espera:
            driver.Navigate.GoToUrl(url)

        Catch ex As WebDriverTimeoutException
            If nrDeTentativas > MaxTentativas Then Throw
            GoTo espera
        End Try

    End Sub

    ''' <summary>
    ''' Função criada para verificar se há a presença de um elemento na página
    ''' </summary>
    ''' <param name="drive">o webdriver que contém o navegador aberto
    ''' precisa escolher apenas um parametro</param>
    ''' <param name="xpath"></param>
    ''' <param name="ClassName"></param>
    ''' <param name="id"></param>
    ''' <param name="texto"></param>
    ''' <param name="PartialText"></param>
    ''' <returns></returns>

    Shared Function ChecarPresenca(ByRef drive As IWebDriver, Optional xpath As String = "", Optional ClassName As String = "",
                              Optional id As String = "", Optional texto As String = "",
                              Optional PartialText As String = "") As Boolean
        Dim output As Boolean
        Dim tempoatual As TimeSpan = drive.Manage.Timeouts.ImplicitWait
        drive.Manage.Timeouts.ImplicitWait = New TimeSpan(0, 0, 0)

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
            output = True
        Catch ex As WebDriverException
            output = False
        Finally
            drive.Manage.Timeouts.ImplicitWait = tempoatual
        End Try


        Return output
    End Function

    ''' <summary>
    ''' Metodo feito para esperar além dos 60 segundos padrões do webdriver
    ''' pelo desaparecimento de algum item
    ''' </summary>
    ''' <param name="driver"></param>
    ''' <param name="TentativasMax">cada tentativa dura um minuto, após este máximo de 
    ''' tentativas a exceção será lançada</param>
    ''' <param name="xpath">xpath que deve desaparecer</param>
    Friend Shared Sub longaEsperaPorInvisibilidade(driver As ChromeDriver, tentativasmax As Integer, xpath As String)

        Dim tentativas As Integer
        Dim wait As New WebDriverWait(driver, New TimeSpan(59, 0, 0))

inicio:
        Try
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(xpath)))

        Catch ex As WebDriverException
            tentativas += 1
            If tentativas > tentativasmax Then
                Throw
            Else
                GoTo inicio
            End If
        End Try



    End Sub

    ''' <summary>
    ''' Metodo feito para clicar e esperar além dos 60 segundos padrões do webdriver
    ''' </summary>
    ''' <param name="driver"></param>
    ''' <param name="TentativasMax">cada tentativa dura um minuto, após este máximo de 
    ''' tentativas a exceção será lançada</param>
    ''' <param name="xpath">xpath a ser clicado</param>
    Friend Shared Sub longaEsperaAposClicar(driver As ChromeDriver, TentativasMax As Integer, xpath As String)

        Dim tentativas As Integer


        Try
            driver.FindElementByXPath(xpath).Click()
        Catch ex As WebDriverException
novaEspera:
            Try
                driver.FindElementByXPath(xpath).Click()
            Catch ex2 As WebDriverException
                Exit Sub
            End Try

            tentativas += 1
            If tentativas > TentativasMax Then
                Throw
            Else
                Threading.Thread.Sleep(60000)
                GoTo novaEspera
            End If
        End Try


    End Sub

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
    ''' <summary>
    ''' função feita para aguardar e confirmar se algum arquivo foi baixado.
    ''' </summary>
    ''' <param name="TempoLimiteEmSegundos"> é o tempo máximo que a função vai esperar, se passar desse tempo lança exceção</param>
    ''' <param name="HoraInicial"> hora inicial, antes da qual os arquivos serão desconsiderados. Capturar o valor desta variável antes de acionar o download para garantir o arquivo baixado seja encontrado. </param>
    ''' <returns>Retorna Verdadeiro caso algum arquivo tenha sido encontrado dentro do período limite, e falso caso nenhum arquivo novo tenha sido encontrado</returns>
    Public Shared Function AguardaEConfirmaDwonload(TempoLimiteEmSegundos As Integer, HoraInicial As Date) As Boolean

inicio:
        MatarProcessosdeAdobeATivos()
        Dim Cronometro As New TimeSpan(0, 0, 0)
        Dim arquivos = IO.Directory.EnumerateFiles(WebdriverCt._folderContas).ToList
        Dim contador As Integer

        Do While contador < TempoLimiteEmSegundos
            arquivos = IO.Directory.EnumerateFiles(WebdriverCt._folderContas).ToList
            For Each arquivo In arquivos
                If File.GetLastWriteTime(arquivo) > HoraInicial Then

                    If Not arquivo.EndsWith("pdf") And Not arquivo.EndsWith("csv") Then
                        GoTo inicio
                    End If

                    Try
                        File.Move(arquivo, arquivo.Replace(Path.GetFileName(arquivo), "UltimoArquivo.pdf"))
                    Catch ex As ArgumentException
                        MatarProcessosdeAdobeATivos()
                    Catch ex As UnauthorizedAccessException
                        File.Delete(arquivo.Replace(Path.GetFileName(arquivo), "UltimoArquivo.pdf"))
                        File.Move(arquivo, arquivo.Replace(Path.GetFileName(arquivo), "UltimoArquivo.pdf"))
                    Catch ex As IO.IOException
                        File.Delete(arquivo.Replace(Path.GetFileName(arquivo), "UltimoArquivo.pdf"))
                        File.Move(arquivo, arquivo.Replace(Path.GetFileName(arquivo), "UltimoArquivo.pdf"))
                    End Try
                    Return True
                End If
            Next
            contador += 1
            Threading.Thread.Sleep(1000)
        Loop

        Return False

    End Function

    Shared Sub MatarProcessosdeAdobeATivos()

        Dim ProcessosAdobe() As Process = Process.GetProcessesByName("Acrobat")

        For Each processo As Process In ProcessosAdobe
            processo.Kill()
        Next

        Threading.Thread.Sleep(500)

    End Sub
    Shared Function ObterFrames(driver As IWebDriver) As List(Of String)


        Dim eles As IReadOnlyCollection(Of IWebElement) = driver.FindElements(By.TagName("Frame"))

        Return eles
    End Function

    Public Shared Function lfRetiraNumeros_linhacadastro(ByVal vValor As String) As String

        Dim lfRetiraNumeros As String
        Dim i As Integer


        'Conta a quantidade de caracteres
        Dim vQtdeCaract As Long
        Dim vControle As Boolean

        vQtdeCaract = vValor.Length
        vControle = False

        'Para cada caractere identifica se é número ou texto

        i = 1
#Disable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
        Do Until i = vQtdeCaract
#Enable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.

            'Se for número adiciona no retorno da função

            If Char.IsNumber(vValor.Substring(i, 1)) Then
#Disable Warning BC42104 ' Variável "lfRetiraNumeros" é usada antes de receber um valor. Uma exceção de referência nula poderia resultar em runtime.
                If vControle = True And lfRetiraNumeros <> vbNullString Then
#Enable Warning BC42104 ' Variável "lfRetiraNumeros" é usada antes de receber um valor. Uma exceção de referência nula poderia resultar em runtime.
                    lfRetiraNumeros = lfRetiraNumeros + " "
                End If
                vControle = False
                lfRetiraNumeros = lfRetiraNumeros & vValor.Substring(i, 1)
            Else
                vControle = True
            End If



#Disable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
            i += 1
#Enable Warning BC37234 ' Late binding não é suportado no tipo de projeto atual.
        Loop


        'Substitui espaços em branco por / e tira espaços em branco no final do retorno da função
        lfRetiraNumeros_linhacadastro = lfRetiraNumeros.Trim.Replace(" ", "")



    End Function


    Public Shared Sub CentralizarElementoComJs(driver As ChromeDriver, elemento As IWebElement)
        'centralizaa objeto por javascript
        

        Dim scrollElementIntoMiddle As String = "var viewPortHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);" _
                                            + "var elementTop = arguments[0].getBoundingClientRect().top;" _
                                            + "window.scrollBy(0, elementTop-(viewPortHeight/2));"

        CType(driver, IJavaScriptExecutor).ExecuteScript(scrollElementIntoMiddle, elemento)
        '***************centralizaa objeto por javascript
    End Sub
End Class




