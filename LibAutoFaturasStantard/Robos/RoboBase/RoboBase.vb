Imports System.IO
Imports BibliotecaAutomacaoFaturas

Public MustInherit Class RoboBase
    Private TratadorPdf As TratadorDeFaturasPDF
    Public Operadora As OperadoraEnum
    Public TipoDeConta As TipoContaEnum

    Private arquivoPath As String
    Protected WithEvents LoginPage As ILoginPage
    Protected WithEvents ContaPage As IContaPage
    Protected ContaLogada As Conta
    Public Shared Event Log(Texto As String)


    Sub New(LoginPage As ILoginPage, ContaPage As IContaPage, tratadorpdf As TratadorDeFaturasPDF,
            Operadora As OperadoraEnum, Tipo As TipoContaEnum)
        Me.TratadorPdf = tratadorpdf
        Me.Operadora = Operadora
        Me.TipoDeConta = Tipo


        Me.LoginPage = LoginPage
        Me.ContaPage = ContaPage


    End Sub


    Sub run(ListaDeContas As List(Of Conta))

        For x = 0 To ListaDeContas.Count - 1

            Console.WriteLine("indice: " + x.ToString)

            Dim faturas = buscarFaturasPendentes(ListaDeContas(x))
            For index = 0 To faturas.Count - 1
Inicio:
                RaiseEvent Log($"Buscando fatura da conta {ListaDeContas(x).NrDaConta} com vencimento em {faturas(index).Vencimento.ToShortDateString} as {Now.ToShortTimeString} 
 empresa: {ListaDeContas(x).Empresa.Nome} cnpj: {ListaDeContas(x).Empresa.CNPJ} 
 fatura baixada: {faturas(index).Baixada} fatura tratada: {faturas(index).Tratada} fatura pendente: {faturas(index).Pendente}")
                Try

                    If GerenciarLogin(ListaDeContas(x)) Then
                        ContaPage.BuscarFatura(faturas(index))

                    End If


                Catch ex As ProdutoNaoCadastradoException
                    RaiseEvent Log($"Produto não cadastrado")
                    'AvancarAteProximoCnpjSeAplciavel(ListaDeContas, x)
                    Exit For

                Catch ex As ContaNaoCadasTradaException
                    RaiseEvent Log($"Conta Não Cadastrada Para este gestor")
                    'AvancarAteProximoGestorSeAplicavel(ListaDeContas, x)
                    Exit For

                Catch ex As LoginOuSenhaInvalidosException

                    RaiseEvent Log($"Login Ou Senha Inválidos")

                    'AvancarAteProximoGestorSeAplicavel(ListaDeContas, x)
                    Exit For

                Catch ex As ErroLoginExcpetion
                    RaiseEvent Log($"Erro de login")
                    Me.ContaLogada = Nothing
                    WebdriverCt.ResetarWebdriver()
                    Exit For

                Catch ex As FaturaNotDownloadedException
                    RaiseEvent Log($"Fatura não baixada")
                    Continue For

                Catch ex As PortalForaDoArException
                    RaiseEvent Log($"portal fora do ar")
                    Me.ContaLogada = Nothing
                    'WebdriverCt.ResetarWebdriver()
                    GoTo Inicio

                Catch ex As FaturaNaoDisponivelException
                    RaiseEvent Log($"fatura não disponível")
                    Continue For

                Catch ex As RoboFaturaException
                    RaiseEvent Log($"Outro erro de robofatura")
                    Continue For

#If Not DEBUG Then
                Catch ex As Exception
                RaiseEvent Log($"Outro erro de desconhecido {ex.message}")
                    Dim t As New RoboFaturaException(faturas(index), ex.Message + ex.StackTrace)
                    Me.ContaLogada = Nothing
                    Continue For
#End If

                End Try


            Next
        Next

    End Sub

    Private Sub AvancarAteProximoCnpjSeAplciavel(listaDeContas As List(Of Conta), x As Integer)

        If Me.Operadora = OperadoraEnum.VIVO And Me.TipoDeConta = TipoContaEnum.FIXA Then

            Dim cnpjAtual = listaDeContas(x).Empresa.CNPJ
            Do Until listaDeContas(x).Empresa.CNPJ <> cnpjAtual
                x += 1
            Loop

        End If

        Me.ContaLogada = Nothing

    End Sub

    Private Sub AvancarAteProximoGestorSeAplicavel(listaDeContas As List(Of Conta), x As Integer)
        If Me.Operadora = OperadoraEnum.VIVO Then

            Dim gestorAtual = listaDeContas(x).Gestores.First
            Do Until listaDeContas(x).Gestores.First.CPF <> gestorAtual.CPF
                x += 1
            Loop

        End If



    End Sub

    Private Function buscarFaturasPendentes(conta As Conta) As List(Of Fatura)

        Dim FaturasPendentes As List(Of Fatura) = conta.Faturas.Where(Function(x) x.Pendente = True _
                                                  Or x.Baixada = False).ToList


        Return FaturasPendentes

    End Function

    Protected MustOverride Function GerenciarLogin(conta As Conta) As Boolean

    Protected Overridable Sub ManejarFatura(fatura As Fatura) Handles ContaPage.FaturaBaixada

        If ConferirArquivoBaixadao(fatura) Then

            Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)
            fatura.Baixada = True
            arquivoPath = EncontrarPathUltimoArquivo()
            arquivoPath = RenomearFatura(fatura, arquivoPath)

            Dim infoDownload As New InfoDownload With {.path = arquivoPath, .tipoArquivo = ArquivoEnum.pdf,
            .nrConta = fatura.NrConta, .vencimento = fatura.Vencimento,
            .operadora = conta.Operadora, .tipoConta = conta.TipoDeConta}

            If Not fatura.InfoDownloads.Any(Function(i)
                                                Return i.nrConta = infoDownload.nrConta And
                                            i.vencimento = infoDownload.vencimento And
                                            i.tipoArquivo = infoDownload.tipoArquivo
                                            End Function) Then

                fatura.InfoDownloads.Add(infoDownload)
            End If

            GerRelDB.AtualizarContaComLogNaFatura(fatura, "Fatura baixada")
            arquivoPath = ""
        Else
            'Stop
            Throw New FalhaDownloadExcpetion(fatura, "O aruqivo baixado é diferente do solicitado")
        End If

    End Sub

    Protected Overridable Sub ManejarFaturaCsv(fatura As Fatura) Handles ContaPage.FaturaBaixadaCSV
        If ConferirArquivoBaixadao(fatura) Then

            Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)
            fatura.Baixada = True
            arquivoPath = EncontrarPathUltimoArquivo()
            arquivoPath = RenomearFatura(fatura, arquivoPath)

            Dim infoDownload As New InfoDownload With {.path = arquivoPath, .tipoArquivo = ArquivoEnum.csv,
            .nrConta = fatura.NrConta, .vencimento = fatura.Vencimento,
            .operadora = conta.Operadora, .tipoConta = conta.TipoDeConta}

            If Not fatura.InfoDownloads.Any(Function(i)
                                                Return i.nrConta = infoDownload.nrConta And
                                            i.vencimento = infoDownload.vencimento And
                                            i.tipoArquivo = infoDownload.tipoArquivo
                                            End Function) Then

                fatura.InfoDownloads.Add(infoDownload)
            End If

            GerRelDB.AtualizarContaComLogNaFatura(fatura, "Fatura CSV baixada")
            arquivoPath = ""
        Else
            Throw New FalhaDownloadExcpetion(fatura, "O aruqivo baixado é diferente do solicitado")
        End If

    End Sub



    Private Function ConferirArquivoBaixadao(fatura As Fatura) As Boolean

        arquivoPath = EncontrarPathUltimoArquivo()

        Dim conferencia = LerFaturaRetornandoNr_REF_DaFaturaParaConferencia(arquivoPath, fatura)

        Dim contaencontrada = conferencia.Item1
        contaencontrada = contaencontrada.Replace(".", "")

        If conferencia.Item2 <> "" Then


            Dim REF_encontrada = conferencia.Item2

            If fatura.Referencia = "" Then
                fatura.Referencia = REF_encontrada
            End If
        End If



        Return contaencontrada = fatura.NrConta

    End Function

    Protected Sub OnFaturaChecada(fatura As Fatura) Handles ContaPage.FaturaChecada

        GerRelDB.AtualizarContaComLogNaFatura(fatura, $"Fatura Checada {Now.ToShortTimeString}", True)

        RaiseEvent Log("Fatura checada")
    End Sub


    Protected Sub OnLoginRealizado(conta As Conta) Handles LoginPage.LoginRealizado ' fazer overrridable

        ContaLogada = conta

        RealizarLogNasContasCorrespondentes(conta)

        RaiseEvent Log($"Login com sucesso")

    End Sub

    Protected MustOverride Sub RealizarLogNasContasCorrespondentes(Conta As Conta)

    Public Shared Sub EnviarLog(texto As String)
        RaiseEvent Log(texto)
    End Sub

    Protected Function EncontrarPathUltimoArquivo() As String
        Dim Arquivopath As String = ""
        Dim ultimoArquivo As FileInfo

        'Do Until Arquivopath.EndsWith("pdf") Or Arquivopath.EndsWith("csv")

        '    Dim arquivos As String() = Directory.GetFiles(WebdriverCt._folderContas)

        '    ultimoArquivo = Nothing

        '    For Each arquivo As String In arquivos
        '        Dim arquivoAtual As New FileInfo(arquivo)
        '        If ultimoArquivo Is Nothing Then
        '            ultimoArquivo = arquivoAtual
        '        End If


        '        If ultimoArquivo.CreationTime < arquivoAtual.CreationTime Then
        '            ultimoArquivo = arquivoAtual
        '        End If
        '    Next


        '    Arquivopath = ultimoArquivo.FullName
        'Loop

        Return WebdriverCt._folderContas + "\UltimoArquivo.pdf"

    End Function



    Protected Function RenomearFatura(fatura As Fatura, ArquivoPath As String) As String

        Dim NomeArquivo = Path.GetFileNameWithoutExtension(ArquivoPath)
        Dim extensaodoarquivo = Path.GetExtension(ArquivoPath)
        Dim _referencia = fatura.Referencia

        Dim nomesArquivo As String() = ArquivoPath.Split("\")

        Dim Novonome = Replace(ArquivoPath, nomesArquivo.Last, fatura.NrConta + "_" + _referencia + extensaodoarquivo)



        Try
            Rename(ArquivoPath, Novonome)
            ArquivoPath = Novonome
        Catch ex As System.IO.IOException
            Dim x As New FileInfo(Novonome)
            x.Delete()
            Rename(ArquivoPath, Novonome)
            ArquivoPath = Novonome
        Catch ex As ArgumentException
            Utilidades.MatarProcessosdeAdobeATivos()
            Threading.Thread.Sleep(500)
            Dim x As New FileInfo(Novonome)
            x.Delete()
            Rename(ArquivoPath, Novonome)
            ArquivoPath = Novonome
        End Try




        Return ArquivoPath

    End Function

    Protected Function LerFaturaRetornandoNr_REF_DaFaturaParaConferencia(arquivoPath As String, FATURA As Fatura) As Tuple(Of String, String)

        Dim RegexerVazio As New Regexer ' Regezer vazio para poder construir o leitorpdf sem que ele faça regex
        Dim leitorPDf As New LeitorPDF(RegexerVazio)
        Return leitorPDf.VerificarNr_REF_DaFatura(arquivoPath, FATURA)
    End Function

    Protected Sub FazerLogNaConta(conta As Conta)
        'GerRelDB.AtualizarContaComLogNaFatura(conta, $"Logado corretamente ", True)
    End Sub


    Protected Sub FazerLogNaEmpresa(empresa As Empresa)
        'GerRelDB.AtualizarContaComLogNaFatura(empresa, $"Logado corretamente ", True)
    End Sub


    Protected Sub FazerLogNoGestor(gestor As Gestor)
        'GerRelDB.AtualizarContaComLogNaFatura(gestor, $"Logado corretamente ", True)
    End Sub


End Class

