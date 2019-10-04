Imports System.ComponentModel
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading
Imports Acrobat
Imports BibliotecaAutomacaoFaturas
Imports iText7

Public Class LeitorPDF

    Public Property conta As Conta

    Private AppAcrobat As AcroApp
    Private objAVDoc As New AcroAVDoc
    Private objPDDoc As New AcroPDDoc
    Private objPage As AcroPDPage
    Private objSelection As AcroPDTextSelect
    Private objHighlight As AcroHiliteList
    Private pageNum As Long
    Private strText As String = ""

    Private Property Regexer As Regexer

    Sub New(Regexer As Regexer)
        Me.Regexer = Regexer

    End Sub
    ''' <summary>
    ''' Converte o documento de pdf para txt.
    ''' </summary>
    ''' <param name="Filepath">caminho do documento a ser convertido</param>
    ''' <param name="DestinoPath">caminho do arquivo txt a ser craido</param>
    ''' <param name="fatura"></param>
    ''' <returns></returns>
    Public Function ConverterPdfParaTxt(Filepath As String, DestinoPath As String, Optional fatura As Fatura = Nothing) As String
        Dim NrDaContaArquivo As String = ""

        If fatura IsNot Nothing Then
            Me.conta = GerRelDB.Contas.Where(Function(x) x.Faturas.Contains(fatura)).First
        End If


        Dim paginas As Integer = ObterNumeroDePaginas(Filepath)

        If paginas = -1 Then
            Throw New PdfCorrompidoException(fatura, "Fatura Corrompida")
        End If

        Dim TextPagina As String
        Dim dadosRegex As New DadosRegex

        Dim ddregex
        If conta IsNot Nothing Then
            ddregex = dadosRegex.Relatorios(conta.Operadora + conta.TipoDeConta)
        End If


        Try
            Regexer.SetarPadores(ddregex)
        Catch ex As KeyNotFoundException
            'segue a vida se não encontrar a key
        End Try



        Dim encoding = Text.Encoding.GetEncoding("Windows-1252")

        Using sw As New StreamWriter(DestinoPath.Replace(".pdf", ".txt"), True, encoding)
            For index = 0 To paginas - 1
                TextPagina = ConverterPagina(index)
                AdicionarPaginaTxt(TextPagina, sw)
                Regexer.RealizarRegex(TextPagina)

                If NrDaContaArquivo = "" Then
                    NrDaContaArquivo = VerificarNumeroDeConta(TextPagina, fatura)
                End If
            Next
        End Using

        Dim resultadpo = objAVDoc.Close(1)
        Dim resultadpo2 = objAVDoc.Close(0)
        Dim resultadpo3 = objAVDoc.Close(2)



        For Each padrao In Regexer.Padroes
            padrao.ConstruirRelatorio()
            fatura.Relatorios.Add(padrao)
        Next

        Return NrDaContaArquivo

    End Function


    ''' <summary>
    ''' Esta função localiza o regex adequado para conferir o número de conta em cada operadora.
    ''' </summary>
    ''' <returns></returns>
    Private Function VerificarNumeroDeConta(texto As String, fatura As Fatura) As String

        If fatura Is Nothing Then Exit Function

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)

        If conta.Operadora = OperadoraEnum.VIVO Then
            If conta.TipoDeConta = TipoContaEnum.MOVEL Then

                Return Regex.Match(texto, "\b\d{10}\b").Value
            ElseIf conta.TipoDeConta = TipoContaEnum.FIXA Then
                'Stop
                If conta.Subtipo = SubtipoEnum.InternetCorp Or conta.Subtipo = SubtipoEnum.SlnVoz Then
                    Return Regex.Match(texto, "\d{4} \d{4} \d{4} (\d+)-\d").Groups(1).Value
                ElseIf conta.Subtipo = SubtipoEnum.VozFixa Then

                    'Return Regex.Match(texto, "\b\d{10}\b").Value

                    Dim teste = Regex.Match(
                    texto, "(Número do telefone: (.+)?(6130248962)(.+)?\b)|\b\d{10}\b", RegexOptions.Multiline)

                    If teste.Value = "" Then
                        Return teste.Groups(2).Value
                    Else
                        Return teste.Value
                    End If

                    'Return Regex.Match(texto, $"Número do telefone: (.+)?({fatura.NrConta})(.+)?\b").Groups(2).Value
                End If
            End If
        ElseIf conta.Operadora = OperadoraEnum.CLARO And conta.TipoDeConta = TipoContaEnum.MOVEL Then
            Return Regex.Match(texto, "Nº da Conta: (\d{9})\b").Groups(1).Value
        ElseIf conta.Operadora = OperadoraEnum.OI And conta.TipoDeConta = TipoContaEnum.MOVEL Then
            Return Regex.Match(texto, "NÚMERO DO CLIENTE: (\d+)").Groups(1).Value
        ElseIf conta.Operadora = OperadoraEnum.TIM And conta.TipoDeConta = TipoContaEnum.MOVEL Then
            Return Regex.Match(texto, "CLIENTE: (\d\.\d{6,9})").Groups(1).Value
        End If

    End Function
    ''' <summary>
    ''' adiciona a pagina ao documento txt
    ''' </summary>
    ''' <param name="textPagina"></param>
    ''' <param name="sw"></param>
    Private Sub AdicionarPaginaTxt(textPagina As String, sw As StreamWriter)
        sw.Write(textPagina + Environment.NewLine)
    End Sub

    Public Function ConverterPagina(PaginaAtual As Integer)
        'prepara a conexão com o adobe
        strText = ""
        'objPDDoc = objAVDoc.GetPDDoc

        objPage = objPDDoc.AcquirePage(PaginaAtual)
        objHighlight = New AcroHiliteList
        objHighlight.Add(0, 15000) ' Adjust this up if it's not getting all the text on the page
        objSelection = objPage.CreatePageHilite(objHighlight)

        If Not objSelection Is Nothing Then
            For tCount = 0 To objSelection.GetNumText - 1
                strText = strText & objSelection.GetText(tCount)
            Next tCount
        End If

        Return strText

    End Function
    ''' <summary>
    ''' abre o documento para conferir o número de páginas dentro dele
    ''' </summary>
    ''' <param name="FilePath"></param>
    ''' <returns>Retorna o número de páginas do documento, retorna -1 caso não seja possível ler o docuemnto</returns>
    Public Function ObterNumeroDePaginas(FilePath As String) As Integer
        Dim retorno1
        Dim retorno2
        Dim retorno3

        Try
            retorno1 = objAVDoc.Close(0)
            retorno2 = objAVDoc.Close(1)
            retorno3 = objAVDoc.Close(2)
        Catch ex As Exception

        End Try


        ' verifica se o arquivo está corrompido
        Dim arquivo As New FileInfo(FilePath)

        If arquivo.Length < 1000 Then
            Return -1
        End If

        '****************
        Try
            If (objAVDoc.Open(FilePath, "")) Then
                objPDDoc = objAVDoc.GetPDDoc
                Return objPDDoc.GetNumPages()
            Else
                Return -1
            End If

        Catch ex As Exception
            Return -1
        End Try




    End Function
    ''' <summary>
    ''' abre o documento e procura por referencia e número da fatura, com funções regex para cada operadora
    ''' </summary>
    ''' <param name="arquivoPath">caminho do arquivo a ser verificado</param>
    ''' <param name="fatura">fatura para verificar detalhes do documento</param>
    ''' <returns>retorna tupla com o valor do número de conta e o valor da referencia</returns>
    Friend Function VerificarNr_REF_DaFatura(arquivoPath As String, fatura As Fatura) As Tuple(Of String, String)

        Dim NrDaContaArquivo As String = ""
        Dim REF As String = ""



        Me.conta = GerRelDB.Contas.Where(Function(x) x.Faturas.Contains(fatura)).First

        Dim paginas As Integer = ObterNumeroDePaginas(arquivoPath)

        If paginas = -1 Then
            Throw New PdfCorrompidoException(fatura, "Fatura Corrompida")
        End If

        Dim TextPagina As String


        For index = 0 To paginas - 1
            TextPagina = ConverterPagina(index)

            If REF = "" Then
                REF = VerficarReferenciaDaFatura(TextPagina, fatura)
            End If

            If NrDaContaArquivo = "" Then
                NrDaContaArquivo = VerificarNumeroDeConta(TextPagina, fatura)
            End If

            If REF <> "" And NrDaContaArquivo <> "" Then
                Exit For
            End If

        Next


        Dim resultadpo = objAVDoc.Close(1)
        Dim resultadpo2 = objAVDoc.Close(0)
        Dim resultadpo3 = objAVDoc.Close(2)


        Return New Tuple(Of String, String)(NrDaContaArquivo, REF)

    End Function

    Private Function VerficarReferenciaDaFatura(texto As String, fatura As Fatura) As String

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)

        If conta.Operadora = OperadoraEnum.VIVO Then
            If conta.TipoDeConta = TipoContaEnum.MOVEL Then

                Dim rawRef = Regex.Match(texto, "de Referência: (\d{2}/\d{4})").Groups(1).Value

                Dim ref
                If rawRef <> "" Then
                    ref = rawRef.Substring(0, 2) + rawRef.Substring(5, 2)
                Else
                    ref = ""
                End If

                Return ref
            ElseIf conta.TipoDeConta = TipoContaEnum.FIXA Then
                If conta.Subtipo = SubtipoEnum.InternetCorp Or conta.Subtipo = SubtipoEnum.SlnVoz Then

                    Dim rawRef = Regex.Match(texto, "Mês de referência (\D+/\d{4})").Groups(1).Value
                    Dim REF = TratarRawReferencia(rawRef)
                    Return REF

                ElseIf conta.Subtipo = SubtipoEnum.VozFixa Then

                    'Stop

                    Dim rawRef = Regex.Match(texto, "[^\s|\d]+\/\d{4}").Value
                    Dim REF = TratarRawReferencia(rawRef)
                    Return REF


                End If

            End If

        ElseIf conta.Operadora = OperadoraEnum.CLARO And conta.TipoDeConta = TipoContaEnum.MOVEL Then
            Return ""
        ElseIf conta.Operadora = OperadoraEnum.OI And conta.TipoDeConta = TipoContaEnum.MOVEL Then

            Return Regex.Match(texto, "(\w{3}/\d{4})").Value
            
            ElseIf conta.Operadora = OperadoraEnum.TIM And conta.TipoDeConta = TipoContaEnum.MOVEL Then

            Dim ReGexReferencia = Regex.Match(texto, "REF: (\w{3}/\d{2})|Mês de referência: (\w+/\d+)")

            Dim RawRerefencia

            If ReGexReferencia.Groups(1).Value.Length > 0 Then
                RawRerefencia = ReGexReferencia.Groups(1).Value
            Else
                RawRerefencia = ReGexReferencia.Groups(2).Value
            End If

            Dim Referencia As String = TratarRawReferencia(RawRerefencia)
                Return Referencia
            End If


    End Function

    Private Function TratarRawReferencia(rawRerefencia As String) As String

        Dim partes As String() = rawRerefencia.Split("/")
        Dim ano = partes(1).Replace("20", "")
        Dim mesRaw = partes(0).ToUpper
        Dim mes As String = ""

        If mesRaw.Contains("JAN") Then
            mes = "01"
        ElseIf mesRaw.StartsWith("JAN") Then
            mes = "02"
        ElseIf mesRaw.StartsWith("FEV") Then
            mes = "03"
        ElseIf mesRaw.StartsWith("MAR") Then
            mes = "04"
        ElseIf mesRaw.StartsWith("ABR") Then
            mes = "05"
        ElseIf mesRaw.StartsWith("JUN") Then
            mes = "06"
        ElseIf mesRaw.StartsWith("JUL") Then
            mes = "07"
        ElseIf mesRaw.StartsWith("AGO") Then
            mes = "08"
        ElseIf mesRaw.StartsWith("SET") Then
            mes = "09"
        ElseIf mesRaw.StartsWith("OUT") Then
            mes = "10"
        ElseIf mesRaw.StartsWith("NOV") Then
            mes = "11"
        ElseIf mesRaw.StartsWith("DEZ") Then
            mes = "12"
        End If




        Return mes + ano

    End Function
End Class

