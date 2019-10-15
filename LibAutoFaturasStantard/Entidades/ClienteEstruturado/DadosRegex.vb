

Imports System.Text.RegularExpressions
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports MongoDB.Bson.Serialization.Attributes
Imports MongoDB.Bson.Serialization.Options

<BsonIgnoreExtraElements>
Public Class DadosRegex

    <BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)>
    Property Relatorios As New Dictionary(Of Integer, List(Of IPesquisaRegex))

    Sub New()
        Relatorios.Add(OperadoraEnum.TIM + TipoContaEnum.MOVEL,
                       New List(Of IPesquisaRegex)(New IPesquisaRegex() _
                       {New TotalMovelTim, New CreditosMovelTim}))

        Relatorios.Add(OperadoraEnum.CLARO + TipoContaEnum.MOVEL,
                       New List(Of IPesquisaRegex)(New IPesquisaRegex() _
                      {New TotalMovelClaro, New CreditosMovelClaro, New EncargosMovelClaro
        }))

        Relatorios.Add(OperadoraEnum.VIVO + TipoContaEnum.MOVEL,
                       New List(Of IPesquisaRegex)(New IPesquisaRegex() _
                      {New TotalMovelVivo, New CreditosMovelVivo, New EncargosMovelVivo
        }))


        Relatorios.Add(OperadoraEnum.OI + TipoContaEnum.MOVEL,
                       New List(Of IPesquisaRegex)(New IPesquisaRegex() _
                      {New TotalMovelOi, New CreditosMovelOi
        }))

        Relatorios.Add(OperadoraEnum.VIVO + TipoContaEnum.FIXA,
                       New List(Of IPesquisaRegex)(New IPesquisaRegex() _
                      {
        }))


        '   Relatorios.Add(OperadoraEnum.ALGAR + TipoContaEnum.FIXA,
        '           New List(Of IPesquisaRegex)(New IPesquisaRegex() _
        '         {New ComposicaoContaDadosAlgar}))

    End Sub
End Class
