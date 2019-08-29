﻿
Imports System.IO
    Imports System.Text.RegularExpressions
    Imports BibliotecaAutomacaoFaturas

Public MustInherit Class TratadorDeFAturasBase

    Protected MustOverride Property extensaodoarquivo As String
    Protected ArquivoPath As String
    Protected conta As Conta
    Public Property ApiBitrix As ApiBitrix
    Protected WithEvents DriveApi As GoogleDriveAPI
    Protected _vencimento As Date
    Protected _referencia As String
    Private _Arquivos As List(Of Google.Apis.Drive.v3.Data.File)
    Private NrFaturaDoArquivo As String

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

    Protected Sub RenomearFatura(fatura As Fatura)

        Dim NomeArquivo = Path.GetFileNameWithoutExtension(ArquivoPath)

        _referencia = fatura.Referencia


        Dim nomesArquivo As String() = ArquivoPath.Split("\")

        Dim Novonome = Replace(ArquivoPath, nomesArquivo.Last, conta.NrDaConta + "_" + _referencia + extensaodoarquivo)

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
        EncontrarPathUltimoArquivo()
        ExtrairArquivoFaturaSeNecessario()
        RenomearFatura(fatura)
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
            _vencimento, fatura.Creditos, fatura.Encargos)

        If IDBitrix.Result > 0 Then
            fatura.Baixada = True
            GerRelDB.AtualizarContaComLogNaFatura(fatura, $"Cliente Enviado Ao Bitrix com id {IDBitrix} ")
        Else
            Throw New ErroDeAtualizacaoBitrix(fatura, "Falha Atualização Bitrix")
        End If

    End Sub

    Protected MustOverride Function LerFaturaRetornandoNrDaFaturaParaConferencia(FATURA As Fatura) As String

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

    Protected Sub EncontrarPathUltimoArquivo()
        Dim ultimoArquivo As FileInfo

        Dim ArquivoPathAnterior = ArquivoPath

        Do Until ArquivoPath <> ArquivoPathAnterior

            Dim arquivos As String() = Directory.GetFiles(WebdriverCt._folderContas) _
            .Where(Function(p) Path.GetExtension(p) = extensaodoarquivo).ToArray

            ultimoArquivo = Nothing

            For Each arquivo As String In arquivos
                Dim arquivoAtual As New FileInfo(arquivo)
                If ultimoArquivo Is Nothing Then
                    ultimoArquivo = arquivoAtual
                End If
                If Not ultimoArquivo.Name.EndsWith(extensaodoarquivo) Then
                    ultimoArquivo = arquivoAtual
                End If

                If ultimoArquivo.CreationTime < arquivoAtual.CreationTime Then
                    ultimoArquivo = arquivoAtual
                End If
            Next


            ArquivoPath = ultimoArquivo.FullName
        Loop


    End Sub



End Class


