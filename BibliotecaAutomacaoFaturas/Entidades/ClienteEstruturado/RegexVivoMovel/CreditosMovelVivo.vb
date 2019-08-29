
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
<BsonDiscriminator(NameOf(CreditosMovelVivo))>
Friend Class CreditosMovelVivo
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "Créditos de Valores Contestados -((\d+\.)?\d+,\d+)|\(Crédito ou Débito\) -(\d+?\.?\d+,\d+)"
    Public Overrides Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoUnico

    Public Overrides Sub ConstruirRelatorio()

        For Each MATCH In Matches
            Resultado += MATCH.Groups(1).Value * 1
        Next



        'Stop
    End Sub


End Class
