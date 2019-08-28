Public Class PathsContainerFox

    Shared Function ObterPaths(Operadora As OperadoraEnum, Tipo As TipoContaEnum) As _
        (PastaEntrada As String, PastaSaida As String, Ativador As String)

        Dim listapaths As New Dictionary(Of Integer, (String, String, String)) From {
            {OperadoraEnum.VIVO + TipoContaEnum.MOVEL,
                       ("\\Servidor\4d_consultoria\AUTO\VIVO_IMP",
                    "\\Servidor\4d_consultoria\AUTO\VIVO_REL",
                    "\\Servidor\4d_consultoria\AUTO\VIVOSQL")},
            {OperadoraEnum.VIVO + TipoContaEnum.MOVEL,
                       ("\\Servidor\4d_consultoria\AUTO\TIM_IMP",
                    "\\Servidor\4d_consultoria\AUTO\TIM_REL",
                    "\\Servidor\4d_consultoria\AUTO\TIMSQL")},
            {OperadoraEnum.VIVO + TipoContaEnum.MOVEL,
                       ("\\Servidor\4d_consultoria\AUTO\CLARO_IMP",
                    "\\Servidor\4d_consultoria\AUTO\CLARO_REL",
                    "\\Servidor\4d_consultoria\AUTO\CLAROSQL")},
            {OperadoraEnum.VIVO + TipoContaEnum.MOVEL,
               ("\\Servidor\4d_consultoria\AUTO\OI_IMP",
            "\\Servidor\4d_consultoria\AUTO\OI_REL",
            "\\Servidor\4d_consultoria\AUTO\OISQL")}
        }


        Return listapaths.Values(Operadora + Tipo)

    End Function

End Class
