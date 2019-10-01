

Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas
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
