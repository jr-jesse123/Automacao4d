

Imports System.Text.RegularExpressions

Public MustInherit Class PesquisaRegexBase
    Implements IPesquisaRegex
    Public MustOverride Property Padrao As String Implements IPesquisaRegex.Padrao
    Public Property Matches As New List(Of Match) Implements IPesquisaRegex.Matches
    Public Property Concluido As Boolean = False Implements IPesquisaRegex.Concluido
    Public MustOverride Property Modelo As ModeloPesquisa Implements IPesquisaRegex.Modelo
    Public Property Relatorio As New DataTable Implements IPesquisaRegex.Relatorio
    Public Property Iniciado As Boolean = False Implements IPesquisaRegex.Iniciado
    Public Property Resultado As Object Implements IPesquisaRegex.Resultado

    Public MustOverride Sub ConstruirRelatorio() Implements IPesquisaRegex.ConstruirRelatorio
End Class
