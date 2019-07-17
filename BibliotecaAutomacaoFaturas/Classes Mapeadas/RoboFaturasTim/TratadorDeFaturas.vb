Imports System.IO
Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas

Public Class TratadorDeFaturas

    Private ArquivoPath As String
    Private DestinoPath As String = "C:\SISTEMA4D\TIM"
    Private conta As Conta
    Private WithEvents ConversorPDF As ConversorPDF
    Private Property regexerTim As RegexerTIM

    Sub New(RegexerTIM As RegexerTIM)
        Me.regexerTim = RegexerTIM
    End Sub

    Public Sub RenomearFatura()
        Dim NomeArquivo = Path.GetFileNameWithoutExtension(ArquivoPath)
        Dim novoNome As String = ArquivoPath.Replace(NomeArquivo, conta.NrDaConta.ToString)

        Rename(ArquivoPath, novoNome)
        ArquivoPath = novoNome

    End Sub

    Public Sub PosicionarFaturaNaPastaENoDrive()
        Dim x As New FileInfo(ArquivoPath)
        x.CopyTo(conta.Pasta + Path.GetFileName(conta.Pasta))
        x.CopyTo(conta.Pasta + Path.GetFileName(conta.Drive))
    End Sub

    Public Sub ConverterPdfParaTxt()

        Dim paginas As Integer = ConversorPDF.ObterNumeroDePaginas
        Dim TextPagina As String

        ConversorPDF = New ConversorPDF(ArquivoPath)

        For index = 0 To paginas
            TextPagina = ConversorPDF.ConverterPagina()
            AdicionarPaginaTxt(TextPagina)
        Next

    End Sub

    Public Sub ProcessarTxt()

    End Sub

    Friend Sub executar(conta As Conta)
        Me.conta = conta
        EncontrarPathUltimoArquivo()
        RenomearFatura()
        PosicionarFaturaNaPastaENoDrive()
        ConverterPdfParaTxt()
        ProcessarTxt()


    End Sub

    Private Sub EncontrarPathUltimoArquivo()

        Dim arquivos As String() = Directory.GetFiles(WebdriverCt._folderContas)

        Dim ultimoArquivo As FileInfo
        For Each arquivo As String In arquivos
            Dim arquivoAtual As New FileInfo(arquivo)
            If ultimoArquivo Is Nothing Then
                ultimoArquivo = arquivoAtual
            ElseIf ultimoArquivo.CreationTime < arquivoAtual.CreationTime Then
                ultimoArquivo = arquivoAtual
            End If
        Next
        ArquivoPath = ultimoArquivo.FullName

    End Sub


    Public Sub AdicionarPaginaTxt(strText)
        Using x As New StreamWriter(DestinoPath, True)
            x.Write(strText)
        End Using

    End Sub

End Class


Public Class Regexer
    Public informacoes2 As List(Of PesquisaRegexTIM)


    Private Function PesquisarTesto(padraoRegex As String) As Match()

        Dim padraoRegex As String = ""
        Dim verifica As New Regex(padraoRegex)
        Dim nrResultados = verifica.Matches(padraoRegex).Count
        Dim resultados(verifica.Matches(padraoRegex).Count - 1) As Match
        verifica.Matches(padraoRegex).CopyTo(resultados, 0)
        Return resultados

    End Function



End Class

Public Class PesquisaRegexTIM
    Public Padrao As String
    Public Resultados As List(Of Match)
    Public Concluido As Boolean
End Class
