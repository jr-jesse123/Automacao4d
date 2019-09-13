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


            Dim contasPosicionarNaPasta = listaFaturas.Where(Function(c) c.FaturaPosicionadaNaPasta = False)
            Dim ContasUparParaDriver = listaFaturas.Where(Function(c) c.FaturaEnviadaParaDrive = False)
            Dim contasFaturaConverterEExtrairRelatorios = listaFaturas.Where(Function(c) c.FaturaConvertida = False)
            Dim contasFluxoDispararar = listaFaturas.Where(Function(c) c.FluxoDisparado = False And c.FaturaConvertida = True)
            Dim contasProcessarFox = listaFaturas.Where(Function(c) c.FaturaProcessadaFox = False And c.FaturaConvertida = True)
            Dim RelatriosUparDrive = listaFaturas.Where(Function(c) c.RelatoriosUpadosDrive = False And c.FaturaProcessadaFox = True)
            Dim RelatoriosEnviarWebapp = listaFaturas.Where(Function(c) c.RelatoriosEnviadosWebapp = False And c.FaturaProcessadaFox = True)





            Dim app = scope.Resolve(Of TratadorDeFaturasPDF)

            For Each fatura In contasPosicionarNaPasta
                app.PosicionarFaturaNaPasta(fatura)
            Next


            For Each fatura In ContasUparParaDriver
                app.PosicionarFaturaNoDrive(fatura)
            Next

            For Each fatura In contasFaturaConverterEExtrairRelatorios
                app.ConverterPdfParaTxtEextrairRelatorios(fatura)
            Next

            For Each fatura In contasFluxoDispararar
                app.DispararFluxoBitrix(fatura)
            Next

            For Each fatura In contasProcessarFox
                app.ProcessarFaturaFox(fatura)
            Next



        End Using

    End Sub

End Module
