Imports System.Collections.ObjectModel
Imports BibliotecaAutomacaoFaturas

Public Class FaturasView

    Public Property ContasEFaturas As New ObservableCollection(Of Tuple(Of Fatura, Conta))

    Private _filtroVencimento As Integer
    Private _filtroOperadora As OperadoraEnum
    Private _filtroTxt As String = ""
    Private _FiltroDadosOk As Boolean
    Private _FiltroBaixada As Boolean
    Private _FluxoDisparado As Boolean

    Public Property FiltroVencimento As Integer
        Get
            Return _filtroVencimento
        End Get
        Set(value As Integer)
            _filtroVencimento = value
            AtualizarContasFiltradas()
        End Set
    End Property
    Public Property FiltroOperadora As OperadoraEnum
        Get
            Return _filtroOperadora
        End Get
        Set(value As OperadoraEnum)
            _filtroOperadora = value
            AtualizarContasFiltradas()
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
    Public Property FiltroDadosOk As String
        Get
            Return _FiltroDadosOk.ToString
        End Get
        Set

            If Value.Contains("Ok") Then
                _FiltroDadosOk = True
            Else
                _FiltroDadosOk = False
            End If

            AtualizarContasFiltradas()
        End Set
    End Property

    Public Property FiltroBaixada As String
        Get
            Return _FiltroBaixada.ToString
        End Get
        Set


            If Value.Contains("Sim") Then
                _FiltroBaixada = True
            Else
                _FiltroBaixada = False
            End If



            AtualizarContasFiltradas()
        End Set
    End Property

    Public Property FluxoDisparado As String
        Get
            Return _FluxoDisparado.ToString
        End Get
        Set

            If Value = "Iniciado" Then
                _FluxoDisparado = True
            Else
                _FluxoDisparado = False
            End If



            AtualizarContasFiltradas()
        End Set
    End Property

    Private Sub AtualizarContasFiltradas()

        ContasEFaturas.Clear()

        Dim filtroContas = GerRelDB.Contas.Where(Function(c)
                                                     Return (c.Empresa.Nome.ToLower.Contains(_filtroTxt.ToLower) Or
                                              c.NrDaConta.Contains(_filtroTxt.ToLower)) And
                                              ValidarVencimento(c) And
                                              ValidarDadosOk(c)
                                                     ValidarOperadora(c)
                                                 End Function).ToList

        For Each conta In filtroContas
            Dim FiltroFaturas = conta.Faturas.Where(Function(f)
                                                        Return ValidarBaixada(f) And
                                              ValidarFluxoDisparado(f)
                                                    End Function)

            For Each fatura In FiltroFaturas
                Dim x = Tuple.Create(fatura, conta)
                ContasEFaturas.Add(x)
            Next
        Next


    End Sub

    Private Function ValidarFluxoDisparado(f As Fatura)
        Return f.Tratada = _FluxoDisparado
    End Function

    Private Function ValidarBaixada(f As Fatura)
        Return f.Baixada = _FiltroBaixada
    End Function

    Private Function ValidarDadosOk(c As Conta)
        Return c.DadosOk = _FiltroDadosOk
    End Function

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
