Imports System.ComponentModel.DataAnnotations
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
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
    <Obsolete("A senha utilizada diretamente pela empresa está obsoleta, por favor utiliza a listaSenhas filtrando pela operadora que deseja buscar")>
    Property LoginContaOnline As String

    ' apropriedade abaixo precisa permanecer com o nome SEnha com o E maiusculo para poder fucnioanr com o banco de dados
    <Obsolete("A senha utilizada diretamente pela empresa está obsoleta, por favor utiliza a listaSenhas filtrando pela operadora que deseja buscar")>
    Property SEnhaContaOnline As String



End Class
