Public Class PathsContainerFox

    Shared Function ObterPaths(Operadora As OperadoraEnum, Tipo As TipoContaEnum) As _
        (PastaEntrada As String, PastaSaida As String, Ativador As String)

#Disable Warning BC42025 ' Acesso de membro compartilhado, membro constante, membro enum ou tipo aninhado por meio de uma instância; a expressão de qualificação não será avaliada.
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
#Enable Warning BC42025 ' Acesso de membro compartilhado, membro constante, membro enum ou tipo aninhado por meio de uma instância; a expressão de qualificação não será avaliada.



        Dim output As (String, String, String)

#Disable Warning BC42108 ' Variável "output" é passada por referência antes de receber um valor. Uma exceção de referência nula poderia resultar em runtime. Certifique-se que a estrutura ou todos os membros de referência são inicializados antes do uso
        listapaths.TryGetValue(Operadora + Tipo, output)
#Enable Warning BC42108 ' Variável "output" é passada por referência antes de receber um valor. Uma exceção de referência nula poderia resultar em runtime. Certifique-se que a estrutura ou todos os membros de referência são inicializados antes do uso


        Return output

    End Function

End Class
