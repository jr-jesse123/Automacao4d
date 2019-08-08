Imports System.ComponentModel.DataAnnotations
Imports BibliotecaAutomacaoFaturas
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Class Gestor
    <Required>
    <StringLength(11, MinimumLength:=11)>
    <Range(0, 99999999999)>
    Property CPF As String
    <RegularExpression(".@.")>
    Property Email As String
    <Required>
    <StringLength(11, MinimumLength:=11)>
    <Range(0, 99999999999)>
    Property LinhaMaster As String
    Property Login As String
    <Required>
    Property Nome As String
    Property SenhaContaOnline As String
    Property SenhaDeAtendimento As String
    Property BitrixID As String
End Class


