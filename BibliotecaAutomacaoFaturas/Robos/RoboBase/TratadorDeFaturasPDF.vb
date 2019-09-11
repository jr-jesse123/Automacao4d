Imports System.IO
Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas

Public Class TratadorDeFaturasPDF
    Inherits TratadorDeFAturasBase

    Private ConversorPDF As LeitorPDF

    Public Sub New(DriveApi As GoogleDriveAPI, ConversorPDF As LeitorPDF, ApiBitrix As ApiBitrix)

        MyBase.New(DriveApi, ApiBitrix)
        Me.ConversorPDF = ConversorPDF


    End Sub

    Protected Overrides Property extensaodoarquivo As String = ".pdf"

    Public Overrides Function LerFaturaRetornandoNrDaFaturaParaConferencia(FATURA As Fatura) As String

        ArquivoPath = FATURA.InfoDownloads.First.path

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(FATURA)

        Dim PastaEntradaFox = PathsContainerFox.ObterPaths(conta.Operadora, conta.TipoDeConta).PastaEntrada

        Dim x As New FileInfo(PastaEntradaFox +
                              Path.GetFileName(ArquivoPath.Replace(".pdf", ".txt")))
        x.Delete()

        ConversorPDF.ConverterPdfParaTxt(ArquivoPath, PastaEntradaFox + "\" + Path.GetFileName(ArquivoPath), FATURA)
    End Function

    Protected Overrides Sub ExtrairArquivoFaturaSeNecessario()
        'esta clase não precisa fazer nada neste caso pois as faturas já vem prontas para consumo
    End Sub

    Protected Overrides Sub ProcessarFaturaFox(fatura As Fatura)



    End Sub

    Protected Overrides Sub AdicionarInformacoesFatura(fatura As Fatura)

        For Each relatorio In fatura.Relatorios
            Dim nome = relatorio.GetType.Name
            Dim propriedades = fatura.GetType.GetProperties
            For Each propriedade In propriedades
                If nome.StartsWith(propriedade.Name) Then
                    If relatorio.Iniciado Then
                        propriedade.SetValue(fatura, relatorio.Resultado)
                    Else
                        propriedade.SetValue(fatura, 0)
                    End If
                End If
            Next
        Next

    End Sub

    Public Sub TratamentoBasicoDeFAtura(fatura As Fatura)

        EcontrarContaDaFatura(fatura)
        PosicionarFaturaNaPasta()
        PosicionarFaturaNoDrive(fatura)
    End Sub

End Class
