
Imports MongoDB.Bson.Serialization.Attributes

<BsonDiscriminator(NameOf(TotalMovelOi))>
Friend Class TotalMovelOi
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "TOTAL DA SUA FATURA (\d+?,\d{2})"
    Public Overrides Property Modelo As ModeloPesquisa = Modelo.ResultadoUnico

    Public Overrides Sub ConstruirRelatorio()

        Dim linhaTotal = Relatorio.NewRow

        'linhaTotal("Total") = Matches.First.Groups(1).Value

        Resultado = CType(Matches.First.Groups(1).Value, Double)


    End Sub
End Class
