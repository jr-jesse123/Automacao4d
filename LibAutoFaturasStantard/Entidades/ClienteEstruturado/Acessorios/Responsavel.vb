Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Class Contato
    Property Nome As String
    Property Telefone As String
    Property E_mail As String
    Property Contas As List(Of String)
End Class
