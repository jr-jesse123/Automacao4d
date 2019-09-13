﻿
Imports System.IO
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
    ''' <param name="destinoPath">Caminho da pasta para gravar o arquivo, não é necessário informar o nome do arquivo</param>
    Public Sub PosicionarFaturaNaPasta(fatura As Fatura, Optional destinoPath As String = "")

        Dim ArquivoPath = fatura.InfoDownloads.First.path
        Dim x As New FileInfo(ArquivoPath)
        Dim conta = GerRelDB.Contas.Where(Function(co) co.Faturas.Contains(fatura)).First


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

        fatura.FaturaPosicionadaNaPasta = True

        GerRelDB.AtualizarContaComLogNaFatura(fatura, "conta posicionada na pasta do servidor")

    End Sub

    Public MustOverride Sub ProcessarFaturaFox(fatura As Fatura)

    Public Sub DispararFluxoBitrix(fatura As Fatura)

        Dim conta = GerRelDB.Contas.Where(Function(x) x.Faturas.Contains(fatura)).First

        Dim IDBitrix = ApiBitrix.atualizaTriagem(
            conta.ContaTriagemBitrixID, _referencia, fatura.Total,
            fatura.Vencimento.ToString("dd/MM/yy"), fatura.Creditos, fatura.Encargos)

        If IDBitrix.Result > 0 Then
            fatura.Tratada = True
            GerRelDB.AtualizarContaComLogNaFatura(fatura, $"Cliente Enviado Ao Bitrix com id {IDBitrix} ")
        Else
            Throw New ErroDeAtualizacaoBitrix(fatura, "Falha Atualização Bitrix")
        End If

        fatura.FluxoDisparado = True

        GerRelDB.AtualizarContaComLogNaFatura(fatura, "$Fluxos disparados com as informações")

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
            GerRelDB.AtualizarContaComLogNaFatura(fatura, $"Fatura enviada para o Drive nome: {NomeDoArquivo} id: {id}")
        Else
            Throw New FalhaUploadNoDriveException(fatura, "Erro Ao salvar a fatura no Drive")
        End If

    End Sub

    Public MustOverride Sub ExtrairArquivoFaturaSeNecessario(fatura As Fatura)
    Public MustOverride Sub AdicionarInformacoesFatura(fatura As Fatura)

End Class


