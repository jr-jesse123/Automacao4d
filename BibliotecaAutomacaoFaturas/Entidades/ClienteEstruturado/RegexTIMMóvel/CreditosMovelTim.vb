Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas
Imports MongoDB.Bson.Serialization.Attributes
'.Creditos = 0, .Encargos = 0, .Pendente = True, .ValorContestado = 0, .ValorOriginal = 0, .Vencimento = ""
<BsonIgnoreExtraElements>
<BsonDiscriminator(NameOf(CreditosMovelTim))>
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
    Public Property Resultado As Object Implements IPesquisaRegex.Resultado

    Public Sub ConstruirRelatorio() Implements IPesquisaRegex.ConstruirRelatorio

        Dim _resultado As Double

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

            _resultado += match.Value
        Next

        Resultado = CType(_resultado, Double)
    End Sub
End Class