Imports BibliotecaAutomacaoFaturas

Public Class EditarGestorView
    Sub New(gestor As Gestor)

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().

        CtrGestor.txtBoxNomeGestor.Text = gestor.Nome
        CtrGestor.txtBoxEmailGestor.Text = gestor.Email
        CtrGestor.txtBoxLinhaMasterGestor.Text = gestor.LinhaMaster

        CtrGestor.txtBoxSenhaAtendimentoGestor.Text = gestor.SenhaContaOnline
        CtrGestor.txtBoxCPFGestor.Text = gestor.CPF
        CtrGestor.txtBoxIdBitrix.Text = gestor.BitrixID



    End Sub

End Class
