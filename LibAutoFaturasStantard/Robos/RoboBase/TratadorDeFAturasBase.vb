
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas

Public MustInherit Class TratadorDeFAturasBase
    Protected running As Boolean = False
    Protected MustOverride Property extensaodoarquivo As String
    'Protected ArquivoPath As String
    Public Property ApiBitrix As ApiBitrix
    Protected WithEvents DriveApi As GoogleDriveAPI
    Protected _vencimento As Date
    Protected _referencia As String
    Private _Arquivos As List(Of Google.Apis.Drive.v3.Data.File)
    Private NrFaturaDoArquivo As String
    Public Event FaturaDisponivel()

    Protected ReadOnly Property Arquivos As List(Of Google.Apis.Drive.v3.Data.File)
        Get
            If _Arquivos Is Nothing Then
                _Arquivos = DriveApi.GetFiles
            End If
            Return _Arquivos
        End Get
    End Property

    Sub New(DriveApi As GoogleDriveAPI, ApiBitrix As ApiBitrix)
        Me.ApiBitrix = ApiBitrix

        Me.DriveApi = DriveApi

    End Sub


    ''' <summary>
    ''' Função para copiar o arquivo para outras pastas, sem encessidade de informar o nome do arquivo
    ''' </summary>

    Public Sub PosicionarFaturaNaPasta(fatura As Fatura)

        Dim ArquivoPath = fatura.InfoDownloads.First.path
        Dim x As New FileInfo(ArquivoPath)
        Dim conta = GerRelDB.Contas.Where(Function(co) co.Faturas.Contains(fatura)).First

        Dim Destino = conta.Pasta + "\" + Path.GetFileName(ArquivoPath)

        Try
            x.CopyTo(Destino.ToLower.Replace("servidor", "192.168.244.112"))

        Catch ex As DirectoryNotFoundException
            Throw New PastaNaoEncontradaException(fatura, "a pasta informada para este cliente não existe.")


        Catch ex As IOException
            Dim arquivoDestino As New FileInfo(Destino.ToLower.Replace("servidor", "192.168.244.112"))
            arquivoDestino.Delete()
            x.CopyTo(Destino.ToLower.Replace("servidor", "192.168.244.112"))
        End Try

        fatura.FaturaPosicionadaNaPasta = True

        GerRelDB.AtualizarContaComLogNaFatura(fatura, "conta posicionada na pasta do servidor")

    End Sub

    Public MustOverride Sub ProcessarFaturaFox(fatura As Fatura)

    Public Sub DispararFluxoBitrix(fatura As Fatura)

        Dim conta = GerRelDB.Contas.Where(Function(x) x.Faturas.Contains(fatura)).First

        Dim IDBitrix = ApiBitrix.atualizaTriagem(
            conta.ContaTriagemBitrixID, fatura.Referencia, fatura.Total,
            fatura.Vencimento.ToString("dd/MM/yy"), fatura.Creditos, fatura.Encargos)

        If IDBitrix.Result > 0 Then
            fatura.FluxoDisparado = True
            GerRelDB.AtualizarContaComLogNaFatura(fatura, $"Cliente Enviado Ao Bitrix com id {IDBitrix}, total: {fatura.Total}, vencimetno: {fatura.Vencimento.ToShortDateString} ")
        Else
            Throw New ErroDeAtualizacaoBitrix(fatura, "Falha Atualização Bitrix")
        End If



    End Sub

    Public MustOverride Function ConverterPdfParaTxtEextrairRelatorios(FATURA As Fatura) As String

    Public Sub PosicionarFaturaNoDrive(fatura As Fatura)
        Dim conta = GerRelDB.Contas.Where(Function(x) x.Faturas.Contains(fatura)).First
        Dim ArquivoPath = fatura.InfoDownloads.First.path

        For Each arquivo In Arquivos
            If arquivo.Name = Path.GetFileName(ArquivoPath) Then

                DriveApi.DeleteFile(arquivo.Id)



            End If
        Next

        Dim NomeDoArquivo = Path.GetFileName(ArquivoPath)

        Dim id = DriveApi.Upload(NomeDoArquivo, conta.Drive, ArquivoPath)
        If id.Length > 0 Then
            fatura.FaturaEnviadaParaDrive = True
            GerRelDB.AtualizarContaComLogNaFatura(fatura, $"Fatura enviada para o Drive nome: {NomeDoArquivo} id: {id}")
        Else
            Throw New FalhaUploadNoDriveException(fatura, "Erro Ao salvar a fatura no Drive")
        End If

    End Sub

    Public MustOverride Sub ExtrairArquivoFaturaSeNecessario(fatura As Fatura)
    Protected MustOverride Sub AdicionarInformacoesFatura(fatura As Fatura)

End Class

