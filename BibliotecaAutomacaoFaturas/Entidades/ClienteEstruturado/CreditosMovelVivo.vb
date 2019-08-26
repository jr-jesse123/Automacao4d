Friend Class CreditosMovelVivo
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "Créditos de Valores Contestados -((\d+\.)?\d+,\d+)"
    Public Overrides Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoUnico

    Public Overrides Sub ConstruirRelatorio()
        Stop
    End Sub


End Class
