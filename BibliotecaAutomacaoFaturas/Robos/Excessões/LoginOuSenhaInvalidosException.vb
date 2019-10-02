Imports System.Runtime.Serialization

<Serializable>
Friend Class LoginOuSenhaInvalidosException
    Inherits RoboFaturaException

    Public Sub New(fatura As Fatura, message As String)
        MyBase.New(fatura, message, False)
    End Sub

    Public Sub New(gestor As Gestor, message As String, operadora As OperadoraEnum, tipo As TipoFaturaEnum)
        MyBase.New(gestor, message, operadora, tipo, False)
    End Sub


    Public Sub New(conta As Conta, message As String)
        MyBase.New(conta, message, False)
    End Sub

    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class
