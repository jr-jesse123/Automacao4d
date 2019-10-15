Imports LibAutoFaturasStantard

Public Class EditarClienteBtnCommand
    Implements ICommand
    Private ClienteVm As ClientesViewModel
    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
    Public Event BancoAtualizado()


    Sub New(ClienteVM As ClientesViewModel)
        Me.ClienteVm = ClienteVM
    End Sub

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        ClienteVm.EditarCliente(CType(parameter, Empresa))
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
