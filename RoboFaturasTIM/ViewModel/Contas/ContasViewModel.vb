



Imports System.Collections.ObjectModel
Imports BibliotecaAutomacaoFaturas

Public Class ContasViewModel

    Public Property AdicionarCcontaBtnCommand As New AdicionarContaBtnCommand
    Public Property EditarContaBtnCommand As New EditarContaBtnCommand
    Public Property RemoverContaBtnCommand As New RemoverContaBtnCommand

    Public Property Contas As New ObservableCollection(Of Conta)
    Public Property ContasFiltradas As New ObservableCollection(Of Conta)

    Private _filtro As String = "text"
    Public Property Filtro() As String
        Get
            Return _filtro
        End Get
        Set(ByVal value As String)
            _filtro = value
            '    AtualizarClientesfiltrados()
        End Set
    End Property





    Sub New()
        PreencherListaContas()

    End Sub

    Private Sub AtualizarContasFiltradas()

        Dim funil = GerenciadordeRelacionamentosMongo.Contas.Where(Function(c) c.Empresa.Nome.ToLower.Contains(Filtro.ToLower)).ToList

        ContasFiltradas.Clear()

        For Each conta In funil
            ContasFiltradas.Add(conta)
        Next



    End Sub

    Private Sub PreencherListaContas()

        For Each conta In GerenciadordeRelacionamentosMongo.Contas
            Contas.Add(conta)
        Next

    End Sub



    Private Sub AdicionarConta()

        Dim AdicionarContaView As New AdicionarContaView
        AdicionarContaView.ShowDialog()

    End Sub



    Private Sub RemoverContaSelecionada(ContaSelecionada As Conta)

        Dim x As New BibliotecaAutomacaoFaturas.MongoDb("AUTOMACAO4D")
        x.DeleTarConta(ContaSelecionada)

    End Sub


End Class

Public Class RemoverContaBtnCommand
    Implements ICommand

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Throw New NotImplementedException()
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Throw New NotImplementedException()
    End Function
End Class

Public Class EditarContaBtnCommand
    Implements ICommand

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Throw New NotImplementedException()
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Throw New NotImplementedException()
    End Function
End Class


Public Class AdicionarContaBtnCommand
    Implements ICommand

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Throw New NotImplementedException()
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Throw New NotImplementedException()
    End Function
End Class


