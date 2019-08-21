
Imports System.IO
    Imports System.Text.RegularExpressions
    Imports BibliotecaAutomacaoFaturas

Public MustInherit Class TratadorDeFAturasBase


    Protected MustOverride Property extensaodoarquivo As String
    Protected ArquivoPath As String
    Protected DestinoPath As String = "C:\SISTEMA4D\TIM\"
    Protected conta As Conta
    Public Property ApiBitrix As ApiBitrix
    Protected WithEvents DriveApi As GoogleDriveAPI
    Protected _vencimento As Date
    Protected _referencia As String
    Private _Arquivos As List(Of Google.Apis.Drive.v3.Data.File)

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

    Protected Sub PosicionarFaturaNaPasta()
        Dim x As New FileInfo(ArquivoPath)
        Dim Destino As String = conta.Pasta + "\" + Path.GetFileName(ArquivoPath)

        Try
            x.CopyTo(Destino)
        Catch ex As System.IO.IOException
            Dim arquivoDestino As New FileInfo(Destino)
            arquivoDestino.Delete()
            x.CopyTo(Destino)
        End Try


    End Sub

    Protected MustOverride Sub ProcessarFatura()

    Public Sub executar(fatura As Fatura)
        EcontrarContaDaFatura(fatura)

        EncontrarPathUltimoArquivo()
            ExtrairFaturaSeNecessario()
            RenomearFatura(fatura)
            PosicionarFaturaNaPasta()
            PosicionarFaturaNoDrive(fatura)
            ExtrairInformacoesDaFatura(fatura)
            ProcessarFatura()
            AdicionarInformacoesFatura(fatura)
            DispararFluxoBitrix(fatura)

    End Sub

    Protected Sub EcontrarContaDaFatura(fatura As Fatura)
        Me.conta = GerRelDB.Contas.Where(Function(x) x.Faturas.Contains(fatura)).First
    End Sub

    Protected MustOverride Sub ExtrairFaturaSeNecessario()

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
            GerRelDB.AtualizarContaComLog(fatura, $"Cliente Enviado Ao Bitrix com id {IDBitrix} ")
        Else
            Throw New ErroDeAtualizacaoBitrix(fatura, "Falha Atualização Bitrix")
        End If

    End Sub

    Protected MustOverride Sub ExtrairInformacoesDaFatura(FATURA As Fatura)

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
            GerRelDB.AtualizarContaComLog(fatura, $"Fatura enviada para o Drive nome: {NomeDoArquivo} id: {id}")
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


