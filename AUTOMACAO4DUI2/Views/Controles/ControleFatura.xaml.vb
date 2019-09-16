Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports BibliotecaAutomacaoFaturas

Public Class ControleFatura
    Implements INotifyPropertyChanged

    Public Shared ReadOnly FaturaProperty As DependencyProperty =
    DependencyProperty.Register("Fatura", GetType(Fatura), GetType(ControleFatura),
     New PropertyMetadata(New Fatura, AddressOf OnValueChanged))
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Property Fatura() As Fatura
        Get
            Return CType(GetValue(FaturaProperty), Fatura)
        End Get
        Set(ByVal value As Fatura)
            SetValue(FaturaProperty, value)

        End Set
    End Property

    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().

    End Sub

    Private Overloads Shared Sub OnValueChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)

        Dim controleFatura = CType(d, ControleFatura)
        If controleFatura IsNot Nothing Then controleFatura.OnValueChanged(e)

    End Sub

    Private Overloads Sub OnValueChanged(e As DependencyPropertyChangedEventArgs)

        Try

            If CType(e.NewValue, Fatura).Baixada Then

                RetanguloBaixada.Fill = Brushes.Green

            Else
                RetanguloBaixada.Fill = Brushes.Red

            End If


            If CType(e.NewValue, Fatura).Tratada Then
                RetanguloFluxoDisparado.Fill = Brushes.Green
            Else
                RetanguloFluxoDisparado.Fill = Brushes.Red
            End If

            If CType(e.NewValue, Fatura).Pendente Then
                RetanguloPendente.Fill = Brushes.Red
            Else
                RetanguloPendente.Fill = Brushes.Green
            End If


            If CType(e.NewValue, Fatura).FaturaPosicionadaNaPasta Then
                RetanguloPasta.Fill = Brushes.Green
            Else
                RetanguloPasta.Fill = Brushes.Red
            End If

            If CType(e.NewValue, Fatura).FluxoDisparado Then
                RetanguloFluxoDisparado.Fill = Brushes.Green
            Else
                RetanguloFluxoDisparado.Fill = Brushes.Red
            End If


            If CType(e.NewValue, Fatura).FaturaProcessadaFox Then
                RetanguloWebApp.Fill = Brushes.Green
            Else
                RetanguloWebApp.Fill = Brushes.Red
            End If

            If CType(e.NewValue, Fatura).FaturaEnviadaParaDrive Then
                RetanguloDrive.Fill = Brushes.Green
            Else
                RetanguloDrive.Fill = Brushes.Red
            End If

            Dim listaExibir As New List(Of String)
            For Each x In CType(e.NewValue, Fatura).LogRobo
                listaExibir.Add(x)
            Next
            listaExibir.Reverse()

            ListaLogRobo.ItemsSource = listaExibir



        Catch ex As NullReferenceException

        End Try


    End Sub

End Class
