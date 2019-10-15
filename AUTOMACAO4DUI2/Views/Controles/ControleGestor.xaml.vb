Imports System.Collections.ObjectModel
Imports LibAutoFaturasStantard



Public Class ControleGestor

    Private _gestor As Gestor
    Public Property senhas As New ObservableCollection(Of DadosDeAcesso)

    Public Shared ReadOnly GestorProperty As DependencyProperty =
    DependencyProperty.Register("Gestor", GetType(Gestor), GetType(ControleGestor),
     New PropertyMetadata(New Gestor, AddressOf OnValueChanged))

    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().

    End Sub


    Public Property Gestor() As Gestor
        Get
            Return CType(GetValue(GestorProperty), Gestor)
        End Get
        Set(ByVal value As Gestor)

            SetValue(GestorProperty, value)

        End Set
    End Property

    Private Overloads Shared Sub OnValueChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)

        Dim ControleGestor = CType(d, ControleGestor)
        If ControleGestor IsNot Nothing Then ControleGestor.OnValueChanged(e)


    End Sub

    Private Overloads Sub OnValueChanged(e As DependencyPropertyChangedEventArgs)

        Try
            txtBoxNomeGestor.Text = CType(e.NewValue, Gestor).Nome
            txtBoxEmailGestor.Text = CType(e.NewValue, Gestor).Email
            txtBoxLinhaMasterGestor.Text = CType(e.NewValue, Gestor).LinhaMaster
            Me.ListaSenhas.ItemsSource = CType(e.NewValue, Gestor).ListaSenhas
            txtBoxIdBitrix.Text = CType(e.NewValue, Gestor).BitrixID

            senhas.Clear()

            For Each senha In CType(e.NewValue, Gestor).ListaSenhas
                senhas.Add(senha)
            Next

        Catch ex As NullReferenceException

            txtBoxNomeGestor.Text = CType(e.OldValue, Gestor).Nome
            txtBoxEmailGestor.Text = CType(e.OldValue, Gestor).Email
            txtBoxLinhaMasterGestor.Text = CType(e.OldValue, Gestor).LinhaMaster
            Me.ListaSenhas.ItemsSource = CType(e.OldValue, Gestor).ListaSenhas
            txtBoxIdBitrix.Text = CType(e.OldValue, Gestor).BitrixID


            senhas.Clear()

            For Each senha In CType(e.NewValue, Gestor).ListaSenhas
                senhas.Add(senha)
            Next

        End Try

    End Sub

    Private Sub BotaoAdicionarSEnha_Click(sender As Object, e As RoutedEventArgs)
        Dim x As New AdicionarSenhaView(Gestor)

        AddHandler x.SenhasAlteradas, AddressOf AdicionarSenhaView_OnSenhasAlteradas

        x.ShowDialog()

    End Sub

    Private Sub BotaoRemoverSEnha_Click(sender As Object, e As RoutedEventArgs)
        If ListaSenhas.SelectedItem IsNot Nothing Then
            Gestor.ListaSenhas.Remove(ListaSenhas.SelectedItem)
            AdicionarSenhaView_OnSenhasAlteradas()
        End If


    End Sub

    Private Sub AdicionarSenhaView_OnSenhasAlteradas()
        senhas.Clear()

        For Each senha In Gestor.ListaSenhas
            senhas.Add(senha)
        Next

    End Sub

End Class