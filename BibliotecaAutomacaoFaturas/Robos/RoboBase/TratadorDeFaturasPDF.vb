Imports System.IO
Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas

Public Class TratadorDeFaturasPDF
    Inherits TratadorDeFAturasBase
    Private PathDeEntradaFox, PathDeSaidaFox, PathAtivadorFox As String
    Private ConversorPDF As ConversorPDF

    Public Sub New(DriveApi As GoogleDriveAPI, ConversorPDF As ConversorPDF, ApiBitrix As ApiBitrix)

        MyBase.New(DriveApi, ApiBitrix)
        Me.ConversorPDF = ConversorPDF

    End Sub

    Protected Overrides Property extensaodoarquivo As String = ".pdf"

    Protected Overrides Function LerFaturaRetornandoNrDaFaturaParaConferencia(FATURA As Fatura) As String

        Dim x As New FileInfo(DestinoPath +
                              Path.GetFileName(ArquivoPath.Replace(".pdf", ".txt")))
        x.Delete()

        Return ConversorPDF.ConverterPdfParaTxt(ArquivoPath, DestinoPath + Path.GetFileName(ArquivoPath), FATURA)
    End Function

    Protected Overrides Sub ExtrairArquivoFaturaSeNecessario()
        'esta clase não precisa fazer nada neste caso pois as faturas já vem prontas para consumo
    End Sub

    Protected Overrides Sub ProcessarFaturaFox()



        Dim x = PathsContainerFox.ObterPaths(conta.Operadora, conta.TipoDeConta)
        PathAtivadorFox = x.Ativador
        PathDeEntradaFox = x.PastaEntrada
        PathDeSaidaFox = x.PastaSaida

        PosicionarFaturaNaPasta(x.PastaEntrada)

        'info.UseShellExecute = False
        Dim info As ProcessStartInfo = New ProcessStartInfo(x.Ativador) With {
            .CreateNoWindow = True,
            .RedirectStandardError = True,
            .RedirectStandardOutput = True,
            .RedirectStandardInput = True
        } ' se não der certo adiciona "+.lnk"

        Dim ProcessoFox As Process = Process.Start(info)

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
        EncontrarPathUltimoArquivo()
        RenomearFatura(fatura)
        PosicionarFaturaNaPasta()
        PosicionarFaturaNoDrive(fatura)
    End Sub

End Class
