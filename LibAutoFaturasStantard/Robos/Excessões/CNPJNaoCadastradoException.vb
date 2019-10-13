Imports System.Runtime.Serialization

<Serializable>
Public Class CNPJNaoCadastradoException
    Inherits RoboFaturaException

    Public Sub New(fatura As Fatura, message As String)
        MyBase.New(fatura, message, False)
    End Sub

    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class

