
Imports MongoDB.Bson.Serialization.Attributes

<BsonDiscriminator(NameOf(TotalMovelOi))>
Friend Class TotalMovelOi
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "TOTAL DA SUA FATURA (\d+?,\d{2})"
#Disable Warning BC42025 ' Acesso de membro compartilhado, membro constante, membro enum ou tipo aninhado por meio de uma instância; a expressão de qualificação não será avaliada.
    Public Overrides Property Modelo As ModeloPesquisa = Modelo.ResultadoUnico
#Enable Warning BC42025 ' Acesso de membro compartilhado, membro constante, membro enum ou tipo aninhado por meio de uma instância; a expressão de qualificação não será avaliada.

    Public Overrides Sub ConstruirRelatorio()

        Dim linhaTotal = Relatorio.NewRow

        'linhaTotal("Total") = Matches.First.Groups(1).Value

        Resultado = CType(Matches.First.Groups(1).Value, Double)


    End Sub
End Class
