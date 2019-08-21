Imports System.Collections.ObjectModel
Imports BibliotecaAutomacaoFaturas

Public Class FaturasView


    Public Property ContasEFaturas As New ObservableCollection(Of Tuple(Of Fatura, Conta))

    Public Property FiltroVencimento As Integer
        Get
            Return _filtroVencimento
        End Get
        Set(value As Integer)
            _filtroVencimento = value
            AtualizarContasFiltradas
        End Set
    End Property

    Public Property FiltroOperadora As OperadoraEnum
        Get
            Return _filtroOperadora
        End Get
        Set(value As OperadoraEnum)
            _filtroOperadora = value
            AtualizarContasFiltradas
        End Set
    End Property


    Public Property FiltroTxt As String
        Get
            Return _filtroTxt
        End Get
        Set(value As String)
            _filtroTxt = value
            AtualizarContasFiltradas()
        End Set
    End Property

    Private _filtroVencimento As Integer
    Private _filtroOperadora As OperadoraEnum
    Private _filtroTxt As String = ""

    Private Sub AtualizarContasFiltradas()



        Dim funil = GerRelDB.Contas.Where(Function(c)
                                              Return (c.Empresa.Nome.ToLower.Contains(_filtroTxt.ToLower) Or
                                              c.NrDaConta.Contains(_filtroTxt.ToLower)) And
                                              ValidarVencimento(c) And
                                              ValidarOperadora(c)
                                          End Function).ToList

        ContasEFaturas.Clear()

        For Each conta In funil

            Dim x = Tuple.Create(conta.Faturas.Last, conta)
            ContasEFaturas.Add(x)
        Next

    End Sub

    Private Function ValidarVencimento(c As Conta) As Boolean

        If FiltroVencimento = 0 Then
            Return True
        Else
            If FiltroVencimento = c.Vencimento Then
                Return True
            Else
                Return False
            End If
        End If

    End Function

    Private Function ValidarOperadora(c As Conta) As Boolean

        If FiltroOperadora = 0 Then
            Return True
        Else
            If c.Operadora = FiltroOperadora Then
                Return True
            Else
                Return False
            End If
        End If


    End Function



    Sub New(contas As List(Of Conta))

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().

        For Each conta In contas

            Dim x = Tuple.Create(conta.Faturas.Last, conta)
            ContasEFaturas.Add(x)
        Next


        OperadoraCB.ItemsSource = [Enum].GetNames(GetType(OperadoraEnum))


    End Sub


End Class
