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
    Public Function ConverterPdfParaTxt(Filepath As String, DestinoPath As String, fatura As Fatura) As String
        Dim NrDaContaArquivo As String = ""


        Me.conta = GerRelDB.Contas.Where(Function(x) x.Faturas.Contains(fatura)).First

        Dim paginas As Integer = ObterNumeroDePaginas(Filepath, fatura)

        If paginas = -1 Then
            Throw New PdfCorrompidoException(fatura, "Fatura Corrompida")
        End If

        Dim TextPagina As String
        Dim dadosRegex As New DadosRegex


        Try
            Regexer.SetarPadores(dadosRegex.Relatorios(conta.Operadora + conta.TipoDeConta))
        Catch ex As KeyNotFoundException
            'segue a vida se não encontrar a key
        End Try



        Using sw As New StreamWriter(DestinoPath.Replace(".pdf", ".txt"), True)
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
        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)

        If conta.Operadora = OperadoraEnum.VIVO And conta.TipoDeConta = TipoContaEnum.MOVEL Then
            Return Regex.Match(texto, "\b\d{10}\b").Value
        ElseIf conta.Operadora = OperadoraEnum.CLARO And conta.TipoDeConta = TipoContaEnum.MOVEL Then
            Return Regex.Match(texto, "Nº da Conta: (\d{9})\b").Groups(1).Value
        ElseIf conta.Operadora = OperadoraEnum.OI And conta.TipoDeConta = TipoContaEnum.MOVEL Then
            Throw New NotImplementedException
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
        sw.Write(textPagina)
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
    Public Function ObterNumeroDePaginas(FilePath As String, fatura As Fatura) As Integer
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

        Dim paginas As Integer = ObterNumeroDePaginas(arquivoPath, fatura)

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

        If conta.Operadora = OperadoraEnum.VIVO And conta.TipoDeConta = TipoContaEnum.MOVEL Then

            Dim rawRef = Regex.Match(texto, "de Referência: (\d{2}/\d{4})").Groups(1).Value

            Dim ref
            If rawRef <> "" Then
                ref = rawRef.Substring(0, 2) + rawRef.Substring(5, 2)
            Else
                ref = ""
            End If


            Return ref

        ElseIf conta.Operadora = OperadoraEnum.CLARO And conta.TipoDeConta = TipoContaEnum.MOVEL Then
            Return ""
        ElseIf conta.Operadora = OperadoraEnum.OI And conta.TipoDeConta = TipoContaEnum.MOVEL Then
            Throw New NotImplementedException
        ElseIf conta.Operadora = OperadoraEnum.TIM And conta.TipoDeConta = TipoContaEnum.MOVEL Then

            Dim RawRerefencia = Regex.Match(texto, "REF: (\w{3}/\d{2})").Groups(1).Value
            Dim Referencia As String = TratarRawReferencia(RawRerefencia)
            Return Referencia
        End If


    End Function

    Private Function TratarRawReferencia(rawRerefencia As String) As String

        Dim partes As String() = rawRerefencia.Split("/")
        Dim ano = partes(1)
        Dim mesRaw = partes(0)
        Dim mes As String = ""

        Select Case mesRaw
            Case "JAN"
                mes = "01"
            Case "FEV"
                mes = "02"
            Case "MAR"
                mes = "03"
            Case "ABR"
                mes = "04"
            Case "MAI"
                mes = "05"
            Case "JUN"
                mes = "06"
            Case "JUL"
                mes = "07"
            Case "AGO"
                mes = "08"
            Case "SET"
                mes = "09"
            Case "OUT"
                mes = "10"
            Case "NOV"
                mes = "11"
            Case "DEZ"
                mes = "12"
        End Select

        Return mes + ano

    End Function
End Class

