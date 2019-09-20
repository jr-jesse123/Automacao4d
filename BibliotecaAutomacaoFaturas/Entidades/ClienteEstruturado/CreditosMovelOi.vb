Friend Class CreditosMovelOi
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "Descontos e Créditos -(\d+?,\d{2})"
    Public Overrides Property Modelo As ModeloPesquisa

    Public Overrides Sub ConstruirRelatorio()


        Dim linhaTotal = Relatorio.NewRow

        linhaTotal("Total") = Matches.First.Groups(1).Value

        Resultado = CType(Matches.First.Groups(1).Value, Double)



    End Sub
End Class
