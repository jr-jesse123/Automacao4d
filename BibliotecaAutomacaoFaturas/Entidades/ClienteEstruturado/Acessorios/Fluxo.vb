Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Class Fluxo
    Property Estado As EstadosFluxo
    Property Observacoes As List(Of String)
    Property Autorizado As Boolean?
End Class

