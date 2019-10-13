
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
<BsonDiscriminator(NameOf(CreditosMovelVivo))>
Friend Class CreditosMovelVivo
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "Créditos de Valores Contestados -((\d+\.)?\d+,\d+)|\(Crédito ou Débito\) -(\d+?\.?\d+,\d+)"
    Public Overrides Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoUnico

    Public Overrides Sub ConstruirRelatorio()


        For Each MATCH In Matches

            Dim valuestr As String = MATCH.Value.ToString
            Dim valueOutput As String = ""

            For x = valuestr.Count - 1 To 0 Step -1
                Dim [char] = valuestr.AsEnumerable(x)
                If Char.IsDigit([char]) Or [char] = "," Or [char] = "." Then
                    valueOutput = [char] + valueOutput

                ElseIf [char] = "," Then
                    valueOutput = "." + valueOutput
                ElseIf [char] = "." Then

                    Exit For
                End If
            Next

            Dim ResultadoInteger As Double = CType(valueOutput, System.Double)

            Resultado += ResultadoInteger
        Next

        'Stop
    End Sub


End Class
