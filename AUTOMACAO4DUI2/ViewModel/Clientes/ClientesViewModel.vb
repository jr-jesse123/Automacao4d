



Imports System.Collections.ObjectModel
Imports BibliotecaAutomacaoFaturas

Public Class ClientesViewModel



    Public Property AdicionarClienteBtnCommand As New AdicionarClienteBtnCommand(Me)
    Public Property EditarClienteBtnCommand As New EditarClienteBtnCommand(Me)
    Public Property RemoverClienteBtnCommand As New RemoverClienteBtnCommand(Me)
    Public WithEvents Banco As New GerRelDB
    Public Property ClientesFiltrados As New ObservableCollection(Of Empresa)
    Private _filtro As String = ""
    Public Property Filtro() As String
        Get
            Return _filtro
        End Get
        Set(ByVal value As String)
            _filtro = value
            AtualizarClientesfiltrados()
        End Set
    End Property

    Sub New()
        PreencherListaclientes()

    End Sub

    Friend Sub EditarCliente(empresa As Empresa)

        Dim x As New EditarClienteView(empresa)
        x.Show()

    End Sub


    Private Sub AtualizarClientesfiltrados() Handles Banco.BancoAtualizado

        Dim funil = GerRelDB.Empresas.Where(Function(c) c.Nome.ToLower.Contains(Filtro.ToLower)).ToList

        ClientesFiltrados.Clear()

        For Each cliente In funil
            ClientesFiltrados.Add(cliente)
        Next



    End Sub

    Private Sub PreencherListaclientes()

        Dim clientes = GerRelDB.Empresas

        ClientesFiltrados.Clear()

        For Each cliente In clientes
            ClientesFiltrados.Add(cliente)
        Next


    End Sub

    Public Sub AdicionarCliente()

        Dim AdicionarclienteView As New AdicionarClienteView
        AdicionarclienteView.ShowDialog()


    End Sub

    Public Sub RemoverClienteSelecionado(ClienteSelecionado As Empresa)

        Banco.RemoverEmpresa(ClienteSelecionado)
    End Sub


End Class


