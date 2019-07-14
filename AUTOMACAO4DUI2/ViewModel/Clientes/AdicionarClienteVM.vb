Imports BibliotecaAutomacaoFaturas

Public Class AdicionarClienteVM

    Property Adicionar As New AdicionarEmpresaIcommand(Me)

    Public Sub AdicionarEmpresa(empresa As Empresa)

        GerRelDB.UpsertEmpresa(empresa)



    End Sub

End Class
