﻿Imports System.Text.RegularExpressions

Public Interface IPesquisaRegex
    Property Padrao As String
    Property Matches As List(Of Match)
    Property Concluido As Boolean
    Property Modelo As ModeloPesquisa
    Property Relatorio As DataTable

    Property Iniciado As Boolean

    Sub ConstruirRelatorio()
End Interface

