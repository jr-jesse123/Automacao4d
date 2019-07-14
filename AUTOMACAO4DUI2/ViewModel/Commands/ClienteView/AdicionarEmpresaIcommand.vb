Public Class AdicionarEmpresaIcommand
    Implements ICommand
    Private VM As AdicionarClienteVM
    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Sub New(VM As AdicionarClienteVM)
        Me.VM = VM
    End Sub

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        VM.AdicionarEmpresa(parameter)
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
