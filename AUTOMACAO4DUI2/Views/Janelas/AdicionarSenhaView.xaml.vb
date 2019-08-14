Imports BibliotecaAutomacaoFaturas

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

        If _empresa IsNot Nothing Then
            _empresa.ListaSenhas.Add(ControleDAdosDeACesso.DadosDeAcesso)
            GerRelDB.UpsertEmpresa(_empresa)
        ElseIf _gestor IsNot Nothing Then
            _gestor.ListaSenhas.Add(ControleDAdosDeACesso.DadosDeAcesso)
            GerRelDB.UpsertGestor(_gestor)
        End If

        RaiseEvent SenhasAlteradas()
    End Sub
End Class
