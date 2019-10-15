Imports LibAutoFaturasStantard
Imports Xunit

Public Class TestesGeradorFaturas



    <Theory>
    <InlineData()>
    Public Sub TestarFaturasGeradas()
        Dim x As New GeradorFatura

        For Each conta In ContasTeste.CriarContas()
            x.GerarObjetoFaturaSeElegivel(conta)


        Next


    End Sub


End Class

Public Class ContasTeste
    Public Shared Function CriarContas() As List(Of Conta)
        Dim output As New List(Of Conta)


        For y = Today.Month - 1 To Today.Month


            For x = 1 To 30
                For Each operadora In [Enum].GetValues(GetType(OperadoraEnum))
                    Dim conta As New Conta With {.Operadora = operadora, .NrDaConta = Now.ToShortTimeString,
                    .Faturas = New List(Of Fatura) From {New Fatura With {.Vencimento = New Date(Today.Year, y)}},
                        .Vencimento = x}
                    output.Add(conta)
                Next
            Next
            Return output

        Next
    End Function

End Class