Imports BibliotecaAutomacaoFaturas

Public Class EditarClienteBtnCommand
    Implements ICommand

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Dim empresa = CType(parameter, Empresa)

        Dim x As New EditarClienteView(empresa)

        x.Show()
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
