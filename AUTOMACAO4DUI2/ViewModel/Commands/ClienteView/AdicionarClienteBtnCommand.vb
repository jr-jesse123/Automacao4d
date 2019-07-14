Imports BibliotecaAutomacaoFaturas

Public Class AdicionarClienteBtnCommand
    Implements ICommand
    Private ClientesVM As ClientesViewModel

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
    Public Event BancoAtualizado()

    Sub New(ClientesVM As ClientesViewModel)
        Me.ClientesVM = ClientesVM
    End Sub


    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        ClientesVM.AdicionarCliente()
        RaiseEvent BancoAtualizado()
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
