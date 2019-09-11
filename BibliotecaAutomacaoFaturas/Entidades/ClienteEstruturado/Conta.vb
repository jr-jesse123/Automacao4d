Imports System.ComponentModel.DataAnnotations
Imports BibliotecaAutomacaoFaturas
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Class Conta '

    <BsonIgnore>
    Property Gestores As New List(Of Gestor)
    <BsonIgnore>
    Property Empresa As Empresa
    <Required>
    Property EmpresaID As String 'cnpj da empresa
    Property GestoresID As New List(Of String) ' cpf do gestor
    Property ContatosID As New List(Of String) ' cpf dos contatos
    Property Bloqueada As Boolean
    <Required>
    <Range(0, Double.MaxValue, ErrorMessage:="Target Invalido")>
    Property Target As Double
    <Required>
    <Range(0, 99999, ErrorMessage:="ID TRIAGEM INVÁLIDA")>
    Property ContaTriagemBitrixID As Integer
    <Required>
    <Range(1000, Double.MaxValue, ErrorMessage:="número de conta inválido")>
    Property NrDaConta As String
    Property Setor As String
    <Required>
    <Range(1, 30, ErrorMessage:="Vencimento inválido")>
    Property Vencimento As Integer
    Property Faturas As New List(Of Fatura)
    <Required>
    Property Pasta As String
    <Required>
    Property Drive As String
    Property Linhas As List(Of Linha)
    Property Fluxos As List(Of Fluxo)
    Property LogRobo As List(Of String)
    Property DadosOk As Boolean
    Property UltimoDowload As String
    <Required>
    Property Operadora As OperadoraEnum
    <Required>
    Property TipoDeConta As TipoContaEnum
    Property Subtipo As SubtipoEnum
    <BsonIgnore>
    Property DadosRegex As New DadosRegex
    Property GeradorFatura As New GeradorFatura



End Class
