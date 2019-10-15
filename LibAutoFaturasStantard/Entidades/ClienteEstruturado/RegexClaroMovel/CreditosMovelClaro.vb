Imports System.Reflection
Imports System.Text.RegularExpressions
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
<BsonDiscriminator(NameOf(CreditosMovelClaro))>
Friend Class CreditosMovelClaro
    Inherits PesquisaRegexBase


    Public Overrides Property Padrao As String = "F \d{2}/\d{4} - .+ R\$ -(\d{0,10}.?\d+,\d){2} F"
    Public Overrides Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoSequencial

    Public Overrides Sub ConstruirRelatorio()

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
