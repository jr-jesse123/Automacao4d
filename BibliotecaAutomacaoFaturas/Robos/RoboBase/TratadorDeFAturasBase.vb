
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

    Sub New(DriveApi As GoogleDriveAPI, ApiBitrix As ApiBitrix)
        Me.ApiBitrix = ApiBitrix

        Me.DriveApi = DriveApi

    End Sub

    Protected Sub RenomearFatura(fatura As Fatura)

        Dim NomeArquivo = Path.GetFileNameWithoutExtension(ArquivoPath)

        _referencia = fatura.Referencia


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

    Protected Sub PosicionarFaturaNaPasta()
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

    Protected MustOverride Sub ProcessarFatura()

    Public Sub executar(fatura As Fatura)
        Me.conta = GerRelDB.Contas.Where(Function(x) x.Faturas.Contains(fatura)).First
        If fatura.Baixada = False Then
            EncontrarPathUltimoArquivo()
            ExtrairFaturaSeNecessario()
            RenomearFatura(fatura)
            PosicionarFaturaNaPasta()
            PosicionarFaturaNoDrive(fatura)
            ExtrairInformacoesDaFatura(fatura)
            ProcessarFatura()
            AdicionarInformacoesFatura(fatura)
            DispararFluxoBitrix(fatura)




        End If



    End Sub

    Protected MustOverride Sub ExtrairFaturaSeNecessario()

    Protected Sub SalvarAlteraçõesFatura()
        GerRelDB.UpsertConta(conta)
    End Sub

    Protected Sub AdicionarInformacoesFatura(fatura As Fatura)



        For Each relatorio In fatura.Relatorios

            ' continuar aqui fazendo reflection para casar a propriedade com o padrao, ver se o nome da propriedade Começa Com.
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

        Dim id = DriveApi.Upload(Path.GetFileName(ArquivoPath), conta.Drive, ArquivoPath)

        If id.Length > 0 Then
            GerRelDB.AtualizarContaComLog(fatura, $"Fatura enviada para o Drive {id}")
        Else
            Throw New FalhaUploadNoDriveException(fatura, "Erro Ao salvar a fatura no Drive")
        End If

    End Sub

    Protected Sub EncontrarPathUltimoArquivo()
        Dim ultimoArquivo As FileInfo

        Dim ArquivoPathAnterior = ArquivoPath

        Do Until Path.GetExtension(ArquivoPath) = extensaodoarquivo _
            And ArquivoPath <> ArquivoPathAnterior

            Dim arquivos As String() = Directory.GetFiles(WebdriverCt._folderContas)
            ultimoArquivo = Nothing

            For Each arquivo As String In arquivos
                Dim arquivoAtual As New FileInfo(arquivo)
                If ultimoArquivo Is Nothing Then
                    ultimoArquivo = arquivoAtual
                End If
                If Not ultimoArquivo.Name.EndsWith(".pdf") Then
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


