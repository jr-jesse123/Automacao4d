Friend Class TotalMovelVivo
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "total a pagar ((\d+\.)\d+,\d+)"
    Public Overrides Property Modelo As ModeloPesquisa

    Public Overrides Sub ConstruirRelatorio()
        Stop
    End Sub
End Class
