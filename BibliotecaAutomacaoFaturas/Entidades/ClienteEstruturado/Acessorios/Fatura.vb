﻿
Imports BibliotecaAutomacaoFaturas
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Class Fatura
    Private _Referencia As String = ""
    Property Vencimento As Date
    Property Ajuste As Double
    Property Total As Double
    Property ValorContestado As Double
    Property Pendente As Boolean
    Property Conferida As Boolean
    Property EmContestacao As Boolean
    Property Aprovada As Boolean
    Property ValorAContestar As Double
    Property Creditos As Double
    Property Encargos As Double
    Property LogRobo As New List(Of String)
    Property Tratada As Boolean
    Property Baixada As Boolean
    Property NrConta As String
    Property NrDaContaAnterior As String
    Property Relatorios As New List(Of IPesquisaRegex)
    Property RelatoriosExcel As New RelatorioCsv
    Property InfoDownloads As New List(Of InfoDownload)
    Property FaturaEnviadaParaDrive As Boolean
    Property FaturaPosicionadaNaPasta As Boolean
    Property FluxoDisparado As Boolean
    Property FaturaConvertida As Boolean
    Property FaturaProcessadaFox As Boolean
    Property FaturaExraida As Boolean
    Property RelatoriosUpadosDrive As Boolean
    Property RelatoriosEnviadosWebapp As Boolean

    Public Property Referencia As String
        Get
            If Referencia Is Nothing Then
                Return DefinidorDeReferenciaDeFaturas.DescobrirReferencia(Vencimento)
            End If

            If _Referencia.Length > 0 Then
                Return _Referencia
            Else
                Return DefinidorDeReferenciaDeFaturas.DescobrirReferencia(Vencimento)
            End If
        End Get
        Set
            _Referencia = Value
        End Set
    End Property


End Class

