Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas
Imports MongoDB.Bson.Serialization.Attributes

Public Enum ModeloPesquisa
    ResultadoUnico
    ResultadoSequencial
    ResultadoGlobal
End Enum

<BsonIgnoreExtraElements>
<BsonDiscriminator("TotalMovelTim")>
Public Class TotalMovelTim
    Implements IPesquisaRegex

    Public Property Padrao As String = "\d+.\d+,\d+|\d+,\d+" Implements IPesquisaRegex.Padrao
    <BsonIgnore>
    Public Property Matches As New List(Of Match) Implements IPesquisaRegex.Matches
    <BsonIgnore>
    Public Property Concluido As Boolean = False Implements IPesquisaRegex.Concluido
    Public Property ModeloPesquisa As ModeloPesquisa = ModeloPesquisa.ResultadoUnico Implements IPesquisaRegex.Modelo
    Public Property Relatorio As New DataTable("Total Da Fatura") Implements IPesquisaRegex.Relatorio
    <BsonIgnore>
    Public Property Iniciado As Boolean Implements IPesquisaRegex.Iniciado

    Public Sub ConstruirRelatorio() Implements IPesquisaRegex.ConstruirRelatorio

        Relatorio.Columns.Add("Valores", GetType(Double))

        For Each match As Match In Matches
            Dim valor = Relatorio.NewRow
            valor(0) = match.Value
            Relatorio.Rows.Add(valor)
        Next

    End Sub

End Class


'.Creditos = 0, .Encargos = 0, .Pendente = True, .ValorContestado = 0, .ValorOriginal = 0, .Vencimento = ""
<BsonIgnoreExtraElements>
<BsonDiscriminator("CreditosMovelTim")>
Public Class CreditosMovelTim
    Implements IPesquisaRegex

    Public Property Padrao As String = "Créd.Contest:.+" Implements IPesquisaRegex.Padrao
    <BsonIgnore>
    Public Property Matches As New List(Of Match) Implements IPesquisaRegex.Matches
    <BsonIgnore>
    Public Property Concluido As Boolean = False Implements IPesquisaRegex.Concluido
    Public Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoGlobal Implements IPesquisaRegex.Modelo
    Public Property Relatorio As New DataTable Implements IPesquisaRegex.Relatorio
    <BsonIgnore>
    Public Property Iniciado As Boolean = False Implements IPesquisaRegex.Iniciado

    Public Sub ConstruirRelatorio() Implements IPesquisaRegex.ConstruirRelatorio


        Try
            Relatorio.Columns.Add("Creditos", GetType(Double))
        Catch ex As NullReferenceException

            Relatorio = New DataTable
            Relatorio.Columns.Add("Creditos", GetType(Double))
        End Try



        For Each match As Match In Matches
            Dim valor = Relatorio.NewRow
            valor(0) = match.Value
            Relatorio.Rows.Add(valor)
        Next


    End Sub
End Class