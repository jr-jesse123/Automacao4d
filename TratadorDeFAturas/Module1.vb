Imports Autofac
Imports BibliotecaAutomacaoFaturas

Module Module1

    Sub Main()
        Dim container As IContainer = ContainerConfig.Configure

        Dim listaFaturas As New List(Of Fatura)

        Using scope = container.BeginLifetimeScope


            For Each conta In GerRelDB.Contas
                Dim faturas = conta.Faturas.Where(Function(f) f.Baixada = True And f.Tratada = False).
                Where(Function(fb) fb.InfoDownloads.Any(Function(_fb) _fb.tipoArquivo = ArquivoEnum.pdf)).ToList
                listaFaturas.AddRange(faturas)
            Next










            Dim app = scope.Resolve(Of TratadorDeFaturasPDF)
            Dim contasPosicionarNaPasta = listaFaturas.Where(Function(c) c.FaturaPosicionadaNaPasta = False).ToList
            For Each fatura In contasPosicionarNaPasta

                If Not fatura.InfoDownloads.First.path.Contains("\Danilo") Then
                    Try
                        app.PosicionarFaturaNaPasta(fatura)
                    Catch ex As PastaNaoEncontradaException


                    End Try


                End If


            Next

            Dim ContasUparParaDriver = listaFaturas.Where(Function(c) c.FaturaEnviadaParaDrive = False).ToList
            For Each fatura In ContasUparParaDriver

                If Not fatura.InfoDownloads.First.path.Contains("\Danilo") Then
                    app.PosicionarFaturaNoDrive(fatura)
                End If

            Next

            Dim contasFaturaConverterEExtrairRelatorios = listaFaturas.Where(Function(c) c.FaturaConvertida = False).ToList
            For Each fatura In contasFaturaConverterEExtrairRelatorios

                If Not fatura.InfoDownloads.First.path.Contains("\Danilo") Then
                    app.ConverterPdfParaTxtEextrairRelatorios(fatura)
                End If

            Next


            Dim contasFluxoDispararar = listaFaturas.Where(Function(c) c.FluxoDisparado = False And c.FaturaConvertida = True).ToList
            For Each fatura In contasFluxoDispararar

                If Not fatura.InfoDownloads.First.path.Contains("\Danilo") Then
                    app.DispararFluxoBitrix(fatura)
                End If

            Next

            Dim contasProcessarFox = listaFaturas.Where(Function(c) c.FaturaProcessadaFox = False And c.FaturaConvertida = True).ToList
            For Each fatura In contasProcessarFox

                If Not fatura.InfoDownloads.First.path.Contains("\Danilo") Then
                    Try
                        app.ProcessarFaturaFox(fatura)
                    Catch ex As RoboFaturaException

                    End Try

                End If

            Next



        End Using

    End Sub

End Module
