Imports Autofac
Imports BibliotecaAutomacaoFaturas

Module Module1

    Sub Main()

        Dim container As IContainer = ContainerConfig.Configure

        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasTIM)
            app.run()

        End Using
    End Sub

End Module
