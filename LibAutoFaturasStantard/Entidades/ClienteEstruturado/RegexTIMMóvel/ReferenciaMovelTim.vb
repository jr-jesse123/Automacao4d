Imports System.Text.RegularExpressions

Friend Class ReferenciaMovelTim
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "Período de Uso.+\sde [\d/]{10} a [\d/]{3}(\d{2}/\d{4})"

    Public Overrides Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoUnico

    Public Overrides Sub ConstruirRelatorio()

        Dim rawReferencia = Matches.First.Groups(1).Value

        Resultado = Renomear(rawReferencia)
    End Sub

    Private Function Renomear(rawReferencia As String) As String

        Dim PARTES = rawReferencia.Split("/")

        Select Case PARTES(0)
            Case "JAN"
                PARTES(0) = "01"
            Case "FEV"
                PARTES(0) = "01"
            Case "MAR"
                PARTES(0) = "01"
            Case "ABR"
                PARTES(0) = "01"
            Case "MAI"
                PARTES(0) = "01"
            Case "JUN"
                PARTES(0) = "01"
            Case "JUL"
                PARTES(0) = "01"
            Case "AGO"
                PARTES(0) = "01"
            Case "SET"
                PARTES(0) = "01"
            Case "OUT"
                PARTES(0) = "01"
            Case "NOV"
                PARTES(0) = "01"
            Case "DEZ"
                PARTES(0) = "01"
            Case Else
                Throw New RoboFaturaException("Não Foi possível identificar a refrencia da fatura")
        End Select

        Return PARTES(0) + PARTES(1)

    End Function
End Class
