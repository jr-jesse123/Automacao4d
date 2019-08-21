Imports Squirrel

Public Class InicioView

    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().

        VerificarAtualizacoes()

        AdicionarNumeroVersao

    End Sub

    Private Sub AdicionarNumeroVersao()
        Dim assembly = System.Reflection.Assembly.GetExecutingAssembly
        Dim versioninfo = FileVersionInfo.GetVersionInfo(assembly.Location)
        Me.Title += $" v.{versioninfo.FileVersion} teste internet novo repositorio somentes googledrive"

    End Sub

    Private Async Function VerificarAtualizacoes() As Task

        Using manager As New UpdateManager("\\Servidor\4d_consultoria\PROGRAMAS - INSTALADORES\AUTOMACAO4D\INTERFACEUSUARIO")

            Await manager.UpdateApp

        End Using

    End Function

End Class
