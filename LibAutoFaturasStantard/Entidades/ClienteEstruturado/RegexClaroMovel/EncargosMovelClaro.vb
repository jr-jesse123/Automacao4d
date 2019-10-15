Imports System.Data
Imports System.Text.RegularExpressions
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
<BsonDiscriminator("EncargosMovelClaro")>
Friend Class EncargosMovelClaro
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "Juros e Multa R\$ (\d{0,10}.?\d+,\d{2})"
    Public Overrides Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoUnico

    Public Overrides Sub ConstruirRelatorio()

        Relatorio.Columns.Add("JurosMulta", GetType(Double))

        If Matches.Count = 1 Then
            Dim LinhaJM = Relatorio.NewRow

            LinhaJM("JurosMulta") = Matches.First.Groups(1).Value

            Relatorio.Rows.Add(LinhaJM)
            Resultado = CType(Matches.First.Groups(1).Value, Double)
        End If



    End Sub
End Class
