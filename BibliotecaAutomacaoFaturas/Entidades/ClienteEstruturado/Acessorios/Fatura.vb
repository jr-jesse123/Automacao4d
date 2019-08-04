Imports BibliotecaAutomacaoFaturas
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Class Fatura

    Property Vencimento As Date
    Property ValorOriginal As Double
    Property ValorContestado As Double
    Property Pendente As Boolean
    Property Conferida As Boolean
    Property EmContestacao As Boolean
    Property Aprovada As Boolean
    Property ValorAContestar As Double
    Property Creditos As Double
    Property Encargos As Double
    Property LogRobo As New List(Of String)
    Property Baixada As Boolean
    Property NrConta As Integer
    Property NrContaAnterior As Integer
    Public Property Referencia As String
    Property Relatorios As New List(Of IPesquisaRegex)
End Class
