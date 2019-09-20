'Public Class portalvivofixo
'    Private bancoDados()
'    Public cnpj, CPFATUAL, SENHA, CPF, LINHA, conta, OPERADORA, COD, salvaCNPJ, vencimento, status, produto, tratarPdf, destino, idTriagem, ultimoDownload As String
'    Public bot As Selenium.WebDriver
'    Public numerador As Integer
'    Private sql As Variant
'    Dim PDF As New PDF
'    Public ref As String
'    Public valor As Double
'    Public creditos As Double
'    Public encargos As Double
'    Dim login As Boolean
'    Dim j
'    'variáveis:
'    Public modeloVF1, modeloVF2, btnLinha, modeloSV, campoSENHA, btnProduto, btnMCDetalhada, btnSair, headerLogoff, telaEstado, presencaFaturas, telaDeslogada, btn2viaContas, campoCNPJ, msgErro, btnContinuar, btnEntrar, acessarFixo, perfilPF, listaProdutos, escolhaVF, btnContas, btnGCDetalhada, btnMinhaConta, btnMinhasFaturas, ausenciaFatura, selecioneRD, solDeDados, campoVencRD, numContaRD, btnConfirmarRD, Z As XPath
'    Public paginaLogin, nLinha As String
'    Public DirFaturasRobo As String

'    Sub initialize()
'        DirFaturasRobo = "C:\Users\Administrador.000\Downloads\faturasRobo\"


'    End Sub



'    Sub TestaConexao()
'        On Error GoTo handler

'        Dim conexaoMysql As conexaoMysql
'    Set conexaoMysql = New conexaoMysql

'    bancoDados = conexaoMysql.buscarbanco_fixoVivo()
'        Exit Sub

'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

'    End Sub

'    Sub setarDados(Optional ByVal numerador As Integer = 0)

'        Err.Clear()
'        On erorr GoTo handler

'        cnpj = bancoDados(2, numerador)
'        OPERADORA = bancoDados(1, numerador)
'        SENHA = bancoDados(6, numerador)
'        CPF = bancoDados(4, numerador)
'        identificador = bancoDados(8, numerador)
'        conta = bancoDados(7, numerador)
'        conta_ultimoDownload = bancoDados(12, numerador)
'        idTriagem = bancoDados(19, numerador)
'        destino = "C:\Users\User\Downloads\faturasRobo\caminho teste\" 'bancoDados(14, numerador)
'        ultimoDownload = bancoDados(13, numerador)
'        Stop
'        ThisWorkbook.Sheets(1).Cells(1, 2).Value = numerador
'        ThisWorkbook.Save


'        Debug.Print conta
'Exit Sub

'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

'    End Sub

'    Sub abrirBot()
'        On Error GoTo handler

'Set bot = New WebDriver

'bot.SetPreference "download.default_directory", DirFaturasRobo
'bot.SetPreference "download.directory_upgrade", True
'bot.SetPreference "download.prompt_for_download", False
'                          'Ativa a preferência de fazer o download automático dos arquivos .pdf em vez de abrir em uma nova aba
'        bot.SetPreference "plugins.always_open_pdf_externally", True

'bot.Start "chrome"
'bot.Window.Maximize

'        'Call paginaInicial

'        Exit Sub

'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

'    End Sub

'    Sub variaveis()
'        On Error GoTo handler

'        paginaLogin = "https://meuvivoempresas.vivo.com.br/meuvivoempresas/appmanager/portal/fixo?_nfpb=true&_nfls=false&_pageLabel=empNegMVivo2FixoPymesBook&pFlutua=true"
'        modeloVF1 = "//*[@id='integracaoMarcaOauth_empNegMVEFVivo2HomePage']"
'        modeloVF2 = "//*[@id='boxComFatura']"
'        modeloSV = "//*[@id='meuvivo-list-item']"
'        btnLinha = "//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[3]"
'        btnProduto = "//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[2]/button/div/span[2]"
'        telaEstado = "/html/body/div[5]/div/div[2]/div[1]/div[3]/a[1]"
'        telaDeslogada = "//*[@id='mainView']/section/div[2]/div/article/h1"
'        campoCNPJ = "//*[@id='cpf_cnpj']"
'        acessarFixo = "//*[@id='acessar_como_fixo']/a"
'        perfilPF = "//*[@id='div_cpf_gestor']"
'        escolhaVF = "//*[@id='loginPJ_vivo_fixo']/div[2]"
'        campoSENHA = "//*[@id='senha_fixo']"
'        msgErro = "/html/body/div[1]/div/div/div/div/div/div[3]"
'        btnContas = "//*[@id='menuV2']/li[2]"
'        btnGCDetalhada = "//*[@id='menuV2']/li[2]/ul/li[6]"
'        btnEntrar = "//*[@id='loginFixo']"
'        btnContinuar = "loginPJ_cpfcnpj_button"
'        btnMinhaConta = "//*[@id='tabs']/div[1]/ul/li[2]"
'        btnMinhasFaturas = "//*[@id='tabs-2']/ul/li[1]"
'        listaProdutos = "//*[@id='formSelectedItem']/ul/li[2]"
'        ausenciaFatura = "/html/body/div[1]/div/div/div/div[2]/div/div"
'        btn2viaContas = "//*[@id='menuV2']/li[2]/ul/li[1]"
'        btnMCDetalhada = "//*[@id='tabs-2']/ul/li[3]"
'        selecioneRD = "//*[@id='formValidaCadastro']/div/div[1]/div/span/div[2]"
'        solDeDados = "//*[@id='sel_option']/li[2]"
'        campoVencRD = "//*[@id='inputDiaVencimentoDados']"
'        numContaRD = "//*[@id='inputNumeroConta']"
'        btnConfirmarRD = "//*[@id='defaultMVE_btn_confirmar']"
'        presencaFaturas = "//*[@id='content']/div/div/div/div[1]/div/div/div/div[1]"
'        btnSair = "//*[@id='headerSubmenu_1_2']/div/div[1]/div[1]/div/div[3]/div"
'        headerLogoff = "//*[@id='headerSubmenu_1_2']/div/div[1]/div[1]/div/div[3]"

'        'Call abrirBot

'        Exit Sub

'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

'    End Sub

'    Sub paginaInicial()
'        On Error GoTo handler

'        bot.Get paginaLogin
'bot.Wait(10000)

'        'verifica se veio tela de escolha de estado e se veio seleciona-o.
'        If checarElementos(telaEstado) Then
'            'Seleciona o estado do Acre
'            bot.FindElementByXPath("/html/body/div[5]/div/div[2]/div[1]/div[3]/a[1]").Click
'            bot.FindElementByXPath("//*[@id='box_scroll']/div[2]/ul/li[1]").Click
'            bot.FindElementByXPath("/html/body/div[5]/div/div[2]/div[1]/div[6]/a[1]").Click
'        End If

'        'Call enviarDados

'        Exit Sub

'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

'    End Sub

'    Sub enviarDados()
'        On Error GoTo handler

'        Dim x As Variant

'        For x = ThisWorkbook.Sheets(1).Cells(1, 2).Value To (UBound(bancoDados, 2) - 1)
'            On Error Resume Next
'            numerador = x

'            Call setarDados(numerador)


'            'Verifica se o cnpj atual é o mesmo do anterior, para saber se será necessário logoff ou não:

'            If salvaCNPJ = "" Then  'Significa que se trata da primeira conta e que não houve conta anterior
'                Call procedimentosLogin()

'                If Not login = False Then   'Verifica se o login foi realmente realizado na sub de procedimentosLogin
'                    Call tratarFatura()       'Chama a sub que irá verificar o tipo de conta e direcionar para seu modelo específico de downloads
'                End If

'            ElseIf salvaCNPJ <> cnpj Then       'Nesse caso será necessário realizar o logoff e ir pra próxima conta

'                If Not checarElementos(telaDeslogada) Then 'Confere se realmente não já foi realizado o logoff e, se não foi, executa o logoff
'                    If Not login = False Then
'                        Call logoff()
'                        Call procedimentosLogin()

'                        If login = True Then
'                            Call tratarFatura()
'                        End If
'                    Else
'                        Call procedimentosLogin()

'                        If login = True Then
'                            Call tratarFatura()
'                        End If
'                    End If
'                Else
'                    Call procedimentosLogin()

'                    If Not login = False Then
'                        Call tratarFatura()
'                    End If
'                End If

'            ElseIf salvaCNPJ = cnpj Then        'Nesse caso, como se trata de conta de mesmo cnpj, não será necessário logoff
'                bot.SwitchToFrame 0
'    If checarElementos("//*[@id='tabs']/div[1]/ul/li[1]") Then 'Verifica se a página é do modelo de Solução de Voz...
'                    bot.FindElementByXPath("//*[@id='tabs']/div[1]/ul/li[1]").Click
'                    bot.SwitchToParentFrame
'                    Call tratarFatura()
'                ElseIf checarElementos("//*[@id='menuV2']/li[1]") Then   '...ou se a página é do modelo de Voz Fixa
'                    bot.FindElementByXPath("//*[@id='menuV2']/li[1]").Click
'                    bot.SwitchToParentFrame
'                    Call tratarFatura()

'                ElseIf Not checarElementos("//*[@id='linhaA0']") Then
'                    bot.SwitchToParentFrame
'                    If checarElementos("//*[@id='linhaA0']") Then
'                        salvaCNPJ = cnpj
'                        Call logoff()
'                        Call procedimentosLogin()
'                    End If
'                    If login = True Then
'                        Call tratarFatura()
'                    End If
'                Else
'                    salvaCNPJ = cnpj
'                    Call tratarFatura()
'                    bot.SwitchToParentFrame
'                    Call enviarLog("Conta sem fatura", True)
'                End If
'            End If

'        Next x

'        On Error GoTo 0
'        Exit Sub

'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        Resume
'    End Sub

'    Sub procedimentosLogin()
'        On Error GoTo handler
'        'Retorna para a página de envio de dados após o logoff
'        bot.Get paginaLogin
'bot.Wait(2000)

'        'Envio do cnpj e confirmação
'        bot.FindElementByXPath(campoCNPJ).SendKeys cnpj
'bot.FindElementById(btnContinuar).Click
'        bot.Wait(2000)

'        'Checa se há erro no formato do cnpj
'        If checarElementos("//*[@id='msg_cpf_cnpj']") Or checarElementos("//*[@id='msg_cpf_cnpj_incompleto']") Then
'            Call enviarLog("Erro no formato do CNPJ", False)
'            login = False
'            Exit Sub
'        End If
'        bot.Wait(4000)
'        'Checa se a opção de entrar pelo modo Vivo Fixo aparece e, se sim, clica nela
'        If checarElementos(acessarFixo) Then
'            bot.FindElementByXPath(acessarFixo).WaitDisplayed(True, 100).Click
'        End If
'        bot.Wait(2000)

'        'Checa se não se trata de uma conta Vivo Fixo de uma Pessoa Física
'        If checarElementos(perfilPF) Then
'            login = True
'            Call procedimentoPF()
'            Exit Sub
'        End If
'        bot.Wait(2000)
'        'Checa se aparece a opção de escolha de Vivo Móvel e Vivo Fixo e clica em Vivo Fixo
'        If checarElementos(escolhaVF) Then
'            bot.FindElementByXPath(escolhaVF).Click
'        ElseIf checarElementos(perfilPF) Then
'            login = True
'            Call procedimentoPF()
'            Exit Sub
'        End If
'        bot.Wait(2000)
'        'Envio da senha e confirmação
'        bot.FindElementByXPath(campoSENHA).WaitDisplayed(True, 0).SendKeys SENHA
'bot.FindElementByXPath(btnEntrar).Click
'        bot.Wait(4000)

'        'Checa se a senha está bloqueada
'        If checarElementos("//*[@id='errorFixo']/div[2]") Then
'            Call enviarLog("Login ou senha inválidos", False)
'            login = False
'            Exit Sub
'        Else
'            login = True
'        End If

'        bot.Wait(3000)
'        'Atualiza a página com o objetivo de fechar qualquer aviso que aparecer
'        bot.Refresh

'        bot.Wait(4000)

'        Exit Sub

'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

'    End Sub

'    Function checarElementos(ByVal XPath As String, Optional metodo As Integer = 1)

'        On Error GoTo handler
'        On Error GoTo objeto_nao_encontrado

'        Dim ele As WebElement
'Set ele = bot.FindElementByXPath(XPath, 1)
'Select Case metodo

'            Case Is = 1
'                If ele.IsDisplayed Then
'                    checarElementos = True
'                Else
'                    checarElementos = False
'                End If
'                Exit Function

'            Case Is = 2

'                If ele Is Nothing Then
'                    checarElementos = False
'                Else
'                    checarElementos = True
'                End If
'                Exit Function

'            Case Is = 3
'                If ele.IsEnabled Then
'                    checarElementos = True
'                Else
'                    checarElementos = False
'                End If
'                Exit Function
'        End Select

'objeto_nao_encontrado:
'        checarElementos = False

'        Exit Function

'        '*******************
'handler:
'        MsgBox "erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source
'Stop
'        Resume

'    End Function

'    Sub tratarFatura()
'        On Error GoTo handler

'        salvaCNPJ = cnpj 'salva o cnpj atual para ser comparado com o cnpj da conta posterior a essa
'        bot.Wait(2000)

'        'Captura o tipo de conta que consta na página inicial
'        bot.SwitchToParentFrame
'        If checarElementos(btnProduto) Then
'            produto = bot.FindElementByXPath(btnProduto).Text
'            'produto = Mid(produto, 8)
'        End If

'        If OPERADORA = "VOZ FIXA" Then
'            Call tipo_vozFixa()
'            Exit Sub

'        ElseIf OPERADORA = "SOLUÇÃO DE VOZ" Then
'            Call tipo_solucaoVoz()
'            Exit Sub

'        ElseIf OPERADORA = "INTERNET" Then
'            Call tipo_internet()
'            Exit Sub

'        ElseIf OPERADORA = "SOLUÇÃO DE REDES DE DADOS" Then
'            Call tipo_solucaoRD()
'            Exit Sub

'        ElseIf OPERADORA = "INTERNETCORPORATIVA" Then
'            Call tipo_internetCorporativa()
'            Exit Sub
'        End If

'handler:
'        Call enviarLog("Produto não encontrado", True)

'    End Sub

'    Sub tipo_vozFixa()
'        On Error GoTo handler

'        bot.Refresh
'        bot.Wait(12000)
'        'bot.SwitchToParentFrame
'        'Verifica se a página já está no modelo de Voz Fixa
'        If checarElementos(modeloVF1) Then
'            nLinha = bot.FindElementByXPath(btnLinha).Text
'            nLinha = Right(nLinha, 14)
'            If nLinha = LINHA Then
'                Call download_PDF()
'            Else
'                On Error GoTo handlerLinha
'                bot.FindElementByXPath(btnLinha).Click
'                bot.FindElementByLinkText(LINHA).Click
'                Call download_PDF()
'            End If
'        Else
'            bot.SwitchToParentFrame
'            On Error GoTo handlerProduto
'            bot.FindElementByXPath(btnProduto).Click
'            bot.FindElementByLinkText(OPERADORA).Click
'            nLinha = bot.FindElementByXPath(btnLinha).Text
'            nLinha = Right(nLinha, 14)
'            If LINHA = nLinha Then
'                Call download_PDF()
'            Else
'                On Error GoTo handlerLinha
'                bot.FindElementByXPath(btnLinha).Click
'                bot.FindElementByLinkText(LINHA).Click
'                Call download_PDF()
'            End If
'        End If

'        Exit Sub

'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        Exit Sub
'unico:
'        Call download_PDF()
'        Exit Sub
'handlerProduto:
'        Call enviarLog("Conta não encontrada", True)
'        Exit Sub
'handlerLinha:
'        Call enviarLog("Linha não encontrada", True)

'    End Sub

'    Sub tipo_solucaoVoz()
'        On Error GoTo handler

'        If UCase(produto) Like UCase("*Solução de Voz*") Then
'            If Not checarElementos(ausenciaFatura) Then
'                Call download_PDF2()
'            Else
'                Call enviarLog("Conta sem fatura", True)
'            End If
'        Else


'            bot.FindElementByXPath(btnProduto).Click
'            bot.FindElementByLinkText("Solução de Voz").Click
'            'bot.FindElementByLinkText(OPERADORA).Click
'            Call download_PDF2()
'        End If

'        Exit Sub
'handler:
'        Call enviarLog("Produto não encontrado", True)

'    End Sub

'    Sub tipo_internet()
'        On Error GoTo handler

'        bot.SwitchToFrame 0
'    If checarElementos(msgErro) Then                     'Verifica se aparece mensagem de erro
'            bot.Refresh
'        End If

'        bot.SwitchToParentFrame
'        If UCase(produto) Like UCase("*Internet*") Then     'Verifica se a página já está no modelo de Voz Fixa
'            If Not checarElementos(ausenciaFatura) Then
'                Call download_PDF()
'            Else
'                Call enviarLog("Conta sem fatura", True)
'            End If
'            Exit Sub
'        ElseIf checarElementos(btnProduto) Then
'            bot.FindElementByXPath(btnProduto).Click
'            bot.FindElementByLinkText(OPERADORA).Click
'            Call download_PDF()
'        End If

'        Exit Sub

'handler:
'        Call enviarLog("Produto não localizado", True)

'    End Sub

'    Sub tipo_solucaoRD()
'        On Error GoTo handler

'        If UCase(produto) Like UCase("Solução Rede de Dados") Then
'            If checarlementos(btnLinha) Then
'                bot.FindElementByXPath(btnLinha).Click      'No caso de Solução de Rede de Dados, o endereço é o mesmo de Linha, porém se refere ao número de conta
'                bot.FindElementByLinkText(conta).Click
'                If Not checarElementos(ausenciaFatura) Then
'                    If checarElementos(selecioneRD) Then
'                        bot.FindElementByXPath(selecioneRD).Click
'                        bot.FindElementByXPath(solDeDados).Click
'                        bot.FindElementByXPath(campoVencRD).SendKeys vencimento 'No caso de Solução de Rede de Dados será necessário enviar o vencimento
'                        bot.FindElementByXPath(numContaRD).SendKeys conta
'                    bot.FindElementByXPath(btnConfirmarRD).Click
'                        'Não sei como procede após isso pois não tive um exemplo de conta do tipo Solução de Rede de Dados
'                    End If
'                End If
'            End If
'        Else
'            bot.FindElementByXPath(btnProduto).Click
'            bot.FindElementByLinkText(OPERADORA).Click
'            Call download_PDF2()
'        End If

'handler:
'        Call enviarLog("Produto não localizado", True)

'    End Sub

'    Sub tipo_internetCorporativa()
'        On Error GoTo handler

'        If UCase(produto) Like UCase("Internet Corporativa") Then
'            If Not checarElementos(ausenciaFatura) Then
'                bot.FindElementByXPath("//*[@id='formSelectedItem']/ul/li[5]").Click
'                Call download_PDF2()
'            Else
'                Call enviarLog("Conta sem fatura", True)
'            End If
'        Else
'            bot.FindElementByXPath(btnProduto).Click
'            bot.FindElementByLinkText(OPERADORA).Click
'            Call download_PDF2()
'        End If
'        End If

'handler:
'        Call enviarLog("Produto não localizado", True)
'    End Sub

'    Sub download_PDF2()
'        On Error GoTo handler
'        'Clica nas abas até chegar na opção de download das faturas em PDF
'        bot.SwitchToFrame 0

'bot.FindElementByXPath(btnMinhaConta).Click
'        bot.FindElementByXPath(btnMinhasFaturas).Click
'        bot.Wait(4000)


'        'Busca em qual div está a fatura da conta selecionada e faz o download
'        If checarElementos(presencaFaturas) Then
'            For i = 1 To 15
'                j = i
'                On Error Resume Next
'                COD = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/p[" & i & "]").Text 'situação da conta
'                COD = Right(COD, 12)
'                If conta = COD Then
'                    ref = bot.FindElementByXPath("//*[@id='content']/div/div/div/div/div[1]/div/div/div[" & i & "]/table/tbody/tr[1]/td[1]").Text
'                    If ref = ultimoDownload Then Exit Sub ' deixar ultimodownload publico

'                    vencimento = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & i & "]/table/tbody/tr[1]/td[2]").Text
'                    status = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & i & "]/table/tbody/tr[1]/td[4]").Text
'                    'clica no botão de download
'                    bot.FindElementByXPath("//*[@id='content']/div/div/div/div/div[1]/div/div/div[" & i & "]/table/tbody/tr[1]/td[5]").Click
'                    bot.SwitchToParentFrame
'                    Call tratamento_PDF()
'                    Exit Sub
'                    On Error GoTo 0
'                End If
'            Next i

'        Else
'            'Significa que na página consta apenas uma conta (o layout é diferente)
'            ref = bot.FindElementByXPath("/html/body/section/div/div[2]/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[1]").Text
'            'bot.FindElementByXPath("//*[@id='btnDown']").Click
'            bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[5]").Click
'            status = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[4]").Text
'            bot.SwitchToParentFrame
'            Call tratamento_PDF()
'        End If


'        Exit Sub

'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

'    End Sub

'    Sub download_PDF()
'        On Error GoTo handler

'        bot.SwitchToParentFrame
'        bot.SwitchToFrame 0
'Dim getStatus As String
'        getStatus = bot.FindElementByXPath("//*[@id='boxComFatura']/div/h4").Text
'        If UCase(getStatus) Like UCase("*paga*") Then
'            status = "pago"
'        Else
'            status = "pendente"
'        End If

'        vencimento = bot.FindElementByXPath("//*[@id='boxComFatura']/div/p[2]").Text
'        vencimento = Right(vencimento, 10)

'        '2ª via de conta:
'        bot.FindElementByXPath(btnContas).Click
'        If Not checarElementos(btnGCDetalhada) Then
'            bot.FindElementByXPath(btnContas).Click
'            bot.FindElementByXPath("//*[@id='menuV2']/li[2]/ul/li[1]").Click
'            bot.FindElementByXPath("//*[@id='opcoes-fatura']").Click
'            bot.FindElementByXPath("//*[@id='opcoes-fatura']/div[2]/ul/li[1]").Click
'            Exit Sub
'        End If

'        bot.FindElementByXPath("//*[@id='menuV2']/li[2]/ul/li[6]").Click
'        ref = bot.FindElementByXPath("/html/body/section/div/div[2]/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[1]").Text

'        bot.FindElementByXPath("//*[@id='menuV2']/li[1]").Click
'        If checarElementos("//*[@id='botaoSegundaVia']") Then
'            bot.FindElementByXPath("//*[@id='botaoSegundaVia']").Click
'        Else
'            bot.FindElementByXPath("//*[@id='box_dots']").Click
'            'bot.FindElementByXPath("//*[@id='menu_fatura']/ul/li[1]").Click
'            'bot.FindElementByXPath("//*[@id='boxComFatura']/div/div[5]").Click
'        End If

'        bot.SwitchToParentFrame

'        Call tratamento_PDF2()

'        Exit Sub
'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

'    End Sub

'    Sub download_CDC()
'        On Error GoTo handler

'        bot.SwitchToFrame 0

'bot.FindElementByXPath("//*[@id='menuV2']/li[2]").Click
'        If Not checarElementos("//*[@id='menuV2']/li[2]/ul/li[6]") Then
'            bot.FindElementByXPath("//*[@id='menuV2']/li[2]").Click
'            bot.FindElementByXPath("//*[@id='menuV2']/li[2]/ul/li[1]").Click
'            bot.FindElementByXPath("//*[@id='opcoes-fatura']").Click
'            bot.FindElementByXPath("//*[@id='opcoes-fatura']/div[2]/ul/li[1]").Click
'            Exit Sub
'        End If

'        bot.FindElementByXPath("//*[@id='menuV2']/li[2]/ul/li[6]").Click
'        'REF = bot.FindElementByXPath("/html/body/section/div/div[2]/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[1]").text

'        If checarElementos("//*[@id='content']/div/div/div/div[1]/div/div/div/p[" & i & "]") Then
'            For i = 1 To 15
'                j = i
'                On Error Resume Next
'                COD = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/p[" & i & "]").Text
'                COD = Right(COD, 12)
'                If conta = COD Then
'                    vencimento = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & i & "]/table/tbody/tr[1]/td[2]").Text
'                    ref = bot.FindElementByXPath("//*[@id='content']/div/div/div/div/div[1]/div/div/div[" & i & "]/table/tbody/tr[1]/td[1]").Text
'                    'status = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[4]").text
'                    'Clica no botão de download:
'                    bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & i & "]/table/tbody/tr[1]/td[5]").Click
'                    bot.SwitchToParentFrame
'                    On Error GoTo 0
'                End If
'            Next i
'        Else
'            ref = bot.FindElementByXPath("/html/body/section/div/div[2]/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[1]").Text
'            bot.FindElementByXPath("/html/body/section/div/div[2]/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[2]").Click
'            bot.SwitchToParentFrame
'        End If

'        Call tratamento_CDC()

'        Exit Sub

'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

'    End Sub


'    Sub download_CDC2()
'        On Error GoTo handler

'        bot.SwitchToFrame 0

''Download da fatura em .CDC
'        bot.FindElementByXPath(btnMinhaConta).Click
'        bot.FindElementByXPath(btnMCDetalhada).Click

'        If checarElementos("//*[@id='content']/div/div/div/div/div[1]/div/div/div[1]/table/thead/tr") Then
'            For i = 1 To 6
'                On Error Resume Next
'                COD = bot.FindElementByXPath("//*[@id='content']/div/div/div/div/div[1]/div/div/div[" & i & "]/table/thead/tr/td").Text 'situação da conta
'                COD = Right(COD, 12)
'                If conta = COD Then
'                    ref = bot.FindElementByXPath("//*[@id='content']/div/div/div/div/div[1]/div/div/div[" & i & "]/table/tbody/tr[1]/td[1]").Text
'                    'clica no botão de download
'                    bot.FindElementByXPath("//*[@id='content']/div/div/div/div/div[1]/div/div/div[" & i & "]/table/tbody/tr[1]/td[2]").Click
'                    bot.SwitchToParentFrame

'                    Call tratamento_CDC2()

'                    Exit Sub
'                    On Error GoTo 0
'                End If
'            Next i
'        End If

'        Exit Sub

'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

'    End Sub
'    Sub tratamento_PDF()
'        Dim tratarPdf As String
'        On Error GoTo handler

'        ref = Right(ref, 8)
'        'prepara nome do arquivo
'        tratarPdf = conta + "_" + ref + ".pdf"

'        Select Case True
'            Case ref Like "*01/20*"
'                tratarPdf = Replace(tratarPdf, "/01/20", "01")
'            Case ref Like "*02/20*"
'                tratarPdf = Replace(tratarPdf, "/02/20", "02")
'            Case ref Like "*03/20*"
'                tratarPdf = Replace(tratarPdf, "/03/20", "03")
'            Case ref Like "*04/20*"
'                tratarPdf = Replace(tratarPdf, "/04/20", "04")
'            Case ref Like "**05/20**"
'                tratarPdf = Replace(tratarPdf, "/05/20", "05")
'            Case ref Like "*06/20*"
'                tratarPdf = Replace(tratarPdf, "/06/20", "06")
'            Case ref Like "*07/20*"
'                tratarPdf = Replace(tratarPdf, "/07/20", "07")
'            Case ref Like "*08/20*"
'                tratarPdf = Replace(tratarPdf, "/08/20", "08")
'            Case ref Like "*09/20*"
'                tratarPdf = Replace(tratarPdf, "/09/20", "09")
'            Case ref Like "*10/20*"
'                tratarPdf = Replace(tratarPdf, "/10/20", "10")
'            Case ref Like "*11/20*"
'                tratarPdf = Replace(tratarPdf, "/11/20", "11")
'            Case ref Like "12/20*"
'                tratarPdf = Replace(tratarPdf, "/12/20", "12")


'        End Select
'        '*******************************************************

'        Dim REF2

'        SLEEP2(tempoEspera)
'        REF2 = VBA.Mid(tratarPdf, 12, 4)
'100
'            Call EsperaFimDown()

'        Call PDF.tratar_pdf(tratarPdf)

'        Dim pastaEnvio, pastaDestino As String

'        'APARENTEMENTE REPETIDO, DELETAR
'        'pastaEnvio = "C:\Users\Administrador.000\Downloads\faturasRobo\" & tratarPdf
'        'pastaDestino = destino & "\" & tratarPdf

'        'On Error Resume Next ' aqui deleta arquivo com o mesmo nome na pasta de destino
'        'Set arquivoantigo = arqSys.GetFile(destino & tratarPdf)
'        'arquivoantigo.Delete
'        'On Error GoTo handler

'        'VBA.FileCopy pastaEnvio, pastaDestino   'Copia o arquivo para a pasta FATURASPDF
'        '********************************************************************



'        Call PDF.getTextFromPDF_Vivo_Fixo(pastaDestino)

'        Dim apiBitrix As New ApiBitrix
'        If apiBitrix.atualizaTriagem(idTriagem, REF2, valor, vencimento, creditos, encargos) Then
'            Call atualizaUltimoDownload(ref) 'atualiza ultimo download com referencia
'        Else
'            Call atualizaUltimoDownload("") ' atualiza ultimo download com referencia em branco
'        End If

'        Call download_CDC2()

'        Exit Sub
'        '*******************
'handler:
'        If (Err.Number = 7 And Erl() = 100) Or Err.Description Like "*Element not found for XPath=//*[@id='linhaA0']*" Then
'            vencimento = bot.FindElementByXPath("//*[@id='faturamovel_portlet_1.formGridFaturas']/div[1]/div[1]/div/div[2]/p[2]", 1).Text ' pega vencimento de faturas contestadas.
'            Resume Next
'        End If
'        'enviar_log ("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        ''stop
'        Resume


'    End Sub

'    Sub tratamento_PDF2()

'        Dim tratarPdf As String

'        tratarPdf = conta + "_" + ref + ".pdf"

'        Select Case True
'            Case ref Like "Janeiro / 20*"
'                tratarPdf = Replace(tratarPdf, "Janeiro / 20", "01")
'            Case ref Like "Fevereiro / 20*"
'                tratarPdf = Replace(tratarPdf, "Fevereiro / 20", "02")
'            Case ref Like "Março / 20*"
'                tratarPdf = Replace(tratarPdf, "Março / 20", "03")
'            Case ref Like "Abril / 20*"
'                tratarPdf = Replace(tratarPdf, "Abril / 20", "04")
'            Case ref Like "Maio / 20*"
'                tratarPdf = Replace(tratarPdf, "Maio / 20", "05")
'            Case ref Like "Junho / 20*"
'                tratarPdf = Replace(tratarPdf, "Junho / 20", "06")
'            Case ref Like "Julho / 20*"
'                tratarPdf = Replace(tratarPdf, "Julho / 20", "07")
'            Case ref Like "Agosto / 20*"
'                tratarPdf = Replace(tratarPdf, "Agosto / 20", "08")
'            Case ref Like "Setembro / 20*"
'                tratarPdf = Replace(tratarPdf, "Setembro / 20", "09")
'            Case ref Like "Outubro / 20*"
'                tratarPdf = Replace(tratarPdf, "Outubro / 20", "10")
'            Case ref Like "Novembro / 20*"
'                tratarPdf = Replace(tratarPdf, "Novembro / 20", "11")
'            Case ref Like "Dezembro / 20**"
'                tratarPdf = Replace(tratarPdf, "Dezembro / 20", "12")


'        End Select
'        '*******************************************************

'        Dim REF2
'        Dim vencimento

'        SLEEP2(tempoEspera)
'        REF2 = VBA.Mid(tratarPdf, 12, 4)
'100

'            Call EsperaFimDown()

'        Call PDF.tratar_pdf(tratarPdf)

'        Dim pastaEnvio, pastaDestino As String
'        pastaEnvio = "C:\Users\Thais\Downloads\" & tratarPdf
'        pastaDestino = "C:\Users\Thais\Documents\FATURASPDF\" & tratarPdf

'        VBA.FileCopy pastaEnvio, pastaDestino

'            Call PDF.getTextFromPDF_Vivo_Fixo(pastaEnvio)

'        Dim apiBitrix As New ApiBitrix
'        If apiBitrix.atualizaTriagem(idTriagem, REF2, valor, vencimento, creditos, encargos) Then
'            Call atualizaUltimoDownload(ref) 'atualiza ultimo download com referencia
'        Else
'            Call atualizaUltimoDownload("") ' atualiza ultimo download com referencia em branco
'        End If

'        Call download_CDC()

'        Exit Sub
'        '*******************
'handler:
'        If (Err.Number = 7 And Erl() = 100) Or Err.Description Like "*Element not found for XPath=//*[@id='linhaA0']*" Then
'            vencimento = bot.FindElementByXPath("//*[@id='faturamovel_portlet_1.formGridFaturas']/div[1]/div[1]/div/div[2]/p[2]", 1).Text ' pega vencimento de faturas contestadas.
'            Resume Next
'        End If
'        'enviar_log ("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        ''stop
'        'resume


'    End Sub

'    Sub tratamento_CDC()

'        Dim tratarCDC As String

'        'Fevereiro / 2019
'        'prepara nome do arquivo
'        tratarCDC = conta + "_" + ref + ".txt"

'        Select Case True
'            Case ref Like "Janeiro / 20*"
'                tratarCDC = Replace(tratarCDC, "Janeiro / 20", "01")
'            Case ref Like "Fevereiro / 20*"
'                tratarCDC = Replace(tratarCDC, "Fevereiro / 20", "02")
'            Case ref Like "Março / 20*"
'                tratarCDC = Replace(tratarCDC, "Março / 20", "03")
'            Case ref Like "Abril / 20*"
'                tratarCDC = Replace(tratarCDC, "Abril / 20", "04")
'            Case ref Like "Maio / 20*"
'                tratarCDC = Replace(tratarCDC, "Maio / 20", "05")
'            Case ref Like "Junho / 20*"
'                tratarCDC = Replace(tratarCDC, "Junho / 20", "06")
'            Case ref Like "Julho / 20*"
'                tratarCDC = Replace(tratarCDC, "Julho / 20", "07")
'            Case ref Like "Agosto / 20*"
'                tratarCDC = Replace(tratarCDC, "Agosto / 20", "08")
'            Case ref Like "Setembro / 20*"
'                tratarCDC = Replace(tratarCDC, "Setembro / 20", "09")
'            Case ref Like "Outubro / 20*"
'                tratarCDC = Replace(tratarCDC, "Outubro / 20", "10")
'            Case ref Like "Novembro / 20*"
'                tratarCDC = Replace(tratarCDC, "Novembro / 20", "11")
'            Case ref Like "Dezembro / 20**"
'                tratarCDC = Replace(tratarCDC, "Dezembro / 20", "12")

'        End Select
'        '*******************************************************

'        Dim REF2
'        Dim vencimento

'        SLEEP2(tempoEspera)
'        REF2 = VBA.Mid(tratarCDC, 12, 4)
'100
'            Call EsperaFimDown()
'        Call PDF.tratar_cdc(tratarCDC)
'        Dim pastaEnvio, pastaDestino As String
'        pastaEnvio = "C:\Users\Thais\Downloads\" & tratarCDC
'        pastaDestino = "C:\Users\Thais\Documents\INTERMEDIARIA\" & tratarCDC

'        VBA.FileCopy pastaEnvio, pastaDestino

'            Call checarPagamentos()

'        Exit Sub
'        '*******************
'handler:
'        If (Err.Number = 7 And Erl() = 100) Or Err.Description Like "*Element not found for XPath=//*[@id='linhaA0']*" Then
'            vencimento = bot.FindElementByXPath("//*[@id='faturamovel_portlet_1.formGridFaturas']/div[1]/div[1]/div/div[2]/p[2]", 1).Text ' pega vencimento de faturas contestadas.
'            Resume Next
'        End If
'        'enviar_log ("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        ''stop
'        'resume


'    End Sub

'    Sub tratamento_CDC2()

'        Dim tratarCDC As String

'        'prepara nome do arquivo
'        tratarCDC = conta + "_" + ref + ".txt"

'        Select Case True
'            Case ref Like "Janeiro/20*"
'                tratarCDC = Replace(tratarCDC, "Janeiro/20", "01")
'            Case ref Like "Fevereiro/20*"
'                tratarCDC = Replace(tratarCDC, "Fevereiro/20", "02")
'            Case ref Like "Março/20*"
'                tratarCDC = Replace(tratarCDC, "Março/20", "03")
'            Case ref Like "Abril/20*"
'                tratarCDC = Replace(tratarCDC, "Abril/20", "04")
'            Case ref Like "Maio/20*"
'                tratarCDC = Replace(tratarCDC, "Maio/20", "05")
'            Case ref Like "Junho/20*"
'                tratarCDC = Replace(tratarCDC, "Junho/20", "06")
'            Case ref Like "Julho/20*"
'                tratarCDC = Replace(tratarCDC, "Julho/20", "07")
'            Case ref Like "Agosto/20*"
'                tratarCDC = Replace(tratarCDC, "Agosto/20", "08")
'            Case ref Like "Setembro/20*"
'                tratarCDC = Replace(tratarCDC, "Setembro/20", "09")
'            Case ref Like "Outubro/20*"
'                tratarCDC = Replace(tratarCDC, "Outubro/20", "10")
'            Case ref Like "Novembro/20*"
'                tratarCDC = Replace(tratarCDC, "Novembro/20", "11")
'            Case ref Like "Dezembro/20**"
'                tratarCDC = Replace(tratarCDC, "Dezembro/20", "12")


'        End Select

'        '*******************************************************

'        Dim REF2

'        SLEEP2(tempoEspera)
'        REF2 = VBA.Mid(tratarCDC, 12, 4)
'100
'            Call EsperaFimDown()
'        Call PDF.tratar_cdc(tratarCDC)
'        Dim pastaEnvio, pastaDestino As String
'        pastaEnvio = DirFaturasRobo & tratarCDC
'        pastaDestino = destino & "\" & tratarCDC
'        VBA.FileCopy pastaEnvio, pastaDestino

'            Set arquivoantigo = arqSys.GetFile(pastaEnvio)
'            arquivoantigo.Delete

'        Call checarPagamentos()

'        Exit Sub
'        '*******************
'    End Sub

'    Sub logoff()

'        If checarElementos("//*[@id='mainView']/section/div[2]/div/article/h1") Then
'            Exit Sub
'        End If

'        On Error GoTo atualiza
'        bot.Refresh
'        bot.Wait(8000)
'        bot.FindElementByXPath(headerLogoff).Click

'        On Error GoTo handler
'        'Sair:
'        bot.FindElementByXPath(btnSair).Click

'        Exit Sub
'atualiza:
'        bot.Get "https://meuvivoempresas.vivo.com.br"
'Call logoff()

'handler:
'        Exit Sub

'    End Sub

'    Sub logoffPF()
'        On Error GoTo handler

'        bot.FindElementByXPath("//*[@id='user-info-dropdown']").Click
'        bot.FindElementByXPath("//*[@id='user-info-dropdown']/div/div[2]/div[2]/div[2]").Click
'        bot.FindElementByXPath("//*[@id='btnSim']").Click

'        Exit Sub
'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

'    End Sub

'    Sub procedimentoPF()
'        On Error GoTo handler

'        bot.FindElementByXPath("//*[@id='linha_gestor']").SendKeys LINHA
'bot.FindElementByXPath("//*[@id='cpf_gestor']").SendKeys CPF
'bot.FindElementByXPath("//*[@id='senha_movel']").SendKeys SENHA
'bot.FindElementByXPath("//*[@id='loginMovel']").Click
'        bot.Wait(15000)

'        'Verifica se algum dado foi enviado incorretamente ou se faltou algum dado
'        If checarElementos("//*[@id='msg_linha_gestor']") Then
'            Call enviarLog("Linha de gestor no formato inválido", False)
'            login = False
'            Exit Sub
'        End If

'        'Verifica se não foi possível carregar o processo: "Ops"
'        If checarElementos("//*[@id='no_products_blank']/div/div") Then
'            If checarElementos("//*[@id='no_products_blank']/div/div/div/h1") Then
'                Dim msgOps As String
'                msgOps = bot.FindElementByXPath("//*[@id='no_products_blank']/div/div/div/h1").Text
'                If UCase(msgOps) Like UCase("Ops!") Then
'                    login = False
'                    Call enviarLog("Não foi possível carregar o conteúdo", False)
'                Else
'                    login = True
'                    Call procedimentoPF1()
'                    Call downloadPF()
'                    Call tratamentoPDF_PF()
'                    Call checarPagamentos()
'                    Call enviarLog("Fatura baixada", True)
'                End If
'            End If
'        ElseIf checarElementos("//*[@id='faturamovel_portlet_1.formGridFaturas']/table") Then
'            login = True
'            Call procedimentoPF1()
'            Call downloadPF()
'            Call tratamentoPDF_PF()
'            Call checarPagamentos()
'            Call enviarLog("Fatura baixada", True)
'        End If

'        salvaCNPJ = cnpj

'        Exit Sub

'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

'    End Sub

'    Sub downloadPF()
'        On Error GoTo handler

'        bot.FindElementByXPath("//*[@id='linhaA0']/td[5]").Click
'        bot.FindElementByXPath("//*[@id='downloadFatura0']").Click

'        Exit Sub

'handler:

'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

'    End Sub


'    Sub procedimentoPF1()
'        On Error GoTo handler

'        If checarElementos("//*[@id='linhaA0']") Then
'            For i = 0 To 0
'                On Error Resume Next
'                status = bot.FindElementByXPath("//*[@id='linhaA" & i & "']/td[3]", 1).Text 'situação da conta
'                vencimento = bot.FindElementByXPath("//*[@id='linhaA" & i & "']/td[2]", 1).Text 'vencimento da conta
'                valor = bot.FindElementByXPath("//*[@id='linhaA" & i & "']/td[4]", 1).Text ' valor da fatura
'                'CreditosDiversos = bot.FindElementByXPath("//*[@id='linhaB" & i & "']/td[1]", 1).text  ' valor fatura contestada
'                ref = bot.FindElementByXPath("//*[@id='linhaA" & i & "']/td[1]", 1).Text ' referencia da fatura
'                On Error GoTo 0
'            Next i
'        End If

'        Exit Sub
'handler:
'        Stop


'    End Sub


'    Sub enviarLog(log As String, Optional dados_ok As Boolean)
'        On Error GoTo handler

'        Dim conexaoMysql As conexaoMysql
'Set conexaoMysql = New conexaoMysql

'sql = "UPDATE contas_vivo_fixo SET  conta_ultimoLog" + " = " + VBA.Chr(34) + log & Now & VBA.Chr(34) + " WHERE conta_numero = " + VBA.Chr(34) + conta + VBA.Chr(34) + ";"
'        conexaoMysql.conectar(sql)

'        log = log & " " & Now & "|" & campo_log

'        sql = "UPDATE contas_vivo_fixo SET  conta_logRobo" + " = " + VBA.Chr(34) + log + VBA.Chr(34) + " WHERE conta_numero = " + VBA.Chr(34) + conta + VBA.Chr(34) + ";"
'        conexaoMysql.conectar(sql)


'        If dados_ok Then
'            sql = "UPDATE contas_vivo_fixo SET conta_dadosOk = " + VBA.Chr(34) + "1" + VBA.Chr(34) + " WHERE conta_numero = " + VBA.Chr(34) + conta + VBA.Chr(34) + ";"
'        ElseIf Not dados_ok Then
'            sql = "UPDATE contas_vivo_fixo SET conta_dadosOk = " + VBA.Chr(34) + "0" + VBA.Chr(34) + " WHERE conta_numero = " + VBA.Chr(34) + conta + VBA.Chr(34) + ";"
'        End If
'        conexaoMysql.conectar(sql)


'        Exit Sub
'        '*******************
'handler:
'        MsgBox "erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source
''Stop
'        'Resume

'    End Sub

'    Private Sub SLEEP2(ByVal x As Integer)

'        On Error GoTo handler
'        Dim Tempo As Date

'        temmpo = Now
'        Tempo = Now + x / 86400 / 1000

'        While Now < Tempo
'            DoEvents
'    Wend

'Exit Sub
'        '*******************
'handler:
'        MsgBox "erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source
'Stop
'        Resume

'    End Sub


'    Private Function ConfirmaDown() As Boolean
'        On Error GoTo handler

'        Dim arquivo As file
'        Dim Diretorio As Folder
'        Dim dir As String
'        Dim fso As FileSystemObject
'        Dim Parametros

'    Set fso = CreateObject("Scripting.FileSystemObject")

'    dir = DirFaturasRobo

'    Set Diretorio = fso.GetFolder(dir)
'    For Each arquivo In Diretorio.Files
'            If UCase(arquivo.Name) Like UCase("*19*") Or UCase(arquivo.Name) Like UCase("*SEGUNDA_VIA*") Or UCase(arquivo.Name) Like UCase("*2019*") Or UCase(arquivo.Name) Like UCase("*downloadConta*") Then
'                ConfirmaDown = True
'                Exit Function
'            Else
'                ConfirmaDown = False
'            End If
'        Next

'        Exit Function
'        '*******************
'handler:
'        'enviar_log ("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        ''stop
'        'resume

'    End Function

'    Sub EsperaFimDown()
'        Dim CONTAant
'        Dim sucesso
'        Dim semaforo

'        Dim b As Boolean

'        While b <> True
'            DoEvents
'            SLEEP2 1000
'        b = ConfirmaDown()
'    Wend
'    CONTAant = conta
'        sucesso = False
'        semaforo = True


'    End Sub

'    Sub atualizaUltimoDownload(ref)
'        On Error GoTo handler

'        Dim conexaoMysql As conexaoMysql
'Set conexaoMysql = New conexaoMysql


'sql = "UPDATE contas_vivo_fixo SET  conta_ultimoDownload" + " = " + Chr(34) + ref + Chr(34) + " WHERE conta_numero = " + Chr(34) + conta + Chr(34) + ";"
'        conexaoMysql.conectar(sql)

'        sql = "UPDATE contas_vivo_fixo SET  conta_valor" + " = " + Chr(34) & valor & Chr(34) + " WHERE conta_numero = " + Chr(34) + conta + Chr(34) + ";"
'        conexaoMysql.conectar(sql)

'        sql = "UPDATE contas_vivo_fixo SET  conta_creditos" + " = " + Chr(34) & creditos & Chr(34) + " WHERE conta_numero = " + Chr(34) + conta + Chr(34) + ";"
'        conexaoMysql.conectar(sql)

'        sql = "UPDATE contas_vivo_fixo SET  conta_encargos" + " = " + Chr(34) & encargos & Chr(34) + " WHERE conta_numero = " + Chr(34) + conta + Chr(34) + ";"
'        conexaoMysql.conectar(sql)

'        Sheets("planilha1").Range("b1").Value = numerador ' guarda numerador

'        Exit Sub
'        '*******************
'handler:
'        MsgBox "erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source
'Stop
'        Resume


'    End Sub

'    Sub checarPagamentos()
'        Dim conexaoMysql As conexaoMysql
'Set conexaoMysql = New conexaoMysql
'On Error GoTo handler

'        Dim faturas(5)
'        Dim faturas2(5)
'        Dim i
'        Dim fatura3
'        Dim sql

'        bot.SwitchToFrame 0

'If checarElementos(btnMinhaConta) Then   'modelo solução de voz

'            bot.FindElementByXPath(btnMinhaConta).Click
'            bot.FindElementByXPath(btnMinhasFaturas).Click

'            For i = 0 To 5
'                On Error Resume Next
'                faturas(0) = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & j & "]/table/tbody/tr[" & i + 1 & "]/td[4]").Text    'status da fatura
'                faturas(1) = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & j & "]/table/tbody/tr[" & i + 1 & "]/td[2]").Text      'vencimento da fatura
'                faturas(2) = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & j & "]/table/tbody/tr[" & i + 1 & "]/td[3]").Text   ' valor da fatura
'                faturas(3) = ""        ' valor fatura contestada
'                faturas(4) = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & j & "]/table/tbody/tr[" & i + 1 & "]/td[1]").Text ' referencia da fatura
'                faturas(5) = Now  'data da atualização
'                On Error GoTo 0
'                faturas2(i) = Join(faturas, ";")
'            Next i

'        ElseIf checarElementos(btnContas) Then  'modelo voz fixa

'            bot.FindElementByXPath(btnContas).Click
'            bot.FindElementByXPath(btn2viaContas).Click

'            For i = 0 To 5
'                On Error Resume Next
'                bot.SwitchToFrame 0
'        bot.FindElementByXPath("//*[@id='circle-interno" & i + 5 & "']").ScrollIntoView
'                bot.FindElementByXPath("//*[@id='circle-interno" & i + 6 & "']").Click '//*[@id="circle-interno6"]
'                bot.FindElementByXPath("//*[@id='circle-interno" & i + 6 & "']").ScrollIntoView
'                faturas(0) = bot.FindElementByXPath("//*[@id='titulo']").Text    'status da fatura
'                Dim getStatus As String
'                If UCase(faturas(0)) Like UCase("*paga*") Then
'                    status = "pago"
'                Else
'                    status = "pendente"
'                End If
'                faturas(1) = bot.FindElementByXPath("//*[@id='vencimento']").Text      'vencimento da fatura
'                faturas(1) = Right(faturas(1), 8)
'                faturas(2) = bot.FindElementByXPath("//*[@id='valor']").Text   ' valor da fatura
'                faturas(3) = ""        ' valor fatura contestada
'                faturas(4) = "" ' referencia da fatura
'                faturas(5) = Now  'data da atualização
'                bot.SwitchToParentFrame
'                On Error GoTo 0
'                faturas2(i) = Join(faturas, ";")
'            Next i
'        End If

'        fatura3 = Join(faturas2, "|")

'        sql = "UPDATE contas_vivo_fixo SET  conta_pagamentos " + " = " + VBA.Chr(34) + fatura3 + VBA.Chr(34) + " WHERE conta_numero = " + VBA.Chr(34) + conta + VBA.Chr(34) + ";"
'        conexaoMysql.conectar(sql)

'        bot.SwitchToParentFrame

'        salvaCNPJ = cnpj

'        Call enviarLog("Faturas baixadas", True)

'        Exit Sub
'        '*******************
'handler:
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        ''stop
'        'resume
'        Exit Sub

'proximoScroll:
'        bot.FindElementByXPath("//*[@id='circle-interno" & i + 7 & "']").ScrollIntoView

'        Resume
'    End Sub

'    Sub downloadUltimaFatura(Optional tempoEspera As Integer = 3000)
'        On Error GoTo handler

'        Dim tratarPdf As String

'        'abre o menu da última fatura
'        bot.Wait 300
'    bot.FindElementByXPath("//*[@id='linhaA0']/td[5]").Click
'        bot.Wait 300

'    If checarPresenca("//*[@id='divMenu0']") Then 'verifica se está com formato de conta atualizada
'            'caminho para conta contestada
'            If checarPresenca("//*[@id='downloadBoleto0']") Then 'verifica se a conta está atrasada
'                ' caminho para conta atrasada
'                bot.FindElementByXPath("//*[@id='visualizarFatura0']").Click
'                bot.FindElementByXPath("//*[@id='doDownloadFatura']").Click
'                Call enviar_log("conta baixada", True)

'            Else
'                'caminho para conta em dia
'                bot.FindElementByXPath("//*[@id='downloadFatura0']").Click
'                Call enviar_log("conta baixada", True)

'            End If

'        Else
'            bot.FindElementByXPath("//*[@id='divA0']/div").Click 'caminho para conta orgiginal
'            If checarPresenca("//*[@id='divMenu0']") Then
'                bot.FindElementByXPath("//*[@id='downloadFatura0']").Click
'                Call enviar_log("conta baixada", True)


'            Else
'                Call enviar_log("impossível baixar fatura, fatura não encontrada", True)

'            End If
'        End If

'        'prepara nome do arquivo
'        tratarPdf = conta + "_" + ref + ".pdf"

'        Select Case True
'            Case ref Like "*Jan*"
'                tratarPdf = Replace(tratarPdf, "Jan/20", "01")
'            Case ref Like "*Fev*"
'                tratarPdf = Replace(tratarPdf, "Fev/20", "02")
'            Case ref Like "*Mar*"
'                tratarPdf = Replace(tratarPdf, "Mar/20", "03")
'            Case ref Like "*Abr*"
'                tratarPdf = Replace(tratarPdf, "Abr/20", "04")
'            Case ref Like "*Mai*"
'                tratarPdf = Replace(tratarPdf, "Mai/20", "05")
'            Case ref Like "*Jun*"
'                tratarPdf = Replace(tratarPdf, "Jun/20", "06")
'            Case ref Like "*Jul*"
'                tratarPdf = Replace(tratarPdf, "Jul/20", "07")
'            Case ref Like "*Ago*"
'                tratarPdf = Replace(tratarPdf, "Ago/20", "08")
'            Case ref Like "*Set*"
'                tratarPdf = Replace(tratarPdf, "Set/20", "09")
'            Case ref Like "*Out*"
'                tratarPdf = Replace(tratarPdf, "Out/20", "10")
'            Case ref Like "*Nov*"
'                tratarPdf = Replace(tratarPdf, "Nov/20", "11")
'            Case ref Like "*Dez*"
'                tratarPdf = Replace(tratarPdf, "Dez/20", "12")

'        End Select
'        '*******************************************************
'        Dim REF2
'        Dim vencimento

'        SLEEP2(tempoEspera)
'        REF2 = VBA.Mid(tratarPdf, 12, 4)
'100
'            vencimento = bot.FindElementByXPath("//*[@id='linhaA0']/td[2]", 1).Text

'        Call EsperaFimDown()

'        Call PDF.tratarPdf(tratarPdf)
'        Dim apiBitrix As New ApiBitrix
'        If apiBitrix.atualizaTriagem(idTriagem, REF2, valor, vencimento, creditos, encargos) Then
'            Call atualizaUltimoDownload(ref) 'atualiza ultimo download com referencia
'        Else
'            Call atualizaUltimoDownload("") ' atualiza ultimo download com referencia em branco
'        End If
'        Exit Sub
'        '*******************
'handler:
'        If (Err.Number = 7 And Erl() = 100) Or Err.Description Like "*Element not found for XPath=//*[@id='linhaA0']*" Then
'            vencimento = bot.FindElementByXPath("//*[@id='faturamovel_portlet_1.formGridFaturas']/div[1]/div[1]/div/div[2]/p[2]", 1).Text ' pega vencimento de faturas contestadas.
'            Resume Next
'        End If
'        enviar_log("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        ''stop
'        'resume


'    End Sub
'    Sub tratamentoPDF_PF(Optional tempoEspera As Integer = 3000)
'        On Error GoTo handler

'        Dim tratarPdf As String

'        'prepara nome do arquivo
'        tratarPdf = conta + "_" + ref + ".pdf"

'        Select Case True
'            Case ref Like "*Jan*"
'                tratarPdf = Replace(tratarPdf, "Jan/20", "01")
'            Case ref Like "*Fev*"
'                tratarPdf = Replace(tratarPdf, "Fev/20", "02")
'            Case ref Like "*Mar*"
'                tratarPdf = Replace(tratarPdf, "Mar/20", "03")
'            Case ref Like "*Abr*"
'                tratarPdf = Replace(tratarPdf, "Abr/20", "04")
'            Case ref Like "*Mai*"
'                tratarPdf = Replace(tratarPdf, "Mai/20", "05")
'            Case ref Like "*Jun*"
'                tratarPdf = Replace(tratarPdf, "Jun/20", "06")
'            Case ref Like "*Jul*"
'                tratarPdf = Replace(tratarPdf, "Jul/20", "07")
'            Case ref Like "*Ago*"
'                tratarPdf = Replace(tratarPdf, "Ago/20", "08")
'            Case ref Like "*Set*"
'                tratarPdf = Replace(tratarPdf, "Set/20", "09")
'            Case ref Like "*Out*"
'                tratarPdf = Replace(tratarPdf, "Out/20", "10")
'            Case ref Like "*Nov*"
'                tratarPdf = Replace(tratarPdf, "Nov/20", "11")
'            Case ref Like "*Dez*"
'                tratarPdf = Replace(tratarPdf, "Dez/20", "12")

'        End Select
'        '*******************************************************
'        Dim REF2
'        Dim vencimento

'        SLEEP2(tempoEspera)
'        REF2 = VBA.Mid(tratarPdf, 12, 4)
'100
'            Call EsperaFimDown()

'        Call PDF.tratar_pdf(tratarPdf)

'        Dim pastaEnvio, pastaDestino As String
'        pastaEnvio = "C:\Users\Thais\Downloads\" & tratarPdf
'        pastaDestino = "C:\Users\Thais\Documents\FATURASPDF\" & tratarPdf

'        VBA.FileCopy pastaEnvio, pastaDestino

'            Call PDF.getTextFromPDF(pastaEnvio)

'        Dim apiBitrix As New ApiBitrix
'        If apiBitrix.atualizaTriagem(idTriagem, REF2, valor, vencimento, creditos, encargos) Then
'            Call atualizaUltimoDownload(ref) 'atualiza ultimo download com referencia
'        Else
'            Call atualizaUltimoDownload("") ' atualiza ultimo download com referencia em branco
'        End If
'        Exit Sub
'        '*******************
'handler:
'        If (Err.Number = 7 And Erl() = 100) Or Err.Description Like "*Element not found for XPath=//*[@id='linhaA0']*" Then
'            vencimento = bot.FindElementByXPath("//*[@id='faturamovel_portlet_1.formGridFaturas']/div[1]/div[1]/div/div[2]/p[2]", 1).Text ' pega vencimento de faturas contestadas.
'            Resume Next
'        End If
'        enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        ''stop
'        'resume

'    End Sub
'    '
'    'Sub procDownloads_tipo1()
'    'On Error GoTo handler
'    '
'    '
'    '            Call download_PDF
'    '            Call tratamento_PDF2
'    '            Call checarPagamentos
'    '            Call download_CDC
'    '            Call tratamento_CDC
'    '            Call enviarLog("Faturas baixadas", True)
'    '            salvaCNPJ = cnpj
'    '
'    'Exit Sub
'    ''*******************
'    'handler:
'    'enviarLog ("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'    '
'    'End Sub
'    '
'    'Sub procDownloads_tipo2()
'    'On Error GoTo handler
'    '
'    '            Call download_PDF2
'    '            Call tratamento_PDF
'    '            Call checarPagamentos
'    '            Call download_CDC2
'    '            Call tratamento_CDC2
'    '            Call enviarLog("Faturas baixadas", True)
'    '            salvaCNPJ = cnpj
'    '
'    'Exit Sub
'    ''*******************
'    'handler:
'    'enviarLog ("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'    '
'    'End Sub


'End Class
