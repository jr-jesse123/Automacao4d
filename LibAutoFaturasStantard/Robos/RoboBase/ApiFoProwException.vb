Imports System.Runtime.Serialization

<Serializable>
Friend Class ApiFoProwException
    Inherits RoboFaturaException

    Public Sub New(fatura As Fatura, message As String, Optional innerException As Exception = Nothing)
        MyBase.New(fatura, message, False, innerException)
    End Sub

    Public Sub New(conta As Conta, message As String)
        MyBase.New(conta, message, False)
    End Sub

End Class
