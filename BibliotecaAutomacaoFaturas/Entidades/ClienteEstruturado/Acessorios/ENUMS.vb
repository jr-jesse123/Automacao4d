Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Enum OperadoraEnum
    VIVO
    CLARO
    TIM
    OI
    NEXTEL
End Enum

<BsonIgnoreExtraElements>
Public Enum SubtipoEnum
    Analogico
    TroncoE1
    TroncoSIP

    Celular
    Dados
    M2M
    CelularEDados
    FixoGSM

    ADSL
    SemiDedicado
    Dedicado
    VPN
    MPLS
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
    MOVEL
    FIXA
    DADOS
End Enum

Public Enum ResultadoLogin
    Logado
    UsuarioOuSenhaInvalidos
    PaginaForaDoar

End Enum

