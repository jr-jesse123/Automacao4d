Imports System.Reflection
Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
<BsonDiscriminator(NameOf(CreditosMovelClaro))>
Friend Class CreditosMovelClaro
    Implements IPesquisaRegex


    Public Property Padrao As String = "F \d{2}/\d{4} - .+ R\$ -(\d{0,10}.?\d+,\d){2} F" Implements IPesquisaRegex.Padrao
    Public Property Matches As New List(Of Match) Implements IPesquisaRegex.Matches
    Public Property Concluido As Boolean = False Implements IPesquisaRegex.Concluido
    Public Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoSequencial Implements IPesquisaRegex.Modelo
    Public Property Relatorio As New DataTable Implements IPesquisaRegex.Relatorio
    Public Property Iniciado As Boolean = False Implements IPesquisaRegex.Iniciado
    Public Property Resultado As Object Implements IPesquisaRegex.Resultado

    Public Sub ConstruirRelatorio() Implements IPesquisaRegex.ConstruirRelatorio

        Relatorio.Columns.Add("Créditos", GetType(Double))

        Dim total As Double
        For Each mach In Matches
            Dim linhaCredito = Relatorio.NewRow
            linhaCredito("Créditos") = mach.Groups(1).Value
            total += mach.Groups(1).Value

            Relatorio.Rows.Add(total)
        Next

        Dim linhaTotal = Relatorio.NewRow
        linhaTotal("Créditos") = total

        Resultado = CType(total, Double)
    End Sub
End Class
