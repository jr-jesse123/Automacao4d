Imports BibliotecaAutomacaoFaturas

Public Class EditarGestorView

    Sub New(gestor As Gestor)

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        CtrGestor.Gestor = gestor

        ' Adicione qualquer inicialização após a chamada InitializeComponent().

        'CtrGestor.txtBoxNomeGestor.Text = gestor.Nome
        'CtrGestor.txtBoxEmailGestor.Text = gestor.Email
        'CtrGestor.txtBoxLinhaMasterGestor.Text = gestor.LinhaMaster

        'CtrGestor.txtBoxSenhaAtendimentoGestor.Text = gestor.SenhaContaOnline
        'CtrGestor.txtBoxCPFGestor.Text = gestor.CPF
        'CtrGestor.txtBoxIdBitrix.Text = gestor.BitrixID

        'CtrGestor.ListaSenhas.ItemsSource = gestor.ListaSenhas



    End Sub

End Class
