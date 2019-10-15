Imports LibAutoFaturasStantard

Public Class AdicionarSenhaView
    Private _empresa As Empresa
    Private _gestor As Gestor
    Public Event SenhasAlteradas()

    Sub New(empresa As Empresa)

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().

        _empresa = empresa

    End Sub

    Sub New(gestor As Gestor)

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().

        _gestor = gestor

    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

        Dim DadosDeAcesso As New DadosDeAcesso With {.Login = ControleDAdosDeACesso.Login.Text,
            .Senha = ControleDAdosDeACesso.Senha.Text,
            .Operadora = ControleDAdosDeACesso.OperadoraCB.SelectedItem,
            .Tipo = ControleDAdosDeACesso.TipoDeContaCB.SelectedItem}


        If _empresa IsNot Nothing Then
            _empresa.ListaSenhas.Add(DadosDeAcesso)
        ElseIf _gestor IsNot Nothing Then

            If _gestor.ListaSenhas Is Nothing Then
                _gestor.ListaSenhas = New List(Of DadosDeAcesso)
            End If

            _gestor.ListaSenhas.Add(DadosDeAcesso)



        End If

        RaiseEvent SenhasAlteradas()
    End Sub
End Class
