Imports LibAutoFaturasStantard

Public Class AdicionarClienteVM

    Property Adicionar As New AdicionarEmpresaIcommand(Me)

    Public Sub AdicionarEmpresa(empresa As Empresa)

        GerRelDB.AdicionarEmpresa(empresa)



    End Sub

End Class
