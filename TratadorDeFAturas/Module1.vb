Imports Autofac
Imports BibliotecaAutomacaoFaturas

Module Module1

    Sub Main()
        Dim container As IContainer = ContainerConfig.Configure



        Dim listaFaturas As New List(Of Fatura)

        Using scope = container.BeginLifetimeScope
            Console.WriteLine("Buscando faturas")

            For Each conta In GerRelDB.Contas
                Dim faturas = conta.Faturas.Where(Function(f) f.Baixada = True And f.Tratada = False).
                Where(Function(fb) fb.InfoDownloads.Any(Function(_fb) _fb.tipoArquivo = ArquivoEnum.pdf)).ToList
                listaFaturas.AddRange(faturas)
            Next


            Console.WriteLine("Posicionar Faturas na pasta")
            Dim app = scope.Resolve(Of TratadorDeFaturasPDF)
            Dim contasPosicionarNaPasta = listaFaturas.Where(Function(c) c.FaturaPosicionadaNaPasta = False).ToList
            For Each fatura In contasPosicionarNaPasta


                Try
                    Console.WriteLine(fatura.NrConta)
                    app.PosicionarFaturaNaPasta(fatura)
                Catch ex As PastaNaoEncontradaException


                End Try





            Next
            Console.WriteLine("Posicionar para upar no driver")
            Dim ContasUparParaDriver = listaFaturas.Where(Function(c) c.FaturaEnviadaParaDrive = False).ToList
            For Each fatura In ContasUparParaDriver

                Console.WriteLine(fatura.NrConta)
                Try
                    app.PosicionarFaturaNoDrive(fatura)
                Catch ex As FalhaUploadNoDriveException

                End Try


            Next

            Console.WriteLine("converter e extrair relatorios")
            Dim contasFaturaConverterEExtrairRelatorios = listaFaturas.Where(Function(c) c.FaturaConvertida = False).ToList
            For Each fatura In contasFaturaConverterEExtrairRelatorios

                Console.WriteLine(fatura.NrConta)

                app.ConverterPdfParaTxtEextrairRelatorios(fatura)





            Next

            Console.WriteLine("disparando fluxos")
            Dim contasFluxoDispararar = listaFaturas.Where(Function(c) c.FluxoDisparado = False And c.FaturaConvertida = True).ToList
            For Each fatura In contasFluxoDispararar

                Console.WriteLine(fatura.NrConta)
                Try
                    app.DispararFluxoBitrix(fatura)
                Catch ex As ErroDeAtualizacaoBitrix

                End Try


            Next


            Console.WriteLine("processar no fox")
            Dim contasProcessarFox = listaFaturas.Where(Function(c) c.FaturaProcessadaFox = False And c.FaturaConvertida = True).ToList
            For Each fatura In contasProcessarFox

                Try
                    Console.WriteLine(fatura.NrConta)
                    app.ProcessarFaturaFox(fatura)
                Catch ex As RoboFaturaException

                End Try

            Next

            Dim faturasTratadas = listaFaturas.Where(Function(c) c.Baixada = True And c.Pendente = False And c.FaturaConvertida = True And
                                                         c.FluxoDisparado = True And c.FaturaProcessadaFox = True And c.FaturaEnviadaParaDrive = True And
                                                         c.FaturaPosicionadaNaPasta = True)


            Dim x As New MongoDb("AUTOMACAO4D")
            For Each fatura In faturasTratadas
                fatura.Tratada = True
                Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)
                x.UpsertRecord(conta)
            Next

        End Using


End Module
