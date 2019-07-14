Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Class Conta '

    <BsonIgnore>
    Property Gestores As New List(Of Gestor)
    <BsonIgnore>
    Property Empresa As Empresa
    Property EmpresaID As String 'cnpj da empresa
    Property GestoresID As New List(Of String) ' cpf do gestor
    Property ContatosID As New List(Of String) ' cpf dos contatos
    Property Bloqueada As Boolean
    Property Baixada As Boolean
    Property Target As Double
    Property ContaTriagemBitrixID As Integer
    Property NrDaConta As String
    Property Setor As String
    Property Vencimento As Integer
    Property Faturas As List(Of Fatura)
    Property Pasta As String
    Property Drive As String
    Property Linhas As List(Of Linha)
    Property Fluxos As List(Of Fluxo)
    Property Log As List(Of String)
    Property DadosOk As Boolean
    Property UltimoDowload As String
    Property Operadora As OperadoraEnum
    Property TipoDeConta As TipoContaEnum
    Property Subtipo As SubtipoEnum

End Class

