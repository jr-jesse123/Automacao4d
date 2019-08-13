Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas
Imports MongoDB.Bson.Serialization.Attributes



<BsonIgnoreExtraElements>
<BsonDiscriminator(NameOf(TotalMovelTim))>
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
    Public Property Resultado As Object Implements IPesquisaRegex.Resultado

    Public Sub ConstruirRelatorio() Implements IPesquisaRegex.ConstruirRelatorio

        Relatorio.Columns.Add("Valores", GetType(Double))

        For Each match As Match In Matches
            Dim valor = Relatorio.NewRow
            valor(0) = match.Value
            Relatorio.Rows.Add(valor)
        Next

        Resultado = CType(Matches.First.Value, Double)

    End Sub



End Class
