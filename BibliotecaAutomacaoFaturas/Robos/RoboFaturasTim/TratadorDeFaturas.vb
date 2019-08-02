Imports System.IO
Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas

Public Class TratadorDeFaturas

    Private ArquivoPath As String
    Private DestinoPath As String = "C:\SISTEMA4D\TIM\"
    Private conta As Conta
    Public Property ApiBitrix As ApiBitrix
    Private ConversorPDF As ConversorPDF
    Private WithEvents DriveApi As GoogleDriveAPI
    Private _vencimento As String
    Private _referencia As String

    Sub New(DriveApi As GoogleDriveAPI, ConversorPDF As ConversorPDF, ApiBitrix As ApiBitrix)
        Me.ApiBitrix = ApiBitrix
        Me.ConversorPDF = ConversorPDF
        Me.DriveApi = DriveApi

    End Sub

    Public Sub RenomearFatura(fatura As Fatura)

        Dim NomeArquivo = Path.GetFileNameWithoutExtension(ArquivoPath)

        Dim dia As Integer = fatura.Vencimento.Day
        Dim mes As Integer = fatura.Vencimento.Month
        Dim ano As Integer = fatura.Vencimento.Year
        Dim referencia As String


        If dia > 17 Then
            referencia = mes.ToString("00")
        Else
            If mes < 12 Then
                referencia = (mes - 1).ToString("00")
            Else
                referencia = "01"
            End If
        End If

        Dim nomesArquivo As String() = ArquivoPath.Split("\")

        Dim Novonome = Replace(ArquivoPath, nomesArquivo.Last, conta.NrDaConta + "_" + referencia + (ano - 2000).ToString("00") + ".pdf")


        Try
            Rename(ArquivoPath, Novonome)
            ArquivoPath = Novonome
        Catch ex As System.IO.IOException
            Dim x As New FileInfo(Novonome)
            x.Delete()
            Rename(ArquivoPath, Novonome)
            ArquivoPath = Novonome
        End Try


        _vencimento = fatura.Vencimento.ToString("dd/MM/yy")

    End Sub

    Public Sub PosicionarFaturaNaPasta()
        Dim x As New FileInfo(ArquivoPath)
        Dim Destino As String

        If Debugger.IsAttached Then
            Destino = "C:\Users\User\source\repos\" + Path.GetFileName(ArquivoPath)
        Else
            Destino = conta.Pasta + "\" + Path.GetFileName(ArquivoPath)
        End If

        Try
            x.CopyTo(Destino)
        Catch ex As System.IO.IOException
            Dim arquivoDestino As New FileInfo(Destino)
            arquivoDestino.Delete()
            x.CopyTo(Destino)
        End Try


    End Sub

    Public Sub ProcessarTxt()

    End Sub

    Friend Sub executar(fatura As Fatura)
        Me.conta = GerRelDB.Contas.Where(Function(x) x.Faturas.Contains(fatura)).First
        If fatura.Baixada = False Then
            EncontrarPathUltimoArquivo()
            RenomearFatura(fatura)
            PosicionarFaturaNaPasta()
            PosicionarFaturaNoDrive(fatura)
            ConverterPdfParaTxt()
            ProcessarTxt()
            AdicionarInformacoesFatura(fatura)
            If DispararFluxoBitrix(fatura).Result Then
                fatura.Baixada = True
            End If


        End If

        SalvarAlteraçõesFatura()

    End Sub

    Private Sub SalvarAlteraçõesFatura()
        GerRelDB.UpsertConta(conta)
    End Sub

    Private Sub AdicionarInformacoesFatura(fatura As Fatura)

        Dim valor = conta.padroesregex.Relatorios.Item(OperadoraEnum.TIM + TipoContaEnum.MOVEL) _
            .Where(Function(x) x.GetType = GetType(TotalMovelTim)).First.Relatorio.Rows(0).Item(0)

        Dim creditos = conta.padroesregex.Relatorios.Item(OperadoraEnum.TIM + TipoContaEnum.MOVEL) _
            .Where(Function(x) x.GetType = GetType(CreditosMovelTim)).First.Relatorio.Compute("SUM(Creditos)", "")


        If creditos.GetType <> GetType(DBNull) Then
            fatura.Creditos = creditos
        Else
            fatura.Creditos = 0
        End If

        fatura.Referencia = _referencia
        fatura.ValorOriginal = valor

        fatura.Encargos = 0

    End Sub

    Friend Sub Atualizar(fatura As Fatura)

        SalvarAlteraçõesFatura()

    End Sub

    Private Async Function DispararFluxoBitrix(fatura As Fatura) As Task(Of String)
        Await ApiBitrix.atualizaTriagem(conta.ContaTriagemBitrixID, _referencia, fatura.Creditos, fatura.Encargos)
    End Function

    Private Sub ConverterPdfParaTxt()

        Dim x As New FileInfo(DestinoPath +
                              Path.GetFileName(ArquivoPath.Replace(".pdf", ".txt")))
        x.Delete()

        ConversorPDF.ConverterPdfParaTxt(ArquivoPath, DestinoPath + Path.GetFileName(ArquivoPath), conta)
    End Sub

    Private Sub PosicionarFaturaNoDrive(fatura As Fatura)

        Dim id = DriveApi.Upload(Path.GetFileName(ArquivoPath), conta.Drive, ArquivoPath)

        If id.Length > 0 Then
            GerRelDB.EnviarLogFatura(fatura, $"Fatura enviada para o Drive {id}")
        Else
            Throw New FalhaUploadNoDriveException(fatura, "Erro Ao salvar a fatura no Drive")
        End If

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



End Class
