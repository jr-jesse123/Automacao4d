Imports LibAutoFaturasStantard


Public Class EditarContaVM
    Public Property AtualizarICommand As New AtualizarIcommand(Me)


    Public Sub AtualizarConta(conta As Conta)

        GerRelDB.UpsertConta(conta)


    End Sub
End Class
