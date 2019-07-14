Imports BibliotecaAutomacaoFaturas

Public Class RemoverClienteBtnCommand
    Implements ICommand
    Private ClienteVM As ClientesViewModel

    Sub New(ClienteVM As ClientesViewModel)
        Me.ClienteVM = ClienteVM
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
    Public Event BancoAtualizado()

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        ClienteVM.RemoverClienteSelecionado(CType(parameter, Empresa))

    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
