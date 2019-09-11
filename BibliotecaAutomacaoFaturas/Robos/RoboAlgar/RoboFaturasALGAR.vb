Imports BibliotecaAutomacaoFaturas

'Public Class RoboFaturasALGAR
'    Inherits RoboBase



'    Public Sub New(LoginPage As IloginPageAlgar, ContaPage As IContaPageAlgar)

'        MyBase.New(LoginPage, ContaPage, 6, 20)

'    End Sub


'    Protected Overrides Function GerenciarLogin(conta As Conta) As Boolean

'        Dim Logado As Boolean


'        Try
'            If ContaLogada Is Nothing Then
'                LoginPage.Logar(conta)
'            End If

'            If ContaLogada IsNot Nothing Then
'                Logado = ContaLogada.Empresa.CNPJ.Contains(conta.Empresa.CNPJ.Substring(0, 9))
'            End If

'            If Logado Then
'                Return True
'            Else
'                LoginPage.Logout()
'                LoginPage.Logar(conta)
'                Return True
'            End If

'        Catch ex As Exception
'            Return False
'        End Try

'    End Function


'    Private Sub ManejarFaturaPDF(fatura As Fatura) Handles ContaPage.FaturaBaixadaCSV

'        Dim arquivoPath = EncontrarPathUltimoArquivo()
'        arquivoPath = RenomearFatura(fatura, arquivoPath)

'        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)

'        Dim infoDwonload As New InfoDownload With {.path = arquivoPath, .tipoArquivo = ArquivoEnum.csv,
'            .nrConta = fatura.NrConta, .vencimento = fatura.Vencimento,
'            .operadora = conta.Operadora, .tipoConta = conta.TipoDeConta}

'        fatura.InfoDownloads.Add(infoDwonload)

'    End Sub


'    Protected Overrides Sub RealizarLogNasContasCorrespondentes(Conta As Conta)

'        Dim ContasAssociadas = GerRelDB.Contas.Where(Function(c) c.Empresa.CNPJ.Contains(Conta.Empresa.CNPJ.Substring(0, 9))).ToList

'        For Each _conta In ContasAssociadas
'            Dim senhaContaDaLista = _conta.Empresa.ListaSenhas.Where(Function(s) s.Operadora = Me.Operadora And s.Tipo = Me.TipoDeConta).First
'            Dim senhaContaLogada = Conta.Empresa.ListaSenhas.First
'            If senhaContaDaLista.Login = senhaContaLogada.Login Then
'                Conta.DadosOk = True
'                GerRelDB.AtualizarContaComLogNaFatura(Conta.Faturas.First, $"Logado corretamente ", True)
'                ContaLogada = Conta
'            End If
'        Next
'    End Sub

'End Class



