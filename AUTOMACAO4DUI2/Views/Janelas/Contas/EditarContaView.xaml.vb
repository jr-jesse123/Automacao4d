Imports System.ComponentModel
Imports BibliotecaAutomacaoFaturas

Public Class EditarContaView
    Implements INotifyPropertyChanged

    Private _conta As Conta
    Public Property Conta As Conta
        Get
            Return _conta
        End Get
        Set
            _conta = Value
            NotifyPropertyChanged("Conta")
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Sub New(conta As Conta)
        Me.Conta = conta


        ' Esta chamada é requerida pelo designer.
        InitializeComponent()


        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        CtrContaExibir.Conta = conta

    End Sub

    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub


End Class
