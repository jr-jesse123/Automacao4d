Imports LibAutoFaturasStantard

Public Class ControleDadosDeAcesso

    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        OperadoraCB.ItemsSource = [Enum].GetValues(GetType(OperadoraEnum))
        TipoDeContaCB.ItemsSource = [Enum].GetValues(GetType(TipoContaEnum))



    End Sub

    Private _DadosDeAcesso As DadosDeAcesso


    Public Shared ReadOnly DadosDeAcessoProperty As DependencyProperty =
    DependencyProperty.Register("DadosDeAcesso", GetType(DadosDeAcesso), GetType(ControleDadosDeAcesso),
     New PropertyMetadata(New DadosDeAcesso, AddressOf OnValueChanged))

    Public Property DadosDeAcesso() As DadosDeAcesso
        Get
            Return CType(GetValue(DadosDeAcessoProperty), DadosDeAcesso)
        End Get
        Set(ByVal value As DadosDeAcesso)
            SetValue(DadosDeAcessoProperty, value)

        End Set
    End Property

    Private Overloads Shared Sub OnValueChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)

        Dim ControleDadosDeAcesso = CType(d, ControleDadosDeAcesso)
        If ControleDadosDeAcesso IsNot Nothing Then ControleDadosDeAcesso.OnValueChanged(e)

    End Sub

    Private Overloads Sub OnValueChanged(e As DependencyPropertyChangedEventArgs)
        Try
            Login.Text = CType(e.NewValue, DadosDeAcesso).Login
            Senha.Text = CType(e.NewValue, DadosDeAcesso).Senha
            OperadoraCB.SelectedItem = CType(e.NewValue, DadosDeAcesso).Operadora
            TipoDeContaCB.SelectedItem = CType(e.NewValue, DadosDeAcesso).Tipo

        Catch ex As NullReferenceException

            Login.Text = CType(e.OldValue, DadosDeAcesso).Login
            Senha.Text = CType(e.OldValue, DadosDeAcesso).Senha
            OperadoraCB.SelectedItem = CType(e.OldValue, DadosDeAcesso).Operadora
            TipoDeContaCB.SelectedItem = CType(e.OldValue, DadosDeAcesso).Tipo
        End Try




    End Sub

End Class
