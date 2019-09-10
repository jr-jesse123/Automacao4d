
Imports System.IO
    Imports System.Text.RegularExpressions
    Imports BibliotecaAutomacaoFaturas

Public MustInherit Class TratadorDeFAturasBase
    Protected running As Boolean = False
    Protected MustOverride Property extensaodoarquivo As String
    Protected ArquivoPath As String
    Protected conta As Conta
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
    ''' <param name="destinoPath">Caminho da pasta para gravar o arquivo, não é necessário informar o nome do arquivo</param>
    Protected Sub PosicionarFaturaNaPasta(Optional destinoPath As String = "")
        Dim x As New FileInfo(ArquivoPath)

        Dim Destino As String
        If destinoPath = "" Then
            Destino = conta.Pasta + "\" + Path.GetFileName(ArquivoPath)
        Else
            Destino = destinoPath + "\" + Path.GetFileName(ArquivoPath)
        End If



        Try
            x.CopyTo(Destino)
        Catch ex As System.IO.IOException
            Dim arquivoDestino As New FileInfo(Destino)
            arquivoDestino.Delete()
            x.CopyTo(Destino)
        End Try


    End Sub

    Protected MustOverride Sub ProcessarFaturaFox()

    Public Sub executar(fatura As Fatura)
        EcontrarContaDaFatura(fatura)
        ExtrairArquivoFaturaSeNecessario()
        NrFaturaDoArquivo = LerFaturaRetornandoNrDaFaturaParaConferencia(fatura)
        ConferirNumeroDeContaDoArquivo(fatura)
        PosicionarFaturaNaPasta()
        PosicionarFaturaNoDrive(fatura)
        ProcessarFaturaFox()
        AdicionarInformacoesFatura(fatura)
        DispararFluxoBitrix(fatura)

    End Sub

    Public Sub ConferirNumeroDeContaDoArquivo(fatura As Fatura)
        If Not NrFaturaDoArquivo.ToString = fatura.NrConta.ToString Then
            Throw New FalhaDownloadExcpetion(fatura, "A Faturabaixada era diferente da fatura solicitada")
        End If
    End Sub

    Protected Sub EcontrarContaDaFatura(fatura As Fatura)
        Me.conta = GerRelDB.Contas.Where(Function(x) x.Faturas.Contains(fatura)).First
    End Sub

    Protected MustOverride Sub ExtrairArquivoFaturaSeNecessario()

    Protected Sub SalvarAlteraçõesFatura()
        GerRelDB.UpsertConta(conta)
    End Sub

    Protected MustOverride Sub AdicionarInformacoesFatura(fatura As Fatura)


    Protected Sub Atualizar(conta As Conta)
        Me.conta = conta

        SalvarAlteraçõesFatura()

    End Sub

    Protected Sub DispararFluxoBitrix(fatura As Fatura)
        Dim IDBitrix = ApiBitrix.atualizaTriagem(
            conta.ContaTriagemBitrixID, _referencia, fatura.Total,
            fatura.Vencimento.ToString("dd/MM/yy"), fatura.Creditos, fatura.Encargos)

        If IDBitrix.Result > 0 Then
            fatura.Tratada = True
            GerRelDB.AtualizarContaComLogNaFatura(fatura, $"Cliente Enviado Ao Bitrix com id {IDBitrix} ")
        Else
            Throw New ErroDeAtualizacaoBitrix(fatura, "Falha Atualização Bitrix")
        End If

    End Sub

    Public MustOverride Function LerFaturaRetornandoNrDaFaturaParaConferencia(FATURA As Fatura) As String

    Protected Sub PosicionarFaturaNoDrive(fatura As Fatura)
#If Not DEBUG Then
        For Each arquivo In Arquivos
            If arquivo.Name = Path.GetFileName(ArquivoPath) Then
                DriveApi.DeleteFile(arquivo.Id)
            End If
        Next
#End If
        Dim NomeDoArquivo = Path.GetFileName(ArquivoPath)

        Dim id = DriveApi.Upload(NomeDoArquivo, conta.Drive, ArquivoPath)
        If id.Length > 0 Then
            GerRelDB.AtualizarContaComLogNaFatura(fatura, $"Fatura enviada para o Drive nome: {NomeDoArquivo} id: {id}")
        Else
            Throw New FalhaUploadNoDriveException(fatura, "Erro Ao salvar a fatura no Drive")
        End If

    End Sub


    Private Function ObterFaturasBaixadas() As List(Of Fatura)

        Dim ListaDeFAturasBaixadas As New List(Of Fatura)

        For Each conta In GerRelDB.Contas
            Dim faturasPendentes = conta.Faturas.Where(Function(f)
                                                           f.Baixada = True And
                                                               f.Tratada = False
                                                       End Function)
            ListaDeFAturasBaixadas.AddRange(faturasPendentes)
        Next

        Return ListaDeFAturasBaixadas

    End Function
End Class


