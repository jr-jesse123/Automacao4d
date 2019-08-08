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
    Private _vencimento As Date
    Private _referencia As String

    Sub New(DriveApi As GoogleDriveAPI, ConversorPDF As ConversorPDF, ApiBitrix As ApiBitrix)
        Me.ApiBitrix = ApiBitrix
        Me.ConversorPDF = ConversorPDF
        Me.DriveApi = DriveApi

    End Sub

    Public Sub RenomearFatura(fatura As Fatura)

        Dim NomeArquivo = Path.GetFileNameWithoutExtension(ArquivoPath)

        DefinidorDeReferenciaDeFaturas.DescobrirReferencia(fatura.Vencimento, conta.Operadora, conta.TipoDeConta)


        Dim nomesArquivo As String() = ArquivoPath.Split("\")

        Dim Novonome = Replace(ArquivoPath, nomesArquivo.Last, conta.NrDaConta + "_" + _referencia + ".pdf")

        Threading.Thread.Sleep(1000)

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
            ConverterPdfParaTxt(fatura)
            ProcessarTxt()
            AdicionarInformacoesFatura(fatura)
            DispararFluxoBitrix(fatura)




        End If



    End Sub

    Private Sub SalvarAlteraçõesFatura()
        GerRelDB.UpsertConta(conta)
    End Sub

    Private Sub AdicionarInformacoesFatura(fatura As Fatura)

        Dim valor = fatura.Relatorios.Where(Function(x) x.GetType = GetType(TotalMovelTim)).First.Relatorio.Rows(0).Item(0)
        Dim creditos = fatura.Relatorios.Where(Function(x) x.GetType = GetType(CreditosMovelTim)).FirstOrDefault


        If creditos.Matches.Count > 0 Then
            fatura.Creditos = creditos.Relatorio.Rows(0).Item(0)
        Else
            fatura.Creditos = 0
        End If

        fatura.Referencia = _referencia
        fatura.ValorOriginal = valor

        fatura.Encargos = 0

    End Sub

    Friend Sub Atualizar(conta As Conta)
        Me.conta = conta

        SalvarAlteraçõesFatura()

    End Sub

    Private Sub DispararFluxoBitrix(fatura As Fatura)
        Dim IDBitrix = ApiBitrix.atualizaTriagem(
            conta.ContaTriagemBitrixID, _referencia, fatura.ValorOriginal,
            _vencimento, fatura.Creditos, fatura.Encargos)

        If IDBitrix.Result > 0 Then
            fatura.Baixada = True
            GerRelDB.AtualizarContaComLog(fatura, $"Cliente Enviado Ao Bitrix com id {IDBitrix} ")
        Else
            Throw New ErroDeAtualizacaoBitrix(fatura, "Falha Atualização Bitrix")
        End If

    End Sub

    Private Sub ConverterPdfParaTxt(FATURA As Fatura)

        Dim x As New FileInfo(DestinoPath +
                              Path.GetFileName(ArquivoPath.Replace(".pdf", ".txt")))
        x.Delete()

        ConversorPDF.ConverterPdfParaTxt(ArquivoPath, DestinoPath + Path.GetFileName(ArquivoPath), FATURA)
    End Sub

    Private Sub PosicionarFaturaNoDrive(fatura As Fatura)

        Dim id = DriveApi.Upload(Path.GetFileName(ArquivoPath), conta.Drive, ArquivoPath)

        If id.Length > 0 Then
            GerRelDB.AtualizarContaComLog(fatura, $"Fatura enviada para o Drive {id}")
        Else
            Throw New FalhaUploadNoDriveException(fatura, "Erro Ao salvar a fatura no Drive")
        End If

    End Sub

    Private Sub EncontrarPathUltimoArquivo()
        Dim ultimoArquivo As FileInfo



        Do Until Path.GetExtension(ArquivoPath) = ".pdf"
            Dim arquivos As String() = Directory.GetFiles(WebdriverCt._folderContas)

            For Each arquivo As String In arquivos
                Dim arquivoAtual As New FileInfo(arquivo)
                If ultimoArquivo Is Nothing Then
                    ultimoArquivo = arquivoAtual
                ElseIf ultimoArquivo.CreationTime < arquivoAtual.CreationTime Then
                    ultimoArquivo = arquivoAtual
                End If
            Next


            ArquivoPath = ultimoArquivo.FullName
        Loop


    End Sub



End Class
