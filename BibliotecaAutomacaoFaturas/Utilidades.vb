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
        Catch ex As NoSuchElementException
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

inicio:
        Try
            driver.FindElementByXPath(xpath).Click()
        Catch ex As WebDriverException
            tentativas += 1
            If tentativas > TentativasMax Then
                Throw
            Else
                GoTo inicio
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

    Public Shared Function lfRetiraNumeros_linhacadastro(ByVal vValor As String) As String

        Dim lfRetiraNumeros
        Dim i


        'Conta a quantidade de caracteres
        Dim vQtdeCaract As Long
        Dim vControle As Boolean

        vQtdeCaract = Len(vValor)
        vControle = False

        'Para cada caractere identifica se é número ou texto
        For i = 1 To vQtdeCaract
            'Se for número adiciona no retorno da função
            If IsNumeric(Mid(vValor, i, 1)) Then
                If vControle = True And lfRetiraNumeros <> vbNullString Then
                    lfRetiraNumeros = lfRetiraNumeros + " "
                End If
                vControle = False
                lfRetiraNumeros = lfRetiraNumeros & Mid(vValor, i, 1)
            Else
                vControle = True
            End If

        Next i

        'Substitui espaços em branco por / e tira espaços em branco no final do retorno da função
        lfRetiraNumeros_linhacadastro = Replace(Trim(lfRetiraNumeros), " ", "")



    End Function


    Public Shared Sub EnviaEmail(ByVal texto As String, ByRef destinatarios As String(), conta As Conta)
        Dim iMsg, iConf, Flds
        Dim schema


        'Seta as variáveis, lembrando que o objeto Microsoft CDO deverá estar habilitado em Ferramentas->Referências->Microsoft CDO for Windows 2000 Library
        iMsg = CreateObject("CDO.Message")
        iConf = CreateObject("CDO.Configuration")
        Flds = iConf.Fields

        'Configura o componente de envio de email
        schema = "http://schemas.microsoft.com/cdo/configuration/"
        Flds.Item(schema & "sendusing") = 2
        'Configura o smtp
        Flds.Item(schema & "smtpserver") = "smtp.gmail.com"
        'Configura a porta de envio de email
        Flds.Item(schema & "smtpserverport") = 465
        Flds.Item(schema & "smtpauthenticate") = 1
        'Configura o email do remetente
        Flds.Item(schema & "sendusername") = "junior.jesse@gmail.com"
        'Configura a senha do email remetente
        Flds.Item(schema & "sendpassword") = "SenhaSegura12"
        Flds.Item(schema & "smtpusessl") = 1
        Flds.Update



        With iMsg
            'Email do destinatário
            .To = destinatarios(0) + ";" + destinatarios(1)
            'Seu email
            .From = "junior.jesse@gmail.com"
            'Título do email
            .Subject = "AVISO DE CORTE CONTA: " & conta.NrDaConta & " & CNPJ : " & conta.Empresa.CNPJ
            'Mensagem do e-mail, você pode enviar formatado em HTML
            .HTMLBody = texto
            'Seu nome ou apelido
            .Sender = "junior.jesse@gmail.com"
            'Nome da sua organização
            .Organization = "4D"
            'email de responder para
            '.ReplyTo = "teste@gmail.com"
            'Anexo a ser enviado na mensagem
            '.AddAttachment ("c:\fatura.txt")
            'Passa a configuração para o objeto CDO
            .Configuration = iConf
            'Envia o email
            .Send
        End With

        'Limpa as variáveis
        iMsg = Nothing
        iConf = Nothing
        Flds = Nothing

    End Sub


End Class




