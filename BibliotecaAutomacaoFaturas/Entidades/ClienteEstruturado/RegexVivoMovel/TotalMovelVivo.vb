﻿Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
<BsonDiscriminator("TotalMovelVivo")>
Friend Class TotalMovelVivo
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "\s(\d+?\.?\d{1,3},\d{2})"
    Public Overrides Property Modelo As ModeloPesquisa

    Public Overrides Sub ConstruirRelatorio()

        Dim output = Matches(1).Value.Substring(1, Matches(1).Value.Count - 1)

        Resultado = CType(output, System.Double)
        Stop
    End Sub
End Class
