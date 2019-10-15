Imports System.Data
Imports System.Text.RegularExpressions
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports MongoDB.Bson.Serialization.Attributes
'.Creditos = 0, .Encargos = 0, .Pendente = True, .ValorContestado = 0, .ValorOriginal = 0, .Vencimento = ""
<BsonIgnoreExtraElements>
<BsonDiscriminator(NameOf(CreditosMovelTim))>
Public Class CreditosMovelTim
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "Créd.Contest:.+(\d+,\d{2})"

    Public Overrides Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoGlobal

    Public Overrides Sub ConstruirRelatorio()

        Dim _resultado As Double

        Try
            Relatorio.Columns.Add("Creditos", GetType(Double))
        Catch ex As NullReferenceException

            Relatorio = New DataTable
            Relatorio.Columns.Add("Creditos", GetType(Double))
        End Try



        For Each match As Match In Matches
            Dim valor = Relatorio.NewRow
            valor(0) = match.Groups(1).Value
            Relatorio.Rows.Add(valor)

            _resultado += match.Groups(1).Value
        Next

        Resultado = CType(_resultado, Double)
    End Sub
End Class