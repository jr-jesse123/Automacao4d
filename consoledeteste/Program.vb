Imports System
Imports Autofac
Imports BibliotecaAutomacaoFaturas

Module Program
    Sub Main(args As String())

        Dim container As IContainer = ContainerConfig.Configure

        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasTIM)
            app.run()

        End Using

    End Sub

End Module
