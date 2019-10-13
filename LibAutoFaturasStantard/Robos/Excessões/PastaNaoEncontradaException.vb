<Serializable>
Public Class PastaNaoEncontradaException
    Inherits RoboFaturaException

    Public Sub New(fatura As Fatura, message As String, Optional innerException As Exception = Nothing)
        MyBase.New(fatura, message, False, innerException)
    End Sub
End Class

