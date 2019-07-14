



Imports System.Collections.ObjectModel
Imports BibliotecaAutomacaoFaturas

Public Class ContasViewModel

    Public Property AdicionarContaBtnCommand As New AdicionarContaBtnCommand(Me)
    Public Property EditarContaBtnCommand As New EditarContaBtnCommand(Me)
    Public Property RemoverContaBtnCommand As New RemoverContaBtnCommand(Me)
    Public WithEvents Banco As New GerRelDB
    Public Property ContasFiltradas As New ObservableCollection(Of Conta)
    Private _filtro As String = ""

    Public Property Filtro() As String
        Get
            Return _filtro
        End Get
        Set(ByVal value As String)
            _filtro = value
            AtualizarContasFiltradas()
        End Set
    End Property

    Sub New()
        PreencherListaContas()

    End Sub

    Private Sub AtualizarContasFiltradas() Handles Banco.BancoAtualizado

        Dim funil = GerRelDB.Contas.Where(Function(c)
                                                                       Return c.Empresa.Nome.ToLower.Contains(_filtro.ToLower) Or
                                                                       c.NrDaConta.Contains(_filtro.ToLower)
                                                                   End Function).ToList

        ContasFiltradas.Clear()

        For Each conta In funil
            ContasFiltradas.Add(conta)
        Next

    End Sub

    Private Sub PreencherListaContas()

        For Each conta In GerRelDB.Contas
            ContasFiltradas.Add(conta)
        Next

    End Sub

    Friend Sub AdicionarConta()

        Dim AdicionarContaView As New AdicionarContaView

        AdicionarContaView.Show()

    End Sub

    Friend Sub RemoverConta(Conta As Conta)

        Banco.removerConta(Conta)

    End Sub

    Friend Sub EditarConta(parameter As Conta)
        Dim x As New EditarContaView(parameter)
        x.Show()
    End Sub
End Class


