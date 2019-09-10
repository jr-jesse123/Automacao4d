Imports Autofac
Imports BibliotecaAutomacaoFaturas

Module Module1

    Sub Main()
        Dim container As IContainer = ContainerConfig.Configure


        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of TratadorDeFaturasPDF)

            For Each conta In GerRelDB.Contas

                Dim fatuas = conta.Faturas.Where(Function(f) f.Baixada = True And f.Tratada = False).
                Where(Function(fb) fb.InfoDownloads.Any(Function(_fb) _fb.tipoArquivo = ArquivoEnum.pdf))

                For Each fatura In fatuas

                    app.executar(fatura)

                Next
            Next

        End Using

    End Sub

End Module
