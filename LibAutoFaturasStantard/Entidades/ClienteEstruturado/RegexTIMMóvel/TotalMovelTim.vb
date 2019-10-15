Imports System.Text.RegularExpressions
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports MongoDB.Bson.Serialization.Attributes



<BsonIgnoreExtraElements>
<BsonDiscriminator(NameOf(TotalMovelTim))>
Public Class TotalMovelTim
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "\d+.\d+,\d+|\d+,\d+"
    Public Overrides Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoUnico

    Public Overrides Sub ConstruirRelatorio()

        Relatorio.Columns.Add("Valores", GetType(Double))

        For Each match As Match In Matches
            Dim valor = Relatorio.NewRow
            valor(0) = match.Value
            Relatorio.Rows.Add(valor)
        Next

        Resultado = CType(Matches.First.Value, Double)

    End Sub



End Class
