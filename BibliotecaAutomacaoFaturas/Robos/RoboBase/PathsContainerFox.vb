Public Class PathsContainerFox

    Shared Function ObterPaths(Operadora As OperadoraEnum, Tipo As TipoContaEnum) As _
        (PastaEntrada As String, PastaSaida As String, Ativador As String)

        Dim listapaths As New Dictionary(Of Integer, (String, String, String)) From {
            {OperadoraEnum.VIVO + TipoContaEnum.MOVEL,
                       ("\\Servidor\4d_consultoria\AUTOMACAO\VIVO_IMPORTAR",
                    "\\Servidor\4d_consultoria\AUTOMACAO\VIVO_RELATORIOS_AUTOMATICOS",
                    "\\Servidor\4d_consultoria\AUTOMACAO\VIVOSQL")},
            {OperadoraEnum.VIVO + TipoContaEnum.MOVEL,
                       ("\\Servidor\4d_consultoria\AUTOMACAO\TIM_IMPORTAR",
                    "\\Servidor\4d_consultoria\AUTOMACAO\TIM_RELATORIOS_AUTOMATICOS",
                    "\\Servidor\4d_consultoria\AUTOMACAO\TIMSQL")},
            {OperadoraEnum.VIVO + TipoContaEnum.MOVEL,
                       ("\\Servidor\4d_consultoria\AUTOMACAO\CLARO_IMPORTAR",
                    "\\Servidor\4d_consultoria\AUTOMACAO\CLARO_RELATORIOS_AUTOMATICOS",
                    "\\Servidor\4d_consultoria\AUTOMACAO\CLAROSQL")},
            {OperadoraEnum.VIVO + TipoContaEnum.MOVEL,
               ("\\Servidor\4d_consultoria\AUTOMACAO\OI_IMPORTAR",
            "\\Servidor\4d_consultoria\AUTOMACAO\OI_RELATORIOS_AUTOMATICOS",
            "\\Servidor\4d_consultoria\AUTOMACAO\OISQL")}
        }


        Return listapaths.Values(Operadora + Tipo)

    End Function

End Class
