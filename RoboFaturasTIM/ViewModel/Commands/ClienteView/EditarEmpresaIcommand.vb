Imports BibliotecaAutomacaoFaturas

Public Class EditarEmpresaIcommand
    Implements ICommand

    Private editarClienteVM As EditarClienteVM

    Public Sub New(editarClienteVM As EditarClienteVM)
        Me.editarClienteVM = editarClienteVM
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Dim controle = CType(parameter, ControleEmpresa)

        Dim empresa As New Empresa With {
            .BitrixID = controle.IDBitrix.Text,
            .CNPJ = controle.CNPJ.Text,
            .Nome = controle.RazaoSocial.Text,
            .NomeFantasia = controle.NomeFantasia.Text,
            .Responsavel = New Responsavel With {.Nome = controle.ResponsavelNome.Text,
                                                .E_mail = controle.ResponsavelEmail.Text,
                                                .Telefone = controle.ResponsavelTelefone.Text},
            .HoldingID = controle.CNPJHOLDING.Text}

        Dim x As New GerenciadordeRelacionamentosMongo

        x.UpsertEmpresa(empresa)

    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
