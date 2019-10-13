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

    Public Overrides Function ConverterPdfParaTxtEextrairRelatorios(FATURA As Fatura) As String

        Dim ArquivoPath = FATURA.InfoDownloads.First.path

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(FATURA)


            ConversorPDF.ConverterPdfParaTxt(ArquivoPath, ArquivoPath.Replace(".pdf", ".txt"), FATURA)

        'If FATURA.Total = 0 Then Stop
        AdicionarInformacoesFatura(FATURA)

        FATURA.FaturaConvertida = True
        GerRelDB.AtualizarContaComLogNaFatura(FATURA, "Fatura Convertida e regexes realizados")

    End Function

    Public Overrides Sub ExtrairArquivoFaturaSeNecessario(fatura As Fatura)
        'esta clase não precisa fazer nada neste caso pois as faturas já vem prontas para consumo
    End Sub

    Public Overrides Sub ProcessarFaturaFox(fatura As Fatura)

        Dim ArquivoPath = fatura.InfoDownloads.First.path.Replace(".pdf", ".txt")

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)

        '**************************************************
        'encerra o ciclo se a fatura não precisar ser tratada no fox
        'If conta.TipoDeConta <> TipoContaEnum.MOVEL Then
        '    fatura.FaturaProcessadaFox = True
        '    Exit Sub
        'End If

        '*******************************************************************
        Dim PastaEntradaFox = PathsContainerFox.ObterPaths(conta.Operadora, conta.TipoDeConta).PastaEntrada

        Dim arquivos = Directory.GetFiles(PastaEntradaFox)

        For Each arquvio In arquivos
            File.Delete(arquvio)
        Next

        File.Copy(ArquivoPath, PastaEntradaFox + "\" + Path.GetFileName(ArquivoPath))

        '*******************************************************************


        Dim apifox As New ApiClienteFoxProw
        Dim result = apifox.SolicitarProcessamento(conta.Operadora.ToString, conta.NrDaConta, fatura.Referencia)

        If result = "OK" Then
            EnviarRelatorioParaGoogleDrive(fatura)
            fatura.FaturaProcessadaFox = True
            GerRelDB.AtualizarContaComLogNaFatura(fatura, "Fatura processada no foxprow, 
arquivos enviados para webapp, relatório padrão enviado par ao drive")
        Else

            If result.Contains("FoxProw") Then
                ApiGmail.NotificarDoracy(fatura)
            End If

            Throw New ApiFoProwException(fatura, result)
        End If


    End Sub

    Private Sub EnviarRelatorioParaGoogleDrive(fatura As Fatura)

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)

        Dim arquivos = Directory.GetFiles($"\\192.168.244.112\4D_CONSULTORIA\AUTO\{conta.Operadora.ToString}_REL")

        Dim relatorio = arquivos.Where(Function(f) f.Contains("RELATÓRIO_MENSAL")).First


        Dim NomeDoArquivo = Path.GetFileName(relatorio)

        Dim id = DriveApi.Upload(NomeDoArquivo, conta.Drive, relatorio)
        If id.Length > 0 Then
            GerRelDB.AtualizarContaComLogNaFatura(fatura, $"Relatório Salvo No Drive: {NomeDoArquivo} id: {id}")
        Else
            Throw New FalhaUploadNoDriveException(fatura, "Erro Ao salvar a Relatório no Drive")
        End If


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


        PosicionarFaturaNaPasta(fatura)
        PosicionarFaturaNoDrive(fatura)
    End Sub


End Class
