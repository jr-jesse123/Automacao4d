Imports System.ComponentModel.DataAnnotations
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Class Gestor
    <Required>
    <StringLength(11, MinimumLength:=11)>
    <Range(0, 99999999999)>
    Property CPF As String
    <RegularExpression(".+@.+")>
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

    Property ListaSenhas As New List(Of DadosDeAcesso)
End Class

