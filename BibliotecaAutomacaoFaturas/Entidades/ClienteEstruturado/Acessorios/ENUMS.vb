Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Enum OperadoraEnum
    CLARO = 1
    TIM = 2
    OI = 3
    NEXTEL = 4
    VIVO = 5
    ALGAR = 6
End Enum

<BsonIgnoreExtraElements>
Public Enum SubtipoEnum
    Analogico = 110
    TroncoE1 = 111
    TroncoSIP = 112

    Celular = 121
    Dados = 122
    M2M = 123
    CelularEDados = 124
    FixoGSM = 125

    ADSL = 131
    SemiDedicado = 132
    Dedicado = 133
    VPN = 134
    MPLS = 135
    VozFixa = 136
    SlnVoz = 137
    Internet = 138
    InternetCorp = 139


End Enum

<BsonIgnoreExtraElements>
Public Enum EstadosFluxo
    DentroDoValorEsperado
    AnalisandoOvertarget
    UsoAtipicoIdentificado
    EmContestacao
    EmIntervercao
End Enum

Public Enum TipoContaEnum
    MOVEL = 10
    FIXA = 20
    DADOS = 30
End Enum

Public Enum ResultadoLogin
    Logado
    UsuarioOuSenhaInvalidos
    PaginaForaDoar

End Enum


Public Enum ModeloPesquisa
    ResultadoUnico
    ResultadoSequencial
    ResultadoGlobal
End Enum

Public Enum TipoFaturaEnum
    PDF
    CSV
End Enum


Public Enum ProdutosVivo
    VozFixa
    Internet
    Movel

End Enum


Public Enum ArquivoEnum
    pdf
    csv
End Enum

Public Module Dicionarios

    Public ProdutosVivo As New Dictionary(Of Integer, String) From
        {{136, "VOZ FIXA"}, {137, "SOLUÇÃO DE VOZ"}, {138, "INTERNET"}}



End Module