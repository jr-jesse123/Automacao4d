Imports AUTOMACAO4DUI2
Imports LibAutoFaturasStantard



Public Class EditarGestorVM


    Public Property AtualizarICommand As New AtualizarGestorIcommand(Me)


    Public Sub AtualizarConta(Gestor As Gestor)

        GerRelDB.UpsertGestor(Gestor)

    End Sub

End Class

Public Class AtualizarGestorIcommand
    Implements ICommand
    Private editarGestorVM As EditarGestorVM

    Public Sub New(editarGestorVM As EditarGestorVM)
        Me.editarGestorVM = editarGestorVM
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        GerRelDB.UpsertGestor(CType(parameter, Gestor))
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
