Imports System.Text.RegularExpressions

Public Class Regexer
    Property Padroes As New List(Of IPesquisaRegex)


    Public Function PesquisarTexto(padraoRegex As String, texto As String) As Match()

        Dim verifica As New Regex(padraoRegex)
        Dim nrResultados = verifica.Matches(texto).Count
        Dim resultados(verifica.Matches(texto).Count - 1) As Match
        verifica.Matches(texto, padraoRegex).CopyTo(resultados, 0)
        Return resultados

    End Function


    Public Sub RealizarRegex(Texto As String)

        For Each padrao In Padroes
            If padrao.Concluido = False Then

                Dim resultados = PesquisarTexto(padrao.Padrao, Texto)

                'retirar esta linha pois ela corrige um erro de construção das primeiras contas
                If padrao.Matches Is Nothing Then padrao.Matches = New List(Of Match)
                '***************************************************

                padrao.Matches.AddRange(resultados)

                If resultados.Count > 0 Then padrao.Iniciado = True

                If padrao.Modelo = ModeloPesquisa.ResultadoUnico And resultados.Count > 0 Then
                    padrao.Concluido = True

                ElseIf padrao.Iniciado = True And resultados.Count = 0 And padrao.Modelo = ModeloPesquisa.ResultadoSequencial Then
                    padrao.Concluido = True

                End If

            End If

        Next
    End Sub


    Public Sub SetarPadores(Padroes As List(Of IPesquisaRegex))
        Me.Padroes.Clear()
        Me.Padroes = Padroes



    End Sub

End Class
