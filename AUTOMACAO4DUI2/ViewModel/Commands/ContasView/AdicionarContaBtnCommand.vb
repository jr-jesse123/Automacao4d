﻿Public Class AdicionarContaBtnCommand
    Implements ICommand
    Private ContasVM As ContasViewModel

    Sub New(ContasVm As ContasViewModel)
        Me.ContasVM = ContasVm
    End Sub


    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        ContasVM.AdicionarConta()
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class


