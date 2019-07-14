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
    Public Property Creditos As Double
    Property Encargos As Double

End Class
