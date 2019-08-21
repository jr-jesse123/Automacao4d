Imports System.Collections.ObjectModel
Imports BibliotecaAutomacaoFaturas

Public Class ControleFatura

    Private _fatura As Fatura
    Public Property Logs As New ObservableCollection(Of String)

    Public Shared ReadOnly FaturaProperty As DependencyProperty =
    DependencyProperty.Register("Fatura", GetType(Fatura), GetType(ControleFatura),
     New PropertyMetadata(New Fatura, AddressOf OnValueChanged))

    Public Property Fatura() As Fatura
        Get
            Return CType(GetValue(FaturaProperty), Fatura)
        End Get
        Set(ByVal value As Fatura)
            SetValue(FaturaProperty, value)

        End Set
    End Property

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

            If CType(e.NewValue, Fatura).Pendente Then
                RetanguloPendente.Fill = Brushes.Red
            Else
                RetanguloPendente.Fill = Brushes.Green
            End If

            ListaLogRobo.ItemsSource = CType(e.NewValue, Fatura).LogRobo

        Catch ex As NullReferenceException

            If CType(e.OldValue, Fatura).Baixada Then
                RetanguloBaixada.Fill = Brushes.Green
            Else
                RetanguloBaixada.Fill = Brushes.Red
            End If

            If CType(e.OldValue, Fatura).Pendente Then
                RetanguloPendente.Fill = Brushes.Red
            Else
                RetanguloPendente.Fill = Brushes.Green

            End If


        End Try


    End Sub



End Class
