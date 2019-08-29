Public Class PathsContainerFox

    Shared Function ObterPaths(Operadora As OperadoraEnum, Tipo As TipoContaEnum) As _
        (PastaEntrada As String, PastaSaida As String, Ativador As String)

        Dim listapaths As New Dictionary(Of Integer, (String, String, String)) From {
            {Operadora.VIVO + TipoContaEnum.MOVEL,
                       ("\\Servidor\4d_consultoria\AUTO\VIVO_IMP",
                    "\\Servidor\4d_consultoria\AUTO\VIVO_REL",
                    "\\Servidor\4d_consultoria\AUTO\VIVOSQL")},
            {(OperadoraEnum.TIM + TipoContaEnum.MOVEL),
                       ("\\Servidor\4d_consultoria\AUTO\TIM_IMP",
                    "\\Servidor\4d_consultoria\AUTO\TIM_REL",
                    "\\Servidor\4d_consultoria\AUTO\TIMSQL")},
            {(OperadoraEnum.CLARO + TipoContaEnum.MOVEL),
                       ("\\Servidor\4d_consultoria\AUTO\CLARO_IMP",
                    "\\Servidor\4d_consultoria\AUTO\CLARO_REL",
                    "\\Servidor\4d_consultoria\AUTO\CLAROSQL")},
            {(OperadoraEnum.OI + TipoContaEnum.MOVEL),
               ("\\Servidor\4d_consultoria\AUTO\OI_IMP",
            "\\Servidor\4d_consultoria\AUTO\OI_REL",
            "\\Servidor\4d_consultoria\AUTO\OISQL")}
        }

        

        Dim output As (String, String, String)

        listapaths.TryGetValue(Operadora + Tipo, output)


        Return output

    End Function

End Class
