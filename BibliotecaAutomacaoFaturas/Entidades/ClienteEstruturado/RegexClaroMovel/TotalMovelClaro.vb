Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
<BsonDiscriminator(NameOf(TotalMovelClaro))>
Friend Class TotalMovelClaro
    Implements IPesquisaRegex

    Public Property Padrao As String = "FF Total a Pagar R\$ (\d{0,10}.?\d+,\d{2}) FF" Implements IPesquisaRegex.Padrao
    Public Property Matches As New List(Of Match) Implements IPesquisaRegex.Matches
    Public Property Concluido As Boolean = False Implements IPesquisaRegex.Concluido
    Public Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoUnico Implements IPesquisaRegex.Modelo
    Public Property Relatorio As New DataTable Implements IPesquisaRegex.Relatorio
    Public Property Iniciado As Boolean = False Implements IPesquisaRegex.Iniciado
    Public Property Resultado As Object Implements IPesquisaRegex.Resultado

    Public Sub ConstruirRelatorio() Implements IPesquisaRegex.ConstruirRelatorio

        Relatorio.Columns.Add("Total", GetType(Double))

        Dim linhaTotal = Relatorio.NewRow

        linhaTotal("Total") = Matches.First.Groups(1).Value


        Resultado = CType(Matches.First.Groups(1).Value, Double)

    End Sub
End Class
