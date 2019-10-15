
Imports Autofac
Imports LibAutoFaturasStantard
Imports Microsoft.Speech.AudioFormat
Imports Microsoft.Speech.Recognition
Imports Squirrel
Imports System.IO

Module Module1


    Sub Main()

        VerificarAtualizacoes()

        Utilidades.MatarProcessosdeAdobeATivos()

        Dim container As IContainer = ContainerConfig.Configure


        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboVivoFixo)
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
            Dim app = scope.Resolve(Of RoboFaturasOI)
            AddHandler app.Log, AddressOf MostrarLog
            Dim listadecontas = GerRelDB.SelecionarContasRobosParaDownload(app)
            app.run(listadecontas)

        End Using



        Using scope = container.BeginLifetimeScope
            Dim app = scope.Resolve(Of RoboFaturasTIM)
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


Public Class streaamm
    Inherits IO.Stream

    Public Overrides ReadOnly Property CanRead As Boolean = True
    Public Overrides ReadOnly Property CanSeek As Boolean = True
    Public Overrides ReadOnly Property CanWrite As Boolean = False
    Public Overrides ReadOnly Property Length As Long
    Public Overrides Property Position As Long

    Public Overrides Sub Flush()
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub SetLength(value As Long)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub Write(buffer() As Byte, offset As Integer, count As Integer)
        Throw New NotImplementedException()
    End Sub

    Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long
        Throw New NotImplementedException()
    End Function

    Public Overrides Function Read(buffer() As Byte, offset As Integer, count As Integer) As Integer
        Throw New NotImplementedException()
    End Function
End Class
