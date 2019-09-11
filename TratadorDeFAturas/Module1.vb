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
            For Each fatura In listaFaturas

                app.executar(fatura)

            Next


        End Using

    End Sub

End Module
