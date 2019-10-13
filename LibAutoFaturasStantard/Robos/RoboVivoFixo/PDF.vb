'Public Class PDF
'    Public pdfName As String
'    Public nome_arquivo As String
'    Dim valor_total As String
'    Dim CreditosDeValores As String
'    Dim CreditosDiversos As String
'    Dim encargos As String
'    Dim vencimento As String
'    Dim PastaIntermediaria As String
'    Public PastaSistema As String
'    Public PastaFaturasRobo As String

'    Sub intialize()
'        PastaIntermediaria = "C:\SISTEMA4D\VIVOFXDADOS\TXT_IMPORTAR\INTERMEDIARIA\"
'        PastaSistema = "C:\SISTEMA4D\VIVOFXDADOS\TXT_IMPORTAR\"
'        PastaFaturasRobo = "C:\Users\User\Downloads\faturasRobo\"
'    End Sub

'    Sub tratar_pdf(Optional nome_do_arquivo As String)
'        On Error GoTo handler

'        nome_arquivo = nome_do_arquivo
'        Dim NewName As String
'        Dim strPath As StringL
'        Dim FileConta As String

'        'encontra o arquivo baixado mais recente na pasta faturasRobo

'        Dim arqSys As FileSystemObject
'        Dim objArq As file
'        Dim minhaPasta
'        Dim nomeArq As String
'        Dim dataArq As Date
'        Dim arquivopdf
'        Dim Diret

'        Diret = PastaFaturasRobo

'    Set arqSys = New FileSystemObject
'    Set minhaPasta = arqSys.GetFolder(Diret)
'    dataArq = DateSerial(1900, 1, 1)
'        For Each objArq In minhaPasta.Files
'            If objArq.DateLastModified > dataArq _
'         And objArq.Name Like "*.pdf" Then
'                dataArq = objArq.DateLastModified
'                nomeArq = objArq.Name
'            Set arquivopdf = objArq
'        End If
'        Next objArq

'        '*******************************************************************************************

'        ' verifica se a pasta indicada no bd existe, caso contrário salva em outra pasta para testes.
'        Dim destino
'        destino = Replace(ModuloDeTeste.portal_vivo_fixo.destino, "192.168.244.112", "servidor") & "\"
'        If VerificaPath(destino) Then
'            If arquivopdf Is Nothing Or arquivopdf.DateLastModified < Now - 10000 Then     'checa se houve download pegando o arquivo mais recente no sistema, e também verificando se ele está ele foi modificado há menos de 10 segundos
'                Call portal_vivo.downloadUltimaFatura(20000) ' erro no downlod, tenta de novo com 20 segundos de espera
'            Else
'                arquivopdf.Copy(destino)
'            End If
'        Else
'            Err.Raise 1003, , "Pasta especificada não existe."
'                       GoTo arquivoantigo
'        End If
'        SLEEP(2000)


'        nomeTemp = arquivopdf.Name
'        arquivopdf.Delete ' deleta o arquivo antigo da pasta faturasRobo
'        'captura o novo arquivo na pasta destino


'arquivoantigo:
'        Dim arquivoantigo

'        On Error Resume Next 'aqui deleta arquivo com mesmo nome se houver, caso não exista resume o erro
'    Set arquivoantigo = arqSys.GetFile(destino & nome_arquivo)
'    arquivoantigo.Delete
'        On Error GoTo handler
'        '********************************************************************************


'        Dim novonome

'Set arquivopdf = arqSys.GetFile("destino" & nomeArq) 'nome_do_arquivo
'   If nome_arquivo = "" Then nome_arquivo = "y.pdf"
'        novonome = Replace(arquivopdf.path, arquivopdf.Name, nome_arquivo)

'        Name arquivopdf.path As novonome 'renomeia o arquivos

'    Set arqSys = Nothing
'    Set minhaPasta = Nothing

'Exit Sub
'        '*******************
'handler:
'        If Err.Number = 58 Then
'Set arquivopdf = arqSys.GetFile(ModuloDeProducao.portal_vivo.destino & arquivopdf.Name)
'arquivopdf.Delete
'            Resume
'        End If
'        If Err.Number = 75 Then 'arquivo sem permisão pra excluir
'            Resume Next
'        End If
'        If Err.Number = 424 Or Err.Number = 91 Then
'            Resume Next
'        End If
'        ModuloDeTeste.portal_vivo_fixo.enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        'stop
'        Resume

'    End Sub

'    Sub getTextFromPDF_Vivo_Fixo(file)
'        On Error GoTo handler

'        Dim objAVDoc As New AcroAVDoc
'        Dim objPDDoc As New AcroPDDoc
'        Dim objPage As AcroPDPage
'        Dim objSelection As AcroPDTextSelect
'        Dim objHighlight As AcroHiliteList
'        Dim pageNum As Long
'        Dim strText As String
'        Dim ultimaColuna As Long
'        Dim conta_fatura As String



'        'prepara as funções regEx
'        conta_fatura = " \d{4} \d{4} \d{4} "
'        valor_total = "TOTAL GERAL A PAGAR\D\d+,\d+|TOTAL GERAL A PAGAR\D\d+.\d+,\d+|Valor a pagar\D\d+,\d+|Valor a pagar\D\d+.\d+,\d+"

'        CreditosDeValores = "Créditos de Valores Contestados -\d+.\d+,\d+|Créditos de Valores Contestados -\d+,\d+"
'        CreditosDiversos = "\(Crédito ou Débito\) -\d+,\d+|\(Crédito ou Débito\) -\d+.\d+,\d+|débito(s) pendente(s) no valor de R$ 187,96"
'        encargos = "Encargos \DJuros\DMulta\D\D\d+,\d+|Encargos \DJuros\DMulta\D \d+.\d+,\d+|Encargos\D\d+.\d+,\d+|Encargos\D\d+,\d+"
'        'vencimento = "Vencimento\D\d+\D\d+\D\d+"
'        '***********************************************************************************************


'        strFilename = file 'eliminar essa estapa
'        'strText = "" ' esvazia a variável

'        'prepara a conexão com o adobe
'        If (objAVDoc.Open(strFilename, "")) Then
'      Set objPDDoc = objAVDoc.GetPDDoc

''apaga arquivos convertidos da pasta da operadora
''       Dim arqSys As FileSystemObject
''            Set arqSys = New FileSystemObject
''            txtName = Replace(nome_arquivo, ".pdf", ".txt")
''            If arqSys.FileExists("C:\SISTEMA4D\VIVODADOS\TXT_IMPORTAR\" & txtName) Then
''            arqSys.DeleteFile "C:\SISTEMA4D\VIVODADOS\TXT_IMPORTAR\" & txtName, False
''            End If
''*******************************************************************************

' 'esvazia variáveis
'            conta_fatura_pdf = ""
'            valor_total_pdf = ""
'            CreditosDeValores_pdf = ""
'            CreditosDiversos_pdf = ""
'            Encargos_pdf = ""
'            vencimento_pdf = ""
'            '*******************************************************************************


'            'abre o pdf uma página por vez, executa as funções regEx, salva a página convertida na pasta da vivo
'            For pageNum = 0 To objPDDoc.GetNumPages() - 1
'        Set objPage = objPDDoc.AcquirePage(pageNum)
'        Set objHighlight = New AcroHiliteList
'         objHighlight.Add 0, 15000 ' Adjust this up if it's not getting all the text on the page
'         Set objSelection = objPage.CreatePageHilite(objHighlight)

'         If Not objSelection Is Nothing Then
'                    For tCount = 0 To objSelection.GetNumText - 1
'                        strText = strText & objSelection.GetText(tCount)
'                    Next tCount
'                End If


'                Dim txtName As String
'                Dim arqSys As FileSystemObject
'            Set arqSys = New FileSystemObject
'            txtName = Replace(nome_arquivo, ".pdf", ".txt")

'                SalvarPagina strText, PastaIntermediaria & txtName

'            'Stop
'                If conta_fatura_pdf = "" Then conta_fatura_pdf = RegularExpressionTester(strText, conta_fatura) ' corrigir o número da fatura
'                If valor_total_pdf = "" Then valor_total_pdf = RegularExpressionTester(strText, valor_total)
'                If CreditosDeValores_pdf = "" Then CreditosDeValores_pdf = RegularExpressionTester(strText, CreditosDeValores)
'                If CreditosDiversos_pdf = "" Then CreditosDiversos_pdf = RegularExpressionTester(strText, CreditosDiversos)
'                If Encargos_pdf = "" Then encargos_conta = RegularExpressionTester(strText, encargos)


'                strText = ""

'            Next pageNum
'            objAVDoc.Close 1


'      If ModuloDeTeste.portal_vivo_fixo.conta <> Replace(conta_fatura_pdf, " ", "") Then Stop

'            Dim arquivotxt
'      Set arqSys = New FileSystemObject
'            txtName = Replace(nome_arquivo, ".pdf", ".txt")
'            arqSys.CopyFile PastaIntermediaria & txtName, PastaSistema & txtName
'            arqSys.DeleteFile PastaIntermediaria & txtName

' '*******************************************************************************'*******************************************************************************


'            'End If

'            'repassa os regEx encontrados para o objeto da operadora

'            If Not valor_total_pdf = "" Then ModuloDeTeste.portal_vivo_fixo.valor = lfRetiraNumeros(valor_total_pdf)
'            If Not CreditosDeValores_pdf = "" Then creditos1 = lfRetiraNumeros(CreditosDeValores_pdf)
'            If Not CreditosDiversos_pdf = "" Then creditos2 = lfRetiraNumeros(CreditosDiversos_pdf)
'            If Not Encargos_pdf = "" Then ModuloDeTeste.portal_vivo_fixo.encargos = lfRetiraNumeros(Encargos_pdf)

'            soma_creditos = (creditos1 * 1) + (creditos2 * 1)
'            If Not soma_creditos = 0 Then _
'    ModuloDeProducao.portal_vivo.creditos = soma_creditos
'        End If
'        '*******************************************************************************


'        Exit Sub
'        '*******************
'handler:
'        ModuloDeTeste.portal_vivo_fixo.enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        'stop
'        Resume
'    End Sub

'    Sub moverCDC(file)
'        On Error GoTo handler

'        Dim objAVDoc As New AcroAVDoc
'        Dim objPDDoc As New AcroPDDoc
'        Dim objPage As AcroPDPage
'        Dim objSelection As AcroPDTextSelect
'        Dim objHighlight As AcroHiliteList
'        Dim pageNum As Long
'        Dim strText As String
'        Dim ultimaColuna As Long

'        strFilename = file 'eliminar essa estapa
'        strText = "" ' esvazia a variável

'        Dim txtName As String
'        Dim arqSys As FileSystemObject
'            Set arqSys = New FileSystemObject
'            txtName = Replace(nome_arquivo, ".cdc", ".txt")

'        SalvarPagina strText, PastaIntermediaria & txtName

'                        strText = ""


'        Exit Sub
'        '*******************
'handler:
'        ModuloDeTeste.portal_vivo_fixo.enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        'stop
'        'resume
'    End Sub


'    Private Sub SalvarPagina(ByVal Pagina As String, path As String)
'        On Error GoTo handler

'        Dim i As Integer
'        Dim arquivo As String
'        Dim find As Boolean
'        Dim LogCompleto As String



'        Close #1
' Close #2
' arquivo = path

'        Open arquivo For Append As #1
'  Print #1, Pagina

' Close #1

'Exit Sub
'        '*******************
'handler:
'        ModuloDeTeste.portal_vivo_fixo.enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        'stop
'        'resume

'    End Sub
'    Private Function RegularExpressionTester(strToSearch As String, regEx As String)
'        On Error GoTo handler

'        Dim lLast As Long

'Set objRegExp = CreateObject("VBScript.RegExp")

''configurar regEx
'objRegExp.Global = False
'        objRegExp.IgnoreCase = True
'        objRegExp.Pattern = regEx

''fazer a pesquisa
'Set regExpMatches = objRegExp.Execute(strToSearch)

'If regExpMatches.Count > 0 Then RegularExpressionTester = regExpMatches(o).Value

'        Exit Function
'        '*******************
'handler:
'        ModuloDeTeste.portal_vivo_fixo.enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        'stop
'        'resume
'    End Function
'    Private Sub SLEEP(ByVal x As Integer)
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
'        ModuloDeTeste.portal_vivo_fixo.enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        'stop
'        'resume

'    End Sub

'    'Esta função tem por objetivo retirar números de células que contenham conteúdos mistos de números e texto
'    'sem a possibilidade de serem colunados
'    Public Function lfRetiraNumeros(ByVal vValor As String) As String
'        On Error GoTo handler

'        vValor = Replace(vValor, ".", "") ' trata pontos
'        'vValor = Replace(vValor, ",", "")
'        '   vValor = Right(vValor, 7)

'        'Conta a quantidade de caracteres
'        Dim vQtdeCaract As Long
'        Dim vControle As Boolean

'        vQtdeCaract = Len(vValor)
'        vControle = False

'        'Para cada caractere identifica se é número ou texto
'        For i = 1 To vQtdeCaract
'            'Se for número adiciona no retorno da função
'            If IsNumeric(Mid(vValor, i, 1)) Then
'                If vControle = True And lfRetiraNumeros <> vbNullString Then
'                    lfRetiraNumeros = lfRetiraNumeros + " "
'                End If
'                vControle = False
'                lfRetiraNumeros = lfRetiraNumeros & Mid(vValor, i, 1)
'            Else
'                vControle = True
'            End If
'        Next

'        'Substitui espaços em branco por / e tira espaços em branco no final do retorno da função
'        lfRetiraNumeros = Replace(lfRetiraNumeros, " ", ",")


'        Dim x As Integer




'        Exit Function
'        '*******************
'handler:
'        ModuloDeTeste.portal_vivo_fixo.enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        'stop
'        'resume

'    End Function


'    Function VerificaPath(caminho)
'        On Error GoTo handler
'        Dim strPath As Variant
'        If Dir(caminho) = vbNullString Then
'            VerificaPath = False
'        Else
'            VerificaPath = True
'        End If

'        Exit Function
'        '*******************
'handler:
'        ModuloDeTeste.portal_vivo_fixo.enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        'stop
'        'resume


'    End Function

'    Sub tratar_cdc(Optional nome_do_arquivo As String)
'        On Error GoTo handler

'        nome_arquivo = nome_do_arquivo
'        Dim NewName As String
'        Dim strPath As String
'        Dim FileConta As String

'        'encontra o arquivo baixado mais recente na pasta faturasRobo

'        Dim arqSys As FileSystemObject
'        Dim objArq As file
'        Dim minhaPasta
'        Dim nomeArq As String
'        Dim dataArq As Date
'        'arquivocdc
'        Dim arquivopdf
'        Dim Diret


'        Diret = PastaFaturasRobo
'    Set arqSys = New FileSystemObject
'    Set minhaPasta = arqSys.GetFolder(Diret)
'    dataArq = DateSerial(1900, 1, 1)
'        For Each objArq In minhaPasta.Files
'            If objArq.DateLastModified > dataArq _
'         And objArq.Name Like "*.cdc" Then
'                dataArq = objArq.DateLastModified
'                nomeArq = objArq.Name
'            Set arquivopdf = objArq
'        End If
'        Next objArq

'        '*******************************************************************************************

'        ' verifica se a pasta indicada no bd existe, caso contrário salva em outra pasta para testes.
'        '
'        '   Dim destino
'        '   destino = Replace(ModuloDeProducao.portal_vivo.destino, "192.168.244.112", "servidor") & "\"
'        '        If VerificaPath(destino) Then
'        '            If arquivopdf Is Nothing Or arquivopdf.DateLastModified < Now - 10000 Then     'checa se houve download pegando o arquivo mais recente no sistema, e também verificando se ele está ele foi modificado há menos de 10 segundos
'        '                Call portal_vivo.downloadUltimaFatura(20000) ' erro no downlod, tenta de novo com 20 segundos de espera
'        '                Else
'        '                arquivopdf.Copy (destino)
'        '            End If
'        '        Else
'        '            arquivopdf.Copy ("C:\Users\Administrador.000\Downloads\faturasRobo\Caminho Teste\")
'        '            Set arquivopdf = arqSys.GetFile("C:\Users\Administrador.000\Downloads\faturasRobo\Caminho Teste\" & arquivopdf.Name) 'nome_do_arquivo
'        '            nomeTemp = arquivopdf.Name
'        '            GoTo arquivoantigo:
'        '        End If
'        '    SLEEP (2000)
'        '
'        '
'        '            nomeTemp = arquivopdf.Name
'        '            arquivopdf.Delete ' deleta o arquivo antigo da pasta faturasRobo
'        '            'captura o novo arquivo na pasta destino
'        '             Set arquivopdf = arqSys.GetFile(destino & nomeTemp) 'nome_do_arquivo
'        '
'        'arquivoantigo:
'        '    Dim arquivoantigo
'        '
'        '    On Error Resume Next 'aqui deleta arquivo com mesmo nome se houver, caso não exista resume o erro
'        '    Set arquivoantigo = arqSys.GetFile(destino & nome_arquivo)
'        '    arquivoantigo.Delete
'        '    On Error GoTo handler
'        ''********************************************************************************
'        '
'        Dim novonome

'Set arquivopdf = arqSys.GetFile(PastaFaturasRobo & nomeArq) 'nome_do_arquivo
'   If nome_arquivo = "" Then nome_arquivo = "y.cdc"
'        novonome = Replace(arquivopdf.path, arquivopdf.Name, nome_arquivo)

'        Name arquivopdf.path As novonome 'renomeia o arquivos


'    Set arqSys = Nothing
'    Set minhaPasta = Nothing

'Exit Sub
'        '*******************
'handler:
'        If Err.Number = 58 Then
'            'Set arquivopdf = arqSys.GetFile(ModuloDeTeste.portal_vivo_fixo.destino & arquivopdf.Name)
'            arquivopdf.Delete
'            Resume
'        End If
'        If Err.Number = 75 Then 'arquivo sem permisão pra excluir
'            Resume Next
'        End If
'        If Err.Number = 424 Or Err.Number = 91 Then
'            Resume Next
'        End If
'        ModuloDeTeste.portal_vivo_fixo.enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        'stop
'        'resume

'    End Sub

'    Sub getContaFromPDF_Vivo_Fixo()
'        On Error GoTo handler

'        Dim objAVDoc As New AcroAVDoc
'        Dim objPDDoc As New AcroPDDoc
'        Dim objPage As AcroPDPage
'        Dim objSelection As AcroPDTextSelect
'        Dim objHighlight As AcroHiliteList
'        Dim pageNum As Long
'        Dim strText As String
'        Dim ultimaColuna As Long

'        'prepara a função regEx
'        num_Conta = "Código do cliente: \d\d\d\d \d\d\d\d \d\d\d\d"
'        '***********************************************************************************************

'        'prepara a conexão com o adobe
'        If (objAVDoc.Open(strFilename, "")) Then
'      Set objPDDoc = objAVDoc.GetPDDoc

' 'esvazia variável
'            num_Conta_pdf = ""

'            '*******************************************************************************

'            'abre o pdf uma página por vez, executa as funções regEx, salva a página convertida na pasta da vivo
'            For pageNum = 0 To objPDDoc.GetNumPages() - 1
'         Set objPage = objPDDoc.AcquirePage(pageNum)
'         Set objHighlight = New AcroHiliteList
'         objHighlight.Add 0, 15000 ' Adjust this up if it's not getting all the text on the page
'         Set objSelection = objPage.CreatePageHilite(objHighlight)

'         If Not objSelection Is Nothing Then
'                    For tCount = 0 To objSelection.GetNumText - 1
'                        strText = strText & objSelection.GetText(tCount)
'                    Next tCount
'                End If

'            Next pageNum
'            objAVDoc.Close 1

'   End If

'        'repassa os regEx encontrados para o objeto da operadora

'        If Not num_Conta_pdf = "" Then
'            Conta = num_Conta_pdf
'        End If

'        Exit Sub
'        '*******************
'handler:
'        ModuloDeTeste.portal_vivo_fixo.enviarLog("erro não previsto" & Err.Description & " " & Err.Number & " " & Err.Source)
'        'stop
'        'Resume

'    End Sub




'End Class
