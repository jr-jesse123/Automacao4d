Imports System.Data

Public Class RelatorioCsv
    Inherits DataTable

    Sub New()

    End Sub
    Sub New(ListaEventos As List(Of String()))

        For i = 0 To ListaEventos.Count - 1
            If i = 0 Then
                For Each coluna In ListaEventos(0)
                    Dim datatype As Type = ObterTipoDaColuna(coluna)
                    Me.Columns.Add(coluna, datatype)
                Next
            Else

                Dim linha = Me.NewRow
                For z = 0 To ListaEventos(i).Count - 1

                    If ListaEventos(i)(z).Length > 0 Then
                        linha(ListaEventos(0)(z)) = ListaEventos(i)(z)
                    End If
                Next
                Me.Rows.Add(linha)
            End If


        Next

    End Sub

    Private Function ObterTipoDaColuna(coluna As String) As Type

        Select Case coluna
            Case "EMPRESA"
                Return GetType(String)
            Case "TELEFONE"
                Return GetType(String)
            Case "PLANO"
                Return GetType(String)
            Case "G_TIPO"
                Return GetType(String)
            Case "COD_SERVICO"
                Return GetType(String)
            Case "GRUPO"
                Return GetType(String)
            Case "DATA"
                Return GetType(Date)
            Case "HORARIO"
                Return GetType(TimeSpan)
            Case "TEMPO"
                Return GetType(Double)
            Case "LOCAL_ORIGEM"
                Return GetType(String)
            Case "LOCAL_DESTINO"
                Return GetType(String)
            Case "DESTINO"
                Return GetType(String)
            Case "TARIFA"
                Return GetType(String)
            Case "VALOR_BRUTO"
                Return GetType(Double)
            Case "ALIQ"
                Return GetType(Double)
            Case "NOTA_FISCAL"
                Return GetType(String)
            Case "BILL_REF_NO"
                Return GetType(String)
            Case "FROM_DATE"
                Return GetType(Date)
            Case "TO_DATE"
                Return GetType(Date)
            Case "ID_CLIENTE"
                Return GetType(String)
        End Select


#Disable Warning BC42105 ' Função "ObterTipoDaColuna" não retorna um valor em todos os caminhos de código. Uma exceção de referência nula pode ocorrer em tempo de execução quando o resultado é usado.
    End Function
#Enable Warning BC42105 ' Função "ObterTipoDaColuna" não retorna um valor em todos os caminhos de código. Uma exceção de referência nula pode ocorrer em tempo de execução quando o resultado é usado.
End Class