Friend Class EncargosMovelVivo
    Inherits PesquisaRegexBase

    Public Overrides Property Padrao As String = "\(Crédito ou Débito\) -((\d+\.)?\d+,\d+)"
    Public Overrides Property Modelo As ModeloPesquisa = ModeloPesquisa.ResultadoUnico

    Public Overrides Sub ConstruirRelatorio()

        Stop

    End Sub
End Class
