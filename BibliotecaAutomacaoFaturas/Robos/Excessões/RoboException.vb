﻿
Imports System.Runtime.Serialization

<Serializable>
Friend Class RoboFaturaException
    Inherits Exception

    Public Sub New()
    End Sub

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(fatura As Fatura, message As String, dadosok As Boolean)
        MyBase.New(message)

        GerRelDB.AtualizarContaComLog(fatura, message, dadosok)
    End Sub

    Public Sub New(fatura As Fatura, message As String)
        MyBase.New(message)

        GerRelDB.AtualizarContaComLog(fatura, message)
    End Sub

    Public Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class

