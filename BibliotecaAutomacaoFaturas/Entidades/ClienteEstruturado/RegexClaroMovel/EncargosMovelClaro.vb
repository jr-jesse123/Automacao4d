Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
<BsonDiscriminator("EncargosMovelClaro")>
Friend Class EncargosMovelClaro
    Implements IPesquisaRegex

    Public Property Padrao As String = "Juros e Multa R\$ (\d{0,10}.?\d+,\d{2})" Implements IPesquisaRegex.Padrao
    Public Property Matches As New List(Of Match) Implements IPesquisaRegex.Matches
    Public Property Concluido As Boolean = False Implements IPesquisaRegex.Concluido
    Public Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoUnico Implements IPesquisaRegex.Modelo
    Public Property Relatorio As New DataTable Implements IPesquisaRegex.Relatorio
    Public Property Iniciado As Boolean = False Implements IPesquisaRegex.Iniciado
    Public Property Resultado As Object Implements IPesquisaRegex.Resultado

    Public Sub ConstruirRelatorio() Implements IPesquisaRegex.ConstruirRelatorio

        Relatorio.Columns.Add("JurosMulta", GetType(Double))

        If Matches.Count = 1 Then
            Dim LinhaJM = Relatorio.NewRow

            LinhaJM("JurosMulta") = Matches.First.Groups(1).Value

            Relatorio.Rows.Add(LinhaJM)

        End If


        Resultado = CType(Matches.First.Groups(1).Value, Double)
    End Sub
End Class
