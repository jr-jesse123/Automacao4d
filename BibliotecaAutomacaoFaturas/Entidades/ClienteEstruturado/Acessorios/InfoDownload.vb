Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Class InfoDownload
    Public Property Tratada As Boolean
    Public Property path As String
    Public Property tipoArquivo As ArquivoEnum
    Public Property vencimento As Date
    Public Property nrConta As String
    Public Property operadora As OperadoraEnum
    Public Property tipoConta As TipoContaEnum

End Class
