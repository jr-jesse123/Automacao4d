Imports OpenQA.Selenium.Chrome

Public Class portalvivofixo
    Private bancoDados()
    Public cnpj, CPFATUAL, SENHA, CPF, LINHA, conta, OPERADORA, COD, salvaCNPJ, vencimento, status, produto, tratarPdf, destino, idTriagem, ultimoDownload As String
    Public bot As ChromeDriver
    Public numerador As Integer

    Public ref As String
    Public valor As Double
    Public creditos As Double
    Public encargos As Double
    Dim login As Boolean
    Dim j

    Public modeloVF1, modeloVF2, btnLinha, modeloSV, campoSENHA, btnProduto, btnMCDetalhada, btnSair, headerLogoff, telaEstado, presencaFaturas, telaDeslogada, btn2viaContas, campoCNPJ, msgErro, btnContinuar, btnEntrar, acessarFixo, perfilPF, listaProdutos, escolhaVF, btnContas, btnGCDetalhada, btnMinhaConta, btnMinhasFaturas, ausenciaFatura, selecioneRD, solDeDados, campoVencRD, numContaRD, btnConfirmarRD, Z As String
    Public paginaLogin, nLinha As String
    Public DirFaturasRobo As String


    Sub variaveis()


        paginaLogin = "https://meuvivoempresas.vivo.com.br/meuvivoempresas/appmanager/portal/fixo?_nfpb=true&_nfls=false&_pageLabel=empNegMVivo2FixoPymesBook&pFlutua=true"
        modeloVF1 = "//*[@id='integracaoMarcaOauth_empNegMVEFVivo2HomePage']"
        modeloVF2 = "//*[@id='boxComFatura']"
        modeloSV = "//*[@id='meuvivo-list-item']"
        btnLinha = "//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[3]"
        btnProduto = "//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[2]/button/div/span[2]"
        telaEstado = "/html/body/div[5]/div/div[2]/div[1]/div[3]/a[1]"
        telaDeslogada = "//*[@id='mainView']/section/div[2]/div/article/h1"
        campoCNPJ = "//*[@id='cpf_cnpj']"
        acessarFixo = "//*[@id='acessar_como_fixo']/a"
        perfilPF = "//*[@id='div_cpf_gestor']"
        escolhaVF = "//*[@id='loginPJ_vivo_fixo']/div[2]"
        campoSENHA = "//*[@id='senha_fixo']"
        msgErro = "/html/body/div[1]/div/div/div/div/div/div[3]"
        btnContas = "//*[@id='menuV2']/li[2]"
        btnGCDetalhada = "//*[@id='menuV2']/li[2]/ul/li[6]"
        btnEntrar = "//*[@id='loginFixo']"
        btnContinuar = "loginPJ_cpfcnpj_button"
        btnMinhaConta = "//*[@id='tabs']/div[1]/ul/li[2]"
        btnMinhasFaturas = "//*[@id='tabs-2']/ul/li[1]"
        listaProdutos = "//*[@id='formSelectedItem']/ul/li[2]"
        ausenciaFatura = "/html/body/div[1]/div/div/div/div[2]/div/div"
        btn2viaContas = "//*[@id='menuV2']/li[2]/ul/li[1]"
        btnMCDetalhada = "//*[@id='tabs-2']/ul/li[3]"
        selecioneRD = "//*[@id='formValidaCadastro']/div/div[1]/div/span/div[2]"
        solDeDados = "//*[@id='sel_option']/li[2]"
        campoVencRD = "//*[@id='inputDiaVencimentoDados']"
        numContaRD = "//*[@id='inputNumeroConta']"
        btnConfirmarRD = "//*[@id='defaultMVE_btn_confirmar']"
        presencaFaturas = "//*[@id='content']/div/div/div/div[1]/div/div/div/div[1]"
        btnSair = "//*[@id='headerSubmenu_1_2']/div/div[1]/div[1]/div/div[3]/div"
        headerLogoff = "//*[@id='headerSubmenu_1_2']/div/div[1]/div[1]/div/div[3]"



        Exit Sub

    End Sub



    Sub enviarDados()

        If Not login = False Then   'Verifica se o login foi realmente realizado na sub de procedimentosLogin
            Call tratarFatura()       'Chama a sub que irá verificar o tipo de conta e direcionar para seu modelo específico de downloads

        ElseIf salvaCNPJ = cnpj Then        'Nesse caso, como se trata de conta de mesmo cnpj, não será necessário logoff

            bot.SwitchTo.Frame(0)
            If Utilidades.ChecarPresenca(bot, "//*[@id='tabs']/div[1]/ul/li[1]") Then 'Verifica se a página é do modelo de Solução de Voz...
                bot.FindElementByXPath("//*[@id='tabs']/div[1]/ul/li[1]").Click()
                bot.SwitchTo.ParentFrame()

                Call tratarFatura()
            ElseIf Utilidades.ChecarPresenca(bot, "//*[@id='menuV2']/li[1]") Then   '...ou se a página é do modelo de Voz Fixa
                bot.FindElementByXPath("//*[@id='menuV2']/li[1]").Click()
                bot.SwitchTo.ParentFrame()
                Call tratarFatura()

            ElseIf Not Utilidades.ChecarPresenca(bot, "//*[@id='linhaA0']") Then
                bot.SwitchTo.ParentFrame()

                If Utilidades.ChecarPresenca(bot, "//*[@id='linhaA0']") Then
                    salvaCNPJ = cnpj
                    Call logoff()
                    '                    Call procedimentosLogin()
                End If
                If login = True Then
                    Call tratarFatura()
                End If
            Else
                salvaCNPJ = cnpj
                Call tratarFatura()
                bot.SwitchTo.ParentFrame()

            End If
        End If

    End Sub



    Sub tratarFatura()


        'Captura o tipo de conta que consta na página inicial
        bot.SwitchTo.ParentFrame()

        If Utilidades.ChecarPresenca(bot, btnProduto) Then
            produto = bot.FindElementByXPath(btnProduto).Text
            produto = Mid(produto, 8)
        End If

        If OPERADORA = "VOZ FIXA" Then
            Call tipo_vozFixa()
            Exit Sub

        ElseIf OPERADORA = "SOLUÇÃO DE VOZ" Then
            Call tipo_solucaoVoz()
            Exit Sub

        ElseIf OPERADORA = "INTERNET" Then
            Call tipo_internet()
            Exit Sub

        ElseIf OPERADORA = "SOLUÇÃO DE REDES DE DADOS" Then
            Call tipo_solucaoRD()
            Exit Sub

        ElseIf OPERADORA = "INTERNETCORPORATIVA" Then
            Call tipo_internetCorporativa()
            Exit Sub
        End If

    End Sub
    Sub tipo_vozFixa()


        bot.Navigate.Refresh()

        bot.SwitchTo.ParentFrame()
        'Verifica se a página já está no modelo de Voz Fixa
        If Utilidades.ChecarPresenca(bot, modeloVF1) Then
            nLinha = bot.FindElementByXPath(btnLinha).Text
            nLinha = Right(nLinha, 14)
            If nLinha = LINHA Then
                Call download_PDF()
            Else
                On Error GoTo handlerLinha
                bot.FindElementByXPath(btnLinha).Click()
                bot.FindElementByLinkText(LINHA).Click()
                Call download_PDF()
            End If
        Else
            bot.SwitchTo.ParentFrame()
            On Error GoTo handlerProduto
            bot.FindElementByXPath(btnProduto).Click()
            bot.FindElementByLinkText(OPERADORA).Click()
            nLinha = bot.FindElementByXPath(btnLinha).Text
            nLinha = Right(nLinha, 14)
            If LINHA = nLinha Then
                Call download_PDF()
            Else
                On Error GoTo handlerLinha
                bot.FindElementByXPath(btnLinha).Click()
                bot.FindElementByLinkText(LINHA).Click()
                Call download_PDF()
            End If
        End If

        Exit Sub

unico:
        Call download_PDF()
        Exit Sub

handlerProduto:
        'Call enviarLog("Conta não encontrada", True)
        Exit Sub
handlerLinha:
        'Call enviarLog("Linha não encontrada", True)

    End Sub

    Sub tipo_solucaoVoz()
        On Error GoTo handler

        If UCase(produto) Like UCase("*Solução de Voz*") Then
            If Not Utilidades.ChecarPresenca(bot, ausenciaFatura) Then
                Call download_PDF2()
            Else
                'Call enviarLog("Conta sem fatura", True)
            End If
        Else


            bot.FindElementByXPath(btnProduto).Click()
            bot.FindElementByLinkText("Solução de Voz").Click()
            bot.FindElementByLinkText(OPERADORA).Click()
            Call download_PDF2()
        End If

        Exit Sub
handler:
        'Call enviarLog("Produto não encontrado", True)

    End Sub

    Sub tipo_internet()
        On Error GoTo handler

        bot.SwitchTo.Frame(0)
        If Utilidades.ChecarPresenca(bot, msgErro) Then                     'Verifica se aparece mensagem de erro
            bot.Navigate.Refresh()
        End If

        bot.SwitchTo.ParentFrame()

        If UCase(produto) Like UCase("*Internet*") Then     'Verifica se a página já está no modelo de Voz Fixa
            If Not Utilidades.ChecarPresenca(bot, ausenciaFatura) Then
                Call download_PDF()
            Else
                'Call enviarLog("Conta sem fatura", True)
            End If
            Exit Sub
        ElseIf Utilidades.ChecarPresenca(bot, btnProduto) Then
            bot.FindElementByXPath(btnProduto).Click()
            bot.FindElementByLinkText(OPERADORA).Click()
            Call download_PDF()
        End If

        Exit Sub

handler:
        'Call enviarLog("Produto não localizado", True)

    End Sub

    Sub tipo_solucaoRD()
        On Error GoTo handler

        If UCase(produto) Like UCase("Solução Rede de Dados") Then
            If Utilidades.ChecarPresenca(bot, btnLinha) Then
                bot.FindElementByXPath(btnLinha).Click()      'No caso de Solução de Rede de Dados, o endereço é o mesmo de Linha, porém se refere ao número de conta
                bot.FindElementByLinkText(conta).Click()

                If Not Utilidades.ChecarPresenca(bot, ausenciaFatura) Then
                    If Utilidades.ChecarPresenca(bot, selecioneRD) Then
                        bot.FindElementByXPath(selecioneRD).Click()
                        bot.FindElementByXPath(solDeDados).Click()
                        bot.FindElementByXPath(campoVencRD).SendKeys(vencimento) 'No caso de Solução de Rede de Dados será necessário enviar o vencimento
                        bot.FindElementByXPath(numContaRD).SendKeys(conta)
                        bot.FindElementByXPath(btnConfirmarRD).Click()
                        'Não sei como procede após isso pois não tive um exemplo de conta do tipo Solução de Rede de Dados
                    End If
                End If
            End If
        Else
            bot.FindElementByXPath(btnProduto).Click()
            bot.FindElementByLinkText(OPERADORA).Click()
            Call download_PDF2()
        End If

handler:
        'Call enviarLog("Produto não localizado", True)

    End Sub

    Sub tipo_internetCorporativa()
        On Error GoTo handler

        If UCase(produto) Like UCase("Internet Corporativa") Then
            If Not Utilidades.ChecarPresenca(bot, ausenciaFatura) Then
                bot.FindElementByXPath("//*[@id='formSelectedItem']/ul/li[5]").Click()
                Call download_PDF2()
            Else
                'Call enviarLog("Conta sem fatura", True)
            End If
        Else
            bot.FindElementByXPath(btnProduto).Click()
            bot.FindElementByLinkText(OPERADORA).Click()
            Call download_PDF2()
        End If


handler:
        'Call enviarLog("Produto não localizado", True)
    End Sub

    Sub download_PDF2()

        'Clica nas abas até chegar na opção de download das faturas em PDF
        bot.SwitchTo.Frame(0)

        bot.FindElementByXPath(btnMinhaConta).Click()
        bot.FindElementByXPath(btnMinhasFaturas).Click()


        'Busca em qual div está a fatura da conta selecionada e faz o download
        If Utilidades.ChecarPresenca(bot, presencaFaturas) Then
            For i = 1 To 15
                j = i
                On Error Resume Next
                COD = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/p[" & i & "]").Text 'situação da conta
                COD = Right(COD, 12)
                If conta = COD Then
                    ref = bot.FindElementByXPath("//*[@id='content']/div/div/div/div/div[1]/div/div/div[" & i & "]/table/tbody/tr[1]/td[1]").Text
                    If ref = ultimoDownload Then Exit Sub ' deixar ultimodownload publico

                    vencimento = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & i & "]/table/tbody/tr[1]/td[2]").Text
                    status = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & i & "]/table/tbody/tr[1]/td[4]").Text
                    'clica no botão de download
                    bot.FindElementByXPath("//*[@id='content']/div/div/div/div/div[1]/div/div/div[" & i & "]/table/tbody/tr[1]/td[5]").Click()
                    bot.SwitchTo.ParentFrame()
                    ' Call tratamento_PDF()
                    Exit Sub
                    On Error GoTo 0
                End If
            Next i

        Else
            'Significa que na página consta apenas uma conta (o layout é diferente)
            ref = bot.FindElementByXPath("/html/body/section/div/div[2]/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[1]").Text
            bot.FindElementByXPath("//*[@id='btnDown']").Click()
            bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[5]").Click()
            status = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[4]").Text
            bot.SwitchTo.ParentFrame()
            'Call tratamento_PDF()
        End If


        Exit Sub

handler:
        'enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

    End Sub

    Sub download_PDF()


        bot.SwitchTo.ParentFrame()
        bot.SwitchTo.Frame(0)
        Dim getStatus As String
        getStatus = bot.FindElementByXPath("//*[@id='boxComFatura']/div/h4").Text
        If UCase(getStatus) Like UCase("*paga*") Then
            status = "pago"
        Else
            status = "pendente"
        End If

        vencimento = bot.FindElementByXPath("//*[@id='boxComFatura']/div/p[2]").Text
        vencimento = Right(vencimento, 10)

        '2ª via de conta:
        bot.FindElementByXPath(btnContas).Click()

        If Not Utilidades.ChecarPresenca(bot, btnGCDetalhada) Then
            bot.FindElementByXPath(btnContas).Click()
            bot.FindElementByXPath("//*[@id='menuV2']/li[2]/ul/li[1]").Click()
            bot.FindElementByXPath("//*[@id='opcoes-fatura']").Click()
            bot.FindElementByXPath("//*[@id='opcoes-fatura']/div[2]/ul/li[1]").Click()
            Exit Sub
        End If

        bot.FindElementByXPath("//*[@id='menuV2']/li[2]/ul/li[6]").Click()
        ref = bot.FindElementByXPath("/html/body/section/div/div[2]/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[1]").Text

        bot.FindElementByXPath("//*[@id='menuV2']/li[1]").Click()

        If Utilidades.ChecarPresenca(bot, "//*[@id='botaoSegundaVia']") Then
            bot.FindElementByXPath("//*[@id='botaoSegundaVia']").Click()
        Else
            bot.FindElementByXPath("//*[@id='box_dots']").Click()
            bot.FindElementByXPath("//*[@id='menu_fatura']/ul/li[1]").Click()
            bot.FindElementByXPath("//*[@id='boxComFatura']/div/div[5]").Click()
        End If

        bot.SwitchTo.ParentFrame()

        'Call tratamento_PDF2()

        Exit Sub
handler:
        'enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

    End Sub

    Sub download_CDC()
        On Error GoTo handler

        bot.SwitchTo.Frame(0)

        bot.FindElementByXPath("//*[@id='menuV2']/li[2]").Click()

        If Not Utilidades.ChecarPresenca(bot, "//*[@id='menuV2']/li[2]/ul/li[6]") Then
            bot.FindElementByXPath("//*[@id='menuV2']/li[2]").Click()
            bot.FindElementByXPath("//*[@id='menuV2']/li[2]/ul/li[1]").Click()
            bot.FindElementByXPath("//*[@id='opcoes-fatura']").Click()
            bot.FindElementByXPath("//*[@id='opcoes-fatura']/div[2]/ul/li[1]").Click()
            Exit Sub
        End If

        bot.FindElementByXPath("//*[@id='menuV2']/li[2]/ul/li[6]").Click()
        ref = bot.FindElementByXPath("/html/body/section/div/div[2]/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[1]").Text

        If Utilidades.ChecarPresenca(bot, "//*[@id='content']/div/div/div/div[1]/div/div/div/p[" & "]") Then
            For i = 1 To 15
                j = i
                On Error Resume Next
                COD = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/p[" & i & "]").Text
                COD = Right(COD, 12)
                If conta = COD Then
                    vencimento = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & i & "]/table/tbody/tr[1]/td[2]").Text
                    ref = bot.FindElementByXPath("//*[@id='content']/div/div/div/div/div[1]/div/div/div[" & i & "]/table/tbody/tr[1]/td[1]").Text
                    status = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[4]").Text
                    'Clica no botão de download
                    bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & i & "]/table/tbody/tr[1]/td[5]").Click()
                    bot.SwitchTo.ParentFrame()
                    On Error GoTo 0
                End If
            Next i
        Else
            ref = bot.FindElementByXPath("/html/body/section/div/div[2]/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[1]").Text
            bot.FindElementByXPath("/html/body/section/div/div[2]/div[1]/div/div/div/div[1]/table/tbody/tr[1]/td[2]").Click()
            bot.SwitchTo.ParentFrame()
        End If

        'Call tratamento_CDC()

        Exit Sub

handler:
        'enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

    End Sub


    Sub download_CDC2()
        On Error GoTo handler

        bot.SwitchTo.Frame(0)

        'Download da fatura em .CDC
        bot.FindElementByXPath(btnMinhaConta).Click()
        bot.FindElementByXPath(btnMCDetalhada).Click()

        If Utilidades.ChecarPresenca(bot, "//*[@id='content']/div/div/div/div/div[1]/div/div/div[1]/table/thead/tr") Then
            For i = 1 To 6
                On Error Resume Next
                COD = bot.FindElementByXPath("//*[@id='content']/div/div/div/div/div[1]/div/div/div[" & i & "]/table/thead/tr/td").Text 'situação da conta
                COD = Right(COD, 12)
                If conta = COD Then
                    ref = bot.FindElementByXPath("//*[@id='content']/div/div/div/div/div[1]/div/div/div[" & i & "]/table/tbody/tr[1]/td[1]").Text
                    'clica no botão de download
                    bot.FindElementByXPath("//*[@id='content']/div/div/div/div/div[1]/div/div/div[" & i & "]/table/tbody/tr[1]/td[2]").Click()
                    bot.SwitchTo.ParentFrame()

                    'Call tratamento_CDC2()

                    Exit Sub
                    On Error GoTo 0
                End If
            Next i
        End If

        Exit Sub

handler:
        'enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

    End Sub

    Sub logoff()

        If Utilidades.ChecarPresenca(bot, "//*[@id='mainView']/section/div[2]/div/article/h1") Then
            Exit Sub
        End If

        On Error GoTo atualiza
        bot.Navigate.Refresh()
        Threading.Thread.Sleep(8000)
        bot.FindElementByXPath(headerLogoff).Click()

        On Error GoTo handler
Sair:
        bot.FindElementByXPath(btnSair).Click()

        Exit Sub
atualiza:
        bot.Navigate.GoToUrl("https://meuvivoempresas.vivo.com.br")
        Call logoff()

handler:
        Exit Sub

    End Sub

    Sub logoffPF()
        On Error GoTo handler

        bot.FindElementByXPath("//*[@id='user-info-dropdown']").Click()
        bot.FindElementByXPath("//*[@id='user-info-dropdown']/div/div[2]/div[2]/div[2]").Click()
        bot.FindElementByXPath("//*[@id='btnSim']").Click()

        Exit Sub
handler:
        'enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

    End Sub

    Sub procedimentoPF()
        On Error GoTo handler

        bot.FindElementByXPath("//*[@id='linha_gestor']").SendKeys(LINHA)
        bot.FindElementByXPath("//*[@id='cpf_gestor']").SendKeys(CPF)
        bot.FindElementByXPath("//*[@id='senha_movel']").SendKeys(SENHA)
        bot.FindElementByXPath("//*[@id='loginMovel']").Click()
        Threading.Thread.Sleep(15000)

        'Verifica se algum dado foi enviado incorretamente ou se faltou algum dado
        If Utilidades.ChecarPresenca(bot, "//*[@id='msg_linha_gestor']") Then
            'Call enviarLog("Linha de gestor no formato inválido", False)
            login = False
            Exit Sub
        End If

        'Verifica se não foi possível carregar o processo:   "Ops"
        If Utilidades.ChecarPresenca(bot, "//*[@id='no_products_blank']/div/div") Then
            If Utilidades.ChecarPresenca(bot, "//*[@id='no_products_blank']/div/div/div/h1") Then
                Dim msgOps As String
                msgOps = bot.FindElementByXPath("//*[@id='no_products_blank']/div/div/div/h1").Text
                If UCase(msgOps) Like UCase("Ops!") Then
                    login = False
                    'Call enviarLog("Não foi possível carregar o conteúdo", False)
                Else
                    login = True
                    Call procedimentoPF1()
                    Call downloadPF()
                    '                    Call tratamentoPDF_PF()
                    Call checarPagamentos()
                    '                   Call enviarLog("Fatura baixada", True)
                End If
            End If
        ElseIf Utilidades.ChecarPresenca(bot, "//*[@id='faturamovel_portlet_1.formGridFaturas']/table") Then
            login = True
            Call procedimentoPF1()
            Call downloadPF()
            'Call tratamentoPDF_PF()
            Call checarPagamentos()
            'Call enviarLog("Fatura baixada", True)
        End If

        salvaCNPJ = cnpj

        Exit Sub

handler:
        'enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

    End Sub

    Sub downloadPF()
        On Error GoTo handler

        bot.FindElementByXPath("//*[@id='linhaA0']/td[5]").Click()
        bot.FindElementByXPath("//*[@id='downloadFatura0']").Click()

        Exit Sub

handler:

        'enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)

    End Sub


    Sub procedimentoPF1()
        On Error GoTo handler

        If Utilidades.ChecarPresenca(bot, "//*[@id='linhaA0']") Then
            For i = 0 To 0
                On Error Resume Next
                status = bot.FindElementByXPath("//*[@id='linhaA" & i & "']/td[3]").Text 'situação da conta
                vencimento = bot.FindElementByXPath("//*[@id='linhaA" & i & "']/td[2]").Text 'vencimento da conta
                valor = bot.FindElementByXPath("//*[@id='linhaA" & i & "']/td[4]").Text ' valor da fatura
                'CreditosDiversos = bot.FindElementByXPath("//*[@id='linhaB" & i & "']/td[1]", 1).text  ' valor fatura contestada
                ref = bot.FindElementByXPath("//*[@id='linhaA" & i & "']/td[1]").Text ' referencia da fatura
                On Error GoTo 0
            Next i
        End If

        Exit Sub
handler:
        Stop


    End Sub



    Sub checarPagamentos()

        Dim faturas(5)
        Dim faturas2(5)
        Dim i
        Dim fatura3
        Dim sql

        bot.SwitchTo.Frame(0)

        If Utilidades.ChecarPresenca(bot, btnMinhaConta) Then   'modelo solução de voz

            bot.FindElementByXPath(btnMinhaConta).Click()
            bot.FindElementByXPath(btnMinhasFaturas).Click()

            For i = 0 To 5
                On Error Resume Next
                faturas(0) = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & j & "]/table/tbody/tr[" & i + 1 & "]/td[4]").Text    'status da fatura
                faturas(1) = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & j & "]/table/tbody/tr[" & i + 1 & "]/td[2]").Text      'vencimento da fatura
                faturas(2) = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & j & "]/table/tbody/tr[" & i + 1 & "]/td[3]").Text   ' valor da fatura
                faturas(3) = ""        ' valor fatura contestada
                faturas(4) = bot.FindElementByXPath("//*[@id='content']/div/div/div/div[1]/div/div/div/div[" & j & "]/table/tbody/tr[" & i + 1 & "]/td[1]").Text ' referencia da fatura
                faturas(5) = Now  'data da atualização
                On Error GoTo 0
                faturas2(i) = Join(faturas, ";")
            Next i

        ElseIf Utilidades.ChecarPresenca(bot, btnContas) Then  'modelo voz fixa

            bot.FindElementByXPath(btnContas).Click()
            bot.FindElementByXPath(btn2viaContas).Click()

            For i = 0 To 5
                On Error Resume Next
                bot.SwitchTo.Frame(0)
                'bot.FindElementByXPath("//*[@id='circle-interno" & i + 5 & "']").ScrollIntoView
                bot.FindElementByXPath("//*[@id='circle-interno" & i + 6 & "']").Click() '//*[@id="circle-interno6"]
                'bot.FindElementByXPath("//*[@id='circle-interno" & i + 6 & "']").ScrollIntoView
                faturas(0) = bot.FindElementByXPath("//*[@id='titulo']").Text    'status da fatura
                Dim getStatus As String
                If UCase(faturas(0)) Like UCase("*paga*") Then
                    status = "pago"
                Else
                    status = "pendente"
                End If
                faturas(1) = bot.FindElementByXPath("//*[@id='vencimento']").Text      'vencimento da fatura
                faturas(1) = Right(faturas(1), 8)
                faturas(2) = bot.FindElementByXPath("//*[@id='valor']").Text   ' valor da fatura
                faturas(3) = ""        ' valor fatura contestada
                faturas(4) = "" ' referencia da fatura
                faturas(5) = Now  'data da atualização
                bot.SwitchTo.ParentFrame()
                On Error GoTo 0
                faturas2(i) = Join(faturas, ";")
            Next i
        End If

        fatura3 = Join(faturas2, "|")


proximoScroll:
        'bot.FindElementByXPath("//*[@id='circle-interno" & i + 7 & "']").ScrollIntoView

        Resume
    End Sub


    Sub procDownloads_tipo1()

        Call download_PDF()
        Call checarPagamentos()
        Call download_CDC()

        Exit Sub

    End Sub

    Sub procDownloads_tipo2()

        Call download_PDF2()
        Call checarPagamentos()
        Call download_CDC2()


        salvaCNPJ = cnpj
        Exit Sub

    End Sub


End Class
