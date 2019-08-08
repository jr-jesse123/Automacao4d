Imports AUTOMACAO4DUI2
Imports BibliotecaAutomacaoFaturas

Public Class AdicionarGestorVM

    Property AdicionarGestorICommand As New AdicionarGestorIcommand(Me)


    Public Sub AdicionarGestor(gestor As Gestor)

        GerRelDB.AdicionarGestor(gestor)

    End Sub

End Class

Public Class AdicionarGestorIcommand
    Implements ICommand

    Private adicionarGestorVM As AdicionarGestorVM

    Public Sub New(adicionarGestorVM As AdicionarGestorVM)
        Me.adicionarGestorVM = adicionarGestorVM
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        adicionarGestorVM.AdicionarGestor(CType(parameter, Gestor))
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
