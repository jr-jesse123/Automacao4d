Imports BibliotecaAutomacaoFaturas

Public Class EditarEmpresaIcommand
    Implements ICommand

    Private editarClienteVM As EditarClienteVM

    Public Sub New(editarClienteVM As EditarClienteVM)
        Me.editarClienteVM = editarClienteVM
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Dim controle = CType(parameter, Empresa)
    

        GerRelDB.UpsertEmpresa(parameter)

    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
