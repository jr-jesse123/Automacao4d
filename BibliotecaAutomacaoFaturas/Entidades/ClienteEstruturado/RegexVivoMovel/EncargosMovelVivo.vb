Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
<BsonDiscriminator(NameOf(EncargosMovelVivo))>
Friend Class EncargosMovelVivo
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "Encargos Financeiros (\d+?\.?\d+,\d+)"
    Public Overrides Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoSequencial

    Public Overrides Sub ConstruirRelatorio()
        For Each MATCH In Matches
            Resultado += MATCH.Groups(1).Value * 1
        Next



    End Sub
End Class
