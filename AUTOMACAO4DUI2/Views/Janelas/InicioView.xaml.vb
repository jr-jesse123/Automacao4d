Imports BibliotecaAutomacaoFaturas
Imports Squirrel

Public Class InicioView

    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().

        VerificarAtualizacoes()

        AdicionarNumeroVersao()

    End Sub

    Private Sub AdicionarNumeroVersao()
        Dim assembly = System.Reflection.Assembly.GetExecutingAssembly
        Dim versioninfo = FileVersionInfo.GetVersionInfo(assembly.Location)
        Me.Title += $" v.{versioninfo.FileVersion} teste internet novo repositorio somentes googledrive"

    End Sub

    Private Async Function VerificarAtualizacoes() As Task

        Using manager As New UpdateManager("\\192.168.244.112\4d_consultoria\PROGRAMAS - INSTALADORES\AUTOMACAO4D\INTERFACEUSUARIO")

            Await manager.UpdateApp

        End Using

    End Function

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

    End Sub


End Class
