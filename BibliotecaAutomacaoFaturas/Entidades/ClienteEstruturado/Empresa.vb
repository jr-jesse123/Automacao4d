Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Class Empresa
    '<BsonId> 
    'Property _id As ObjectId
    <BsonIgnore>
    Property Filiais As New List(Of Empresa)
    <BsonIgnore>
    Property Contas As New List(Of Conta)
    <BsonIgnore>
    Property Gestores As List(Of Gestor)
    Property HoldingID As String
    Property Nome As String
    Property NomeFantasia As String
    Property BitrixID As Integer
    Property CNPJ As String
    Property Contatos As List(Of Contato)
    Property Contratante As Boolean
    Property LoginContaOnline As String
    Property SEnhaContaOnline As String

End Class
