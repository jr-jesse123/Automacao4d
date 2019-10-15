Imports LibAutoFaturasStantard

Public Class AtualizarIcommand

    Implements ICommand

    Private editarContaVM As EditarContaVM

    Public Sub New(editarContaVM As EditarContaVM)
        Me.editarContaVM = editarContaVM
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        editarContaVM.AtualizarConta(CType(parameter, Conta))
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
