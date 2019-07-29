Imports Squirrel

Public Class InicioView

    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().

        VerificarAtualizacoes()

    End Sub

    Private Async Function VerificarAtualizacoes() As Task

        Using manager As New UpdateManager("\\Servidor\4d_consultoria\PROGRAMAS - INSTALADORES")

            Await manager.UpdateApp

        End Using

    End Function

End Class
