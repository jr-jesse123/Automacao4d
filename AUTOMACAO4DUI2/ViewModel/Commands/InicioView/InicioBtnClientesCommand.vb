﻿

Public Class InicioBtnClientesCommand
    Implements ICommand

    Private inicioVM As InicioVM

    Public Sub New(inicioVM As InicioVM)
        Me.inicioVM = inicioVM
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Dim ClientesView As New ClientesView
        ClientesView.Show()
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
