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

                If Not fatura.InfoDownloads.First.path.Contains("\Danilo") Then
                    Try
                        Console.WriteLine(fatura.NrConta)
                        app.PosicionarFaturaNaPasta(fatura)
                    Catch ex As PastaNaoEncontradaException


                    End Try


                End If


            Next
            Console.WriteLine("Posicionar para upar no driver")
            Dim ContasUparParaDriver = listaFaturas.Where(Function(c) c.FaturaEnviadaParaDrive = False).ToList
            For Each fatura In ContasUparParaDriver

                If Not fatura.InfoDownloads.First.path.Contains("\Danilo") Then
                    Console.WriteLine(fatura.NrConta)
                    app.PosicionarFaturaNoDrive(fatura)
                End If

            Next

            Console.WriteLine("converter e extrair relatorios")
            Dim contasFaturaConverterEExtrairRelatorios = listaFaturas.Where(Function(c) c.FaturaConvertida = False).ToList
            For Each fatura In contasFaturaConverterEExtrairRelatorios

                If Not fatura.InfoDownloads.First.path.Contains("\Danilo") Then
                    Console.WriteLine(fatura.NrConta)
                    app.ConverterPdfParaTxtEextrairRelatorios(fatura)
                End If

            Next

            Console.WriteLine("disparando fluxos")
            Dim contasFluxoDispararar = listaFaturas.Where(Function(c) c.FluxoDisparado = False And c.FaturaConvertida = True).ToList
            For Each fatura In contasFluxoDispararar

                If Not fatura.InfoDownloads.First.path.Contains("\Danilo") Then
                    Console.WriteLine(fatura.NrConta)
                    app.DispararFluxoBitrix(fatura)
                End If

            Next


            Console.WriteLine("processar no fox")
            Dim contasProcessarFox = listaFaturas.Where(Function(c) c.FaturaProcessadaFox = False And c.FaturaConvertida = True).ToList
            For Each fatura In contasProcessarFox

                If Not fatura.InfoDownloads.First.path.Contains("\Danilo") Then
                    Try
                        Console.WriteLine(fatura.NrConta)
                        app.ProcessarFaturaFox(fatura)
                    Catch ex As RoboFaturaException

                    End Try

                End If

            Next



        End Using

    End Sub

End Module
