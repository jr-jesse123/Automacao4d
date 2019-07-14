



Imports System.Collections.ObjectModel
Imports BibliotecaAutomacaoFaturas

Public Class ClientesViewModel

    Public Property AdicionarClienteBtnCommand As New AdicionarClienteBtnCommand
    Public Property EditarClienteBtnCommand As New EditarClienteBtnCommand
    Public Property RemoverClienteBtnCommand As New RemoverClienteBtnCommand

    Public Property clientes As New List(Of Empresa)
    Public Property ClientesFiltrados As New ObservableCollection(Of Empresa)

    Private _filtro As String = "text"
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

    Private Sub AtualizarClientesfiltrados()



        Dim funil = clientes.Where(Function(c) c.Nome.ToLower.Contains(Filtro.ToLower)).ToList

        ClientesFiltrados.Clear()

        For Each cliente In funil
            ClientesFiltrados.Add(cliente)
        Next



    End Sub

    Private Sub PreencherListaclientes()
        Dim x As New GerenciadordeRelacionamentosMongo
        clientes = x.Empresas


        ClientesFiltrados.Clear()

        For Each cliente In clientes
            ClientesFiltrados.Add(cliente)
        Next


    End Sub

    Private Sub AdicionarCliente()

        Dim AdicionarclienteView As New AdicionarClienteView
        AdicionarclienteView.ShowDialog()

    End Sub



    Private Sub RemoverClienteSelecionado(ClienteSelecionado As Empresa)

        Dim x As New BibliotecaAutomacaoFaturas.MongoDb("AUTOMACAO4D")
        x.DeleRecord(Of Empresa)("ClientesEContas", ClienteSelecionado.CNPJ.ToString)

        PreencherListaclientes()

    End Sub


End Class


