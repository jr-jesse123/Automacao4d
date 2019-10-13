Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas

Friend Class ReferenciaMovelClaro
    Implements IPesquisaRegex

    Public Property Padrao As String = "Período de Uso.+\sde [\d/]{10} a [\d/]{3}(\d{2}/\d{4})" Implements IPesquisaRegex.Padrao
    Public Property Matches As New List(Of Match) Implements IPesquisaRegex.Matches
    Public Property Concluido As Boolean = False Implements IPesquisaRegex.Concluido
    Public Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoUnico Implements IPesquisaRegex.Modelo
    Public Property Relatorio As New DataTable Implements IPesquisaRegex.Relatorio
    Public Property Iniciado As Boolean = False Implements IPesquisaRegex.Iniciado
    Public Property Resultado As Object Implements IPesquisaRegex.Resultado

    Public Sub ConstruirRelatorio() Implements IPesquisaRegex.ConstruirRelatorio

        Dim rawReferencia = Matches.First.Groups(1).Value

        Resultado = rawReferencia.Substring(0, 2) + rawReferencia.Substring(5, 2)

    End Sub
End Class
