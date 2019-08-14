Imports System.ComponentModel.DataAnnotations
Imports BibliotecaAutomacaoFaturas
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
    <Required>
    Property Nome As String
    Property NomeFantasia As String
    <Required>
    <Range(1, 1000)>
    Property BitrixID As Integer
    <Required>
    <Range(1, 99999999999999)>
    <StringLength(14, MinimumLength:=14)>
    Property CNPJ As String
    Property Contatos As List(Of Contato)
    Property Contratante As Boolean

    Property ListaSenhas As New List(Of DadosDeAcesso)

    Property LoginContaOnline As String
    Property SenhaContaOnline As String


End Class
