



Imports System.Collections.ObjectModel
Imports AUTOMACAO4DUI2
Imports BibliotecaAutomacaoFaturas

Public Class GestoresViewModel

    Public Property AdicionarGestorBtnCommand As New AdicionarGestorBtnCommand(Me)
    Public Property EditarGestorBtnCommand As New EditarGestorBtnCommand(Me)
    Public Property RemoverGesotrBtnCommand As New RemoverGestorBtnCommand(Me)
    Public WithEvents Banco As New GerRelDB
    Public Property GestoresFiltrados As New ObservableCollection(Of Gestor)
    Private _filtro As String = ""

    Public Property Filtro() As String
        Get
            Return _filtro
        End Get
        Set(ByVal value As String)
            _filtro = value
            AtualizarGestoresFiltrados()
        End Set
    End Property

    Sub New()
        PreencherLista()

    End Sub

    Private Sub AtualizarGestoresFiltrados() Handles Banco.BancoAtualizado

        Dim funil = GerRelDB.
            Gestores.Where(Function(g)
                               Return g.Nome.ToLower.Contains(_filtro.ToLower) Or
                               g.CPF.ToLower.Contains(_filtro.ToLower) Or
                               g.Email.ToLower.Contains(_filtro.ToLower) Or
                               g.LinhaMaster.ToLower.Contains(_filtro.ToLower)
                           End Function).ToList
        GestoresFiltrados.Clear()

        For Each gestor In funil
            GestoresFiltrados.Add(gestor)
        Next

    End Sub

    Private Sub PreencherLista()
        For Each gestor In GerRelDB.Gestores
            GestoresFiltrados.Add(gestor)
        Next
    End Sub

    Friend Sub RemoverGestor(parameter As Gestor)

        GerRelDB.removerGestor(parameter)

    End Sub

    Friend Sub EditarGestor(gestor As Gestor)

        Dim x As New EditarGestorView(gestor)
        x.Show

    End Sub

    Friend Sub AdicionarGestor()
        Dim x As New AdicionarGestorView
        x.Show()
    End Sub
End Class

Public Class RemoverGestorBtnCommand
    Implements ICommand
    Private gestoresViewModel As GestoresViewModel

    Public Sub New(gestoresViewModel As GestoresViewModel)
        Me.gestoresViewModel = gestoresViewModel
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        gestoresViewModel.RemoverGestor(CType(parameter, Gestor))
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class

Public Class EditarGestorBtnCommand
    Implements ICommand

    Private gestoresViewModel As GestoresViewModel

    Public Sub New(gestoresViewModel As GestoresViewModel)
        Me.gestoresViewModel = gestoresViewModel
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        gestoresViewModel.EditarGestor(CType(parameter, Gestor))
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class

Public Class AdicionarGestorBtnCommand
    Implements ICommand

    Private gestoresViewModel As GestoresViewModel

    Public Sub New(gestoresViewModel As GestoresViewModel)
        Me.gestoresViewModel = gestoresViewModel
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        gestoresViewModel.AdicionarGestor()
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class

