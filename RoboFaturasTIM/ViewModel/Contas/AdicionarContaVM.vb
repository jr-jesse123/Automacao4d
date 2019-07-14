Imports BibliotecaAutomacaoFaturas

Public Class AdicionarContaVM

    Property Adicionar As New AdicionarEmpresaIcommand(Me)

    Property Conta As New Conta

    Public Sub AdicionarEmpresa(ContaControle As ControleConta)
        Dim x As New GerenciadordeRelacionamentosMongo


        Dim Conta As New Conta With {
            .ContaTriagemBitrixID = ContaControle.IdBitrix.Text,
            .Empresa = ContaControle.CbEmpepresa.SelectedItem,
            .NrDaConta = ContaControle.NrDaconta.Text,
            .Target = ContaControle.Target.Text,
            .Vencimento = ContaControle.Vencimento.Text,
            .Setor = ContaControle.Departamento.Text,
            .Pasta = ContaControle.Pasta.Text,
            .Drive = ContaControle.Drive.Text,
            .Operadora = ContaControle.OperadoraCB.SelectedIndex,
            .TipoDeConta = ContaControle.TipoDeConta.SelectedItem,
            .Subtipo = ContaControle.SubtipoCB.SelectedItem}


        x.UpsertConta(Conta)


    End Sub

End Class
