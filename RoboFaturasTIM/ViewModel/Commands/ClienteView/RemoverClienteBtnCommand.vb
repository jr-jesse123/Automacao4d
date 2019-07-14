Public Class RemoverClienteBtnCommand
    Implements ICommand

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute

    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute

    End Function
End Class
