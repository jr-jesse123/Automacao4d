Imports System.Data
Imports System.Text.RegularExpressions

Friend Class ReferenciaMovelClaro
    Inherits PesquisaRegexBase
    Public Overrides Property Padrao As String = "Período de Uso.+\sde [\d/]{10} a [\d/]{3}(\d{2}/\d{4})"
    Public Overrides Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoUnico


    Public Overrides Sub ConstruirRelatorio()

        Dim rawReferencia = Matches.First.Groups(1).Value

        Resultado = rawReferencia.Substring(0, 2) + rawReferencia.Substring(5, 2)

    End Sub
End Class
