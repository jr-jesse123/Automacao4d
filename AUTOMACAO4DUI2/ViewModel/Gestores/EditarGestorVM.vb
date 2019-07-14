Imports AUTOMACAO4DUI2
Imports BibliotecaAutomacaoFaturas



Public Class EditarGestorVM
    Public Property AtualizarICommand As New AtualizarGestorIcommand(Me)


    Public Sub AtualizarConta(Gestor As Gestor)

        GerRelDB.UpsertGestor(Gestor)

    End Sub
End Class

Public Class AtualizarGestorIcommand
    Private editarGestorVM As EditarGestorVM

    Public Sub New(editarGestorVM As EditarGestorVM)
        Me.editarGestorVM = editarGestorVM
    End Sub
End Class
