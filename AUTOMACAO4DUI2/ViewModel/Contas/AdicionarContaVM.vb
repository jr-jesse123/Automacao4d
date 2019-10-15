Imports System.ComponentModel.DataAnnotations
Imports AUTOMACAO4DUI2
Imports LibAutoFaturasStantard

Public Class AdicionarContaVM

    Property AdicionarContaICommand As New AdicionarcontaIcommand(Me)

    Property Empresas = GerRelDB.Empresas
    Property Gestores = GerRelDB.Gestores



    Public Sub AdicionarConta(conta As Conta)

        GerRelDB.AdicionarConta(conta)

    End Sub

End Class
