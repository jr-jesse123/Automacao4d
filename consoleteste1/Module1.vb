Imports Autofac
Imports BibliotecaAutomacaoFaturas
Imports Squirrel


Module Module1

    Sub Main()
        VerificarAtualizacoes()

        Utilidades.MatarProcessosdeAdobeATivos()

        Dim container As IContainer = ContainerConfig.Configure

        'Using scope = container.BeginLifetimeScope
        '    Dim app = scope.Resolve(Of RoboVivoFixo)
        '    AddHandler app.Log, AddressOf MostrarLog
        '    Dim listadecontas = GerRelDB.SelecionarContasRobosParaDownload(app)
        '    app.run(listadecontas)

        'End Using



        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasTIM)
            AddHandler app.Log, AddressOf MostrarLog
            Dim listadecontas = GerRelDB.SelecionarContasRobosParaDownload(app)
            app.run(listadecontas)

        End Using


        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasClaro)
            AddHandler app.Log, AddressOf MostrarLog
            Dim listadecontas = GerRelDB.SelecionarContasRobosParaDownload(app)
            app.run(listadecontas)

        End Using




        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasVIVOMOVEL)
            AddHandler app.Log, AddressOf MostrarLog
            Dim listadecontas = GerRelDB.SelecionarContasRobosParaDownload(app)
            app.run(listadecontas)

        End Using




        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasClaro)
            AddHandler app.Log, AddressOf MostrarLog
            Dim listadecontas = GerRelDB.SelecionarContasRobos(app)
            app.run(listadecontas)

        End Using


        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasTIM)
            AddHandler app.Log, AddressOf MostrarLog
            Dim listadecontas = GerRelDB.SelecionarContasRobos(app)
            app.run(listadecontas)

        End Using



        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasVIVOMOVEL)
            AddHandler app.Log, AddressOf MostrarLog
            Dim listadecontas = GerRelDB.SelecionarContasRobos(app)
            app.run(listadecontas)

        End Using






    End Sub
    ''' <summary>
    ''' Essa função mata os processos de adobe abertos 
    ''' </summary>



    Private Async Function VerificarAtualizacoes() As Task

        Using manager As New UpdateManager("\\192.168.244.112\4d_consultoria\PROGRAMAS - INSTALADORES\AUTOMACAO4D\ROBOFATURAS")

            Await manager.UpdateApp

        End Using

    End Function

    Public Sub MostrarLog(texto As String)
        Console.WriteLine(texto)
    End Sub

End Module
