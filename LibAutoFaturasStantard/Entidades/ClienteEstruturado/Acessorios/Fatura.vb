
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports MongoDB.Bson.Serialization.Attributes

<BsonIgnoreExtraElements>
Public Class Fatura
    Private _Referencia As String = ""
    Property Vencimento As Date
    Property Ajuste As Double
    Property Total As Double
    Property ValorContestado As Double
    Property Pendente As Boolean
    Property Conferida As Boolean
    Property EmContestacao As Boolean
    Property Aprovada As Boolean
    Property ValorAContestar As Double
    Property Creditos As Double
    Property Encargos As Double
    Property LogRobo As New List(Of String)
    Property Tratada As Boolean
    Property Baixada As Boolean
    Property NrConta As String
    Property NrDaContaAnterior As String
    <BsonIgnore>
    Property Relatorios As New List(Of IPesquisaRegex)
    Property RelatoriosExcel As New RelatorioCsv
    Property InfoDownloads As New List(Of InfoDownload)
    Property FaturaEnviadaParaDrive As Boolean
    Property FaturaPosicionadaNaPasta As Boolean
    Property FluxoDisparado As Boolean
    Property FaturaConvertida As Boolean
    Property FaturaProcessadaFox As Boolean



    Public Property Referencia As String
        Get
#Disable Warning BC42104 ' Variável "Referencia" é usada antes de receber um valor. Uma exceção de referência nula poderia resultar em runtime.
            If Referencia Is Nothing Then
#Enable Warning BC42104 ' Variável "Referencia" é usada antes de receber um valor. Uma exceção de referência nula poderia resultar em runtime.
                Return DefinidorDeReferenciaDeFaturas.DescobrirReferencia(Vencimento)
            End If

            If _Referencia.Length > 0 Then
                Return _Referencia
            Else
                Return DefinidorDeReferenciaDeFaturas.DescobrirReferencia(Vencimento)
            End If
        End Get
        Set
            _Referencia = Value
        End Set
    End Property


End Class

