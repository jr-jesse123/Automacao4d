Public Class AdicionarcontaIcommand
    Implements ICommand


    Private adicionarContaVM As AdicionarContaVM
    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub New(adicionarContaVM As AdicionarContaVM)
        Me.adicionarContaVM = adicionarContaVM
    End Sub



    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        adicionarContaVM.AdicionarConta(parameter)
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
