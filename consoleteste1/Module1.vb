Imports Autofac
Imports BibliotecaAutomacaoFaturas

Module Module1

    Sub Main()


        MatarProcessosdeAdobeATivos()
        Dim container As IContainer = ContainerConfig.Configure


        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasClaro)
            app.run()

        End Using


        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasTIM)
            app.run()

        End Using
    End Sub


    Private Sub MatarProcessosdeAdobeATivos()

        Dim ProcessosAdobe() As Process = Process.GetProcessesByName("Acrobat")

        For Each processo As Process In ProcessosAdobe
            processo.Kill()
        Next



    End Sub
End Module
