



Imports System.Collections.ObjectModel
Imports BibliotecaAutomacaoFaturas

Public Class ContasViewModel

    Public Property AdicionarContaBtnCommand As New AdicionarContaBtnCommand(Me)
    Public Property EditarContaBtnCommand As New EditarContaBtnCommand(Me)
    Public Property RemoverContaBtnCommand As New RemoverContaBtnCommand(Me)
    Public WithEvents Banco As New GerRelDB
    Public Property ContasFiltradas As New ObservableCollection(Of Conta)
    Private _filtro As String = ""
    Private _Operadora As OperadoraEnum
    Private _Tipoconta As TipoContaEnum
    Private _Subtipo As SubtipoEnum

    Public Property Operadora As OperadoraEnum
        Get
            Return _Operadora
        End Get
        Set
            _Operadora = Value
            AtualizarContasFiltradas()
        End Set
    End Property

    Public Property Tipoconta As TipoContaEnum
        Get
            Return _Tipoconta
        End Get
        Set
            _Tipoconta = Value
            AtualizarContasFiltradas()
        End Set
    End Property

    Public Property Subtipo As SubtipoEnum
        Get
            Return _Subtipo
        End Get
        Set
            _Subtipo = Value
            AtualizarContasFiltradas()
        End Set
    End Property

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
                                              Return (c.Empresa.Nome.ToLower.Contains(_filtro.ToLower) Or
                                              c.NrDaConta.Contains(_filtro.ToLower)) And
                                              ValidarOperadora(c) And
                                              ValidarTipo(c) And
                                              ValidarSubtipo(c)
                                          End Function).ToList

        ContasFiltradas.Clear()

        For Each conta In funil
            ContasFiltradas.Add(conta)
        Next

    End Sub

    Private Function ValidarSubtipo(c As Conta) As Boolean

        If Not [Enum].IsDefined(GetType(SubtipoEnum), Subtipo) Then
            Return True
        Else
            Return c.Subtipo = Subtipo
        End If


    End Function

    Private Function ValidarTipo(c As Conta) As Boolean

        If Not [Enum].IsDefined(GetType(TipoContaEnum), Tipoconta) Then
            Return True
        Else
            Return c.TipoDeConta = Tipoconta
        End If


    End Function

    Private Function ValidarOperadora(c As Conta) As Boolean

        If Not [Enum].IsDefined(GetType(OperadoraEnum), Operadora) Then
            Return True
        Else
            Return c.Operadora = Operadora
        End If

    End Function

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


