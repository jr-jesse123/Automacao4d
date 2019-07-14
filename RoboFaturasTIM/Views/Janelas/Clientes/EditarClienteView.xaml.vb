Imports BibliotecaAutomacaoFaturas

Public Class EditarClienteView
    Property Empresa As Empresa



    Sub New(empresa As Empresa)

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().


        Me.empresa = empresa

        If empresa IsNot Nothing Then
            If empresa.CNPJ IsNot Nothing Then Controle.CNPJ.Text = empresa.CNPJ
            Controle.IDBitrix.Text = empresa.BitrixID
            If empresa.Nome IsNot Nothing Then Controle.RazaoSocial.Text = empresa.Nome
            If empresa.NomeFantasia IsNot Nothing Then Controle.NomeFantasia.Text = empresa.NomeFantasia
        End If

        If empresa.Responsavel IsNot Nothing Then

            If empresa.Responsavel.Nome IsNot Nothing Then Controle.ResponsavelNome.Text = empresa.Responsavel.Nome
            If empresa.Responsavel.E_mail IsNot Nothing Then Controle.ResponsavelEmail.Text = empresa.Responsavel.E_mail
            If empresa.Responsavel.Telefone IsNot Nothing Then Controle.ResponsavelTelefone.Text = empresa.Responsavel.Telefone
        End If


    End Sub


End Class
