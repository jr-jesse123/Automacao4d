Imports BibliotecaAutomacaoFaturas

Public Class AdicionarContaVM

    Property Adicionar As New AdicionarEmpresaIcommand(Me)

    Public Sub AdicionarEmpresa(cliente As ControleEmpresa)
        Dim x As New GerenciadordeRelacionamentosMongo



        Dim Empresa As New Empresa With {
            .BitrixID = cliente.IDBitrix.Text,
            .Nome = cliente.RazaoSocial.Text,
            .NomeFantasia = cliente.NomeFantasia.Text,
            .CNPJ = cliente.CNPJ.Text,
            .Responsavel = New Responsavel With {.Nome = cliente.ResponsavelNome.Text,
                                                .E_mail = cliente.ResponsavelEmail.Text,
                                                .Telefone = cliente.ResponsavelTelefone.Text},
            .HoldingID = cliente.CNPJHOLDING.Text}

        x.UpsertEmpresa(Empresa)


    End Sub

End Class
