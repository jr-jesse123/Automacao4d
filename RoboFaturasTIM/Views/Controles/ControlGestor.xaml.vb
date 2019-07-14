Imports BibliotecaAutomacaoFaturas



Public Class ControleGestor
        Inherits UserControl

    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().

    End Sub


    Public Shared ReadOnly GestorProperty As DependencyProperty =
    DependencyProperty.Register("Gestor", GetType(Gestor), GetType(ControleGestor),
     New PropertyMetadata(New Gestor With {.Nome = "TESTE"}, AddressOf OnValueChanged))

    Public Property Gestor() As Gestor
        Get
            Return CType(GetValue(GestorProperty), Gestor)
        End Get
        Set(ByVal value As Gestor)
            SetValue(GestorProperty, value)

        End Set
    End Property

    Private Overloads Shared Sub OnValueChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim controleGestor = CType(d, ControleGestor)
        controleGestor.OnValueChanged(e)


        If controleGestor IsNot Nothing Then

            controleGestor.txtBoxCPFGestor.Text = CType(e.NewValue, Gestor).CPF
            controleGestor.txtBoxNomeGestor.Text = CType(e.NewValue, Gestor).Nome
            controleGestor.txtBoxEmailGestor.Text = CType(e.NewValue, Gestor).Email
            controleGestor.txtBoxLinhaMasterGestor.Text = CType(e.NewValue, Gestor).LinhaMaster
            controleGestor.txtBoxLoginGestor.Text = CType(e.NewValue, Gestor).Login
            controleGestor.txtBoxSenhaAtendimentoGestor.Text = CType(e.NewValue, Gestor).SenhaDeAtendimento
            controleGestor.txtBoxSenhaContaOnlineGestor.Text = CType(e.NewValue, Gestor).SenhaContaOnline

        End If

    End Sub

    Protected Overridable Overloads Sub OnValueChanged(ByVal e As DependencyPropertyChangedEventArgs)

    End Sub


End Class

