﻿Imports Autofac
Imports BibliotecaAutomacaoFaturas
Imports Squirrel


Module Module1

    Sub Main()
        VerificarAtualizacoes()

        MatarProcessosdeAdobeATivos()
        Dim container As IContainer = ContainerConfig.Configure


        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasVIVOMOVEL)
            AddHandler app.Log, AddressOf MostrarLog
            app.run()

        End Using


        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasTIM)
            AddHandler app.Log, AddressOf MostrarLog
            app.run()

        End Using


        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasClaro)
            AddHandler app.Log, AddressOf MostrarLog
            app.run()

        End Using

        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasALGAR)
            AddHandler app.Log, AddressOf MostrarLog
            app.run()

        End Using


    End Sub


    Private Sub MatarProcessosdeAdobeATivos()

        Dim ProcessosAdobe() As Process = Process.GetProcessesByName("Acrobat")

        For Each processo As Process In ProcessosAdobe
            processo.Kill()
        Next

    End Sub

    Private Async Function VerificarAtualizacoes() As Task

        Using manager As New UpdateManager("\\Servidor\4d_consultoria\PROGRAMAS - INSTALADORES\AUTOMACAO4D\ROBOFATURAS")

            Await manager.UpdateApp

        End Using

    End Function

    Public Sub MostrarLog(texto As String)
        Console.WriteLine(texto)
    End Sub

End Module
