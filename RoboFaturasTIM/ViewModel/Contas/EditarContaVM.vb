Imports RoboFaturasTIM

Public Class EditarContaVM
    Public Property Editar As New EditarContaIcommand(Me)
End Class



Public Class EditarContaIcommand

    Implements ICommand

    Private editarContaVM As EditarContaVM

    Public Sub New(editarContaVM As EditarContaVM)
        Me.editarContaVM = editarContaVM
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Throw New NotImplementedException()
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Throw New NotImplementedException()
    End Function
End Class
