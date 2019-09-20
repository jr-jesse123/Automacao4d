Public Class PathsContainerFox

    Shared Function ObterPaths(Operadora As OperadoraEnum, Tipo As TipoContaEnum) As _
        (PastaEntrada As String, PastaSaida As String, Ativador As String)

        Dim listapaths As New Dictionary(Of Integer, (String, String, String)) From {
            {Operadora.VIVO + TipoContaEnum.MOVEL,
                       ("\\192.168.244.112\4d_consultoria\AUTO\VIVO_REL\TXT_IMPORTAR",
                    "\\192.168.244.112\4d_consultoria\AUTO\VIVO_REL",
                    "\\192.168.244.112\4d_consultoria\AUTO\VIVOSQL")},
            {(OperadoraEnum.TIM + TipoContaEnum.MOVEL),
                       ("\\192.168.244.112\4d_consultoria\AUTO\TIM_REL\TXT_IMPORTAR",
                    "\\192.168.244.112\4d_consultoria\AUTO\TIM_REL",
                    "\\192.168.244.112\4d_consultoria\AUTO\TIMSQL")},
            {(OperadoraEnum.CLARO + TipoContaEnum.MOVEL),
                       ("\\192.168.244.112\4d_consultoria\AUTO\CLARO_REL\TXT_IMPORTAR",
                    "\\192.168.244.112\4d_consultoria\AUTO\CLARO_REL",
                    "\\192.168.244.112\4d_consultoria\AUTO\CLAROSQL")},
            {(OperadoraEnum.OI + TipoContaEnum.MOVEL),
               ("\\192.168.244.112\4d_consultoria\AUTO\OI_REL\TXT_IMPORTAR",
            "\\192.168.244.112\4d_consultoria\AUTO\OI_REL",
            "\\192.168.244.112\4d_consultoria\AUTO\OISQL")}
        }



        Dim output As (String, String, String)

        listapaths.TryGetValue(Operadora + Tipo, output)


        Return output

    End Function

End Class
