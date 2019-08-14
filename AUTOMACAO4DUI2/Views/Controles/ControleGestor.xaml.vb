Imports BibliotecaAutomacaoFaturas



Public Class ControleGestor

    Private _gestor As Gestor


    Public Shared ReadOnly GestorProperty As DependencyProperty =
    DependencyProperty.Register("Gestor", GetType(Gestor), GetType(ControleGestor),
     New PropertyMetadata(New Gestor, AddressOf OnValueChanged))

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
        'Try
        '    RazaoSocial.Text = CType(e.NewValue, Gestor).Nome
        '    NomeFantasia.Text = CType(e.NewValue, Gestor).NomeFantasia
        '    CNPJ.Text = CType(e.NewValue, Gestor).CNPJ
        '    CNPJHOLDING.Text = CType(e.NewValue, Gestor).HoldingID
        '    LoginContaOnline.Text = CType(e.NewValue, Gestor).LoginContaOnline
        '    txtBoxSenhaContaOnline.Text = CType(e.NewValue, Gestor).SenhaContaOnline
        '    IDBitrix.Text = CType(e.NewValue, Gestor).BitrixID
        'Catch ex As NullReferenceException

        '    RazaoSocial.Text = CType(e.OldValue, Gestor).Nome
        '    NomeFantasia.Text = CType(e.OldValue, Gestor).NomeFantasia
        '    CNPJ.Text = CType(e.OldValue, Gestor).CNPJ
        '    CNPJHOLDING.Text = CType(e.OldValue, Gestor).HoldingID
        '    LoginContaOnline.Text = CType(e.OldValue, Gestor).LoginContaOnline
        '    txtBoxSenhaContaOnline.Text = CType(e.OldValue, Gestor).SenhaContaOnline
        '    IDBitrix.Text = CType(e.OldValue, Gestor).BitrixID
        'End Try


    End Sub

    Private Sub BotaoAdicionarSEnha_Click(sender As Object, e As RoutedEventArgs)
        Dim x As New AdicionarSenhaView(Gestor)
        x.ShowDialog()

    End Sub

    Private Sub BotaoRemoverSEnha_Click(sender As Object, e As RoutedEventArgs)
        If ListaSenhas.SelectedItem IsNot Nothing Then
            Gestor.ListaSenhas.Remove(ListaSenhas.SelectedItem)
        End If


    End Sub
End Class