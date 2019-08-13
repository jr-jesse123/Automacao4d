Imports System.IO
Imports System.Text.RegularExpressions
Imports Acrobat
Imports BibliotecaAutomacaoFaturas
Imports iText7

Public Class ConversorPDF
    Private Filepath As String
    Public Property conta As Conta

    Private AppAcrobat As AcroApp
    Private objAVDoc As New AcroAVDoc
    Private objPDDoc As New AcroPDDoc
    Private objPage As AcroPDPage
    Private objSelection As AcroPDTextSelect
    Private objHighlight As AcroHiliteList
    Private pageNum As Long
    Private strText As String = ""
    Private Property regexer As Regexer

    Sub New(RegexerTIM As Regexer)
        Me.regexer = RegexerTIM

    End Sub

    Public Sub ConverterPdfParaTxt(Filepath As String, DestinoPath As String, fatura As Fatura)
        Me.Filepath = Filepath
        Me.conta = GerRelDB.Contas.Where(Function(x) x.Faturas.Contains(fatura)).First

        Dim paginas As Integer = ObterNumeroDePaginas()

        If paginas = -1 Then
            'CUIDAR DA EXCEÇÃO DE FATURA CORROMPIDA.
            Throw New PdfCorrompidoException(fatura, "Fatura Corrompida")
        End If

        Dim TextPagina As String


        Dim dadosRegex As New DadosRegex
        regexer.SetarPadores(dadosRegex.Relatorios(conta.Operadora + conta.TipoDeConta))



        Using sw As New StreamWriter(DestinoPath.Replace(".pdf", ".txt"), True)
            For index = 0 To paginas - 1
                TextPagina = ConverterPagina(index)
                AdicionarPaginaTxt(TextPagina, sw)
                regexer.RealizarRegex(TextPagina)
            Next
        End Using

        objAVDoc.Close(1)



        For Each padrao In regexer.Padroes
            padrao.ConstruirRelatorio()
            fatura.Relatorios.Add(padrao)
        Next

    End Sub

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

    Public Function ObterNumeroDePaginas() As Integer



        Try
            If (objAVDoc.Open(Filepath, "")) Then
                objPDDoc = objAVDoc.GetPDDoc
                Return objPDDoc.GetNumPages()
            Else
                Return -1
            End If
        Catch ex As Exception

            Stop
            'MatarProcessosdeAdobeATivos()


            If (objAVDoc.Open(Filepath, "")) Then
                objPDDoc = objAVDoc.GetPDDoc
                Return objPDDoc.GetNumPages()
            Else
                Return -1
            End If
        End Try

    End Function


End Class

