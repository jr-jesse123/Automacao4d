Imports Autofac
Imports BibliotecaAutomacaoFaturas


Module ModuloTeste

    Sub Main()

        MatarProcessosdeAdobeATivos()
        Dim container As IContainer = ContainerConfig.Configure


        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasOI)
            AddHandler app.Log, AddressOf MostrarLog
            app.run()

        End Using


    End Sub


    Public Sub MostrarLog(texto As String)
        Console.WriteLine(texto)
    End Sub



    Private Sub MatarProcessosdeAdobeATivos()

        Dim ProcessosAdobe() As Process = Process.GetProcessesByName("Acrobat")

        For Each processo As Process In ProcessosAdobe
            processo.Kill()
        Next

    End Sub
End Module
