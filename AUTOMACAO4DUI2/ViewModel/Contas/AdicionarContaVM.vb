Imports AUTOMACAO4DUI2
Imports BibliotecaAutomacaoFaturas

Public Class AdicionarContaVM

    Property AdicionarContaICommand As New AdicionarcontaIcommand(Me)

    Property Empresas = GerRelDB.Empresas
    Property Gestores = GerRelDB.Gestores



    Public Sub AdicionarConta(conta As Conta)

        GerRelDB.UpsertConta(conta)

    End Sub

End Class
