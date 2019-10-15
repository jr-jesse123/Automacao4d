Public Class DefinidorDeReferenciaDeFaturas


    Shared Function DescobrirReferencia(vencimento As Date) As String
#Disable Warning BC42024 ' Variável local não utilizada: "output".
        Dim output As Integer
#Enable Warning BC42024 ' Variável local não utilizada: "output".

        Return ReferenciaTimMovel(vencimento)

    End Function

    Private Shared Function ReferenciaTimMovel(vencimento As Date) As String

        Dim dia As Integer = vencimento.Day
        Dim mes As Integer = vencimento.Month
        Dim ano As Integer = vencimento.Year

        Dim output As String

        If dia > 17 Then
            output = mes.ToString("00") + (ano - 2000).ToString("00")
        Else
            If mes < 12 Then
                output = (mes - 1).ToString("00") + (ano - 2000).ToString("00")
            Else
                output = "01"
            End If
        End If

        Return output
    End Function
End Class
