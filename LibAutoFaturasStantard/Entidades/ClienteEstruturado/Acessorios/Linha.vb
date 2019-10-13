Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Class Linha

    Property Numero As String
    Property Usuario As String
    Property Cargo As String
    Property Setor As String
    Property PLano As String
    Property Dados As Double
    Property ValorEsperado As Double
    Property Pacotes As List(Of String)

End Class
