Imports System.Data
Imports System.Text.RegularExpressions
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
<BsonDiscriminator(NameOf(TotalMovelClaro))>
Friend Class TotalMovelClaro
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "FF Total a Pagar R\$ (\d{0,10}.?\d+,\d{2}) FF|Total de Débitos R\$ (\d+,\d{2})"

    Public Overrides Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoUnico


    Public Overrides Sub ConstruirRelatorio()

        Relatorio.Columns.Add("Total", GetType(Double))

        Dim linhaTotal = Relatorio.NewRow

        Try
            linhaTotal("Total") = Matches.First.Groups(1).Value
            Resultado = CType(Matches.First.Groups(1).Value, Double)
        Catch ex As ArgumentException
            linhaTotal("Total") = Matches.First.Groups(2).Value
            Resultado = CType(Matches.First.Groups(2).Value, Double)
        End Try


    End Sub
End Class
