Imports BibliotecaAutomacaoFaturas

Public MustInherit Class RoboBase
    Public Operadora As OperadoraEnum
    Public TipoDeConta As TipoContaEnum
    Private ListaDeContas As List(Of Conta)
    Protected WithEvents TratadorDeFAturaPDF As TratadorDeFaturasPDF
    Protected WithEvents LoginPage As ILoginPage
    Protected WithEvents ContaPage As IContaPage
    Protected ContaLogada As Conta
    Public Shared Event Log(Texto As String)

    Sub New(LoginPage As ILoginPage, ContaPage As IContaPage, TratadorDeFaturaPDF As TratadorDeFaturasPDF,
            Operadora As OperadoraEnum, Tipo As TipoContaEnum)

        Me.Operadora = Operadora
        Me.TipoDeConta = Tipo

        Me.TratadorDeFAturaPDF = TratadorDeFaturaPDF
        Me.LoginPage = LoginPage
        Me.ContaPage = ContaPage

        ListaDeContas = GerRelDB.SelecionarContasRobos(Me)
    End Sub


    Sub run()

        For Each conta In ListaDeContas

            Debug.Print(ListaDeContas.IndexOf(conta))

            Dim faturas = buscarFaturasPendentes(conta)
            For index = 0 To faturas.Count - 1
Inicio:
                RaiseEvent Log($"Buscando fatura da conta {conta.NrDaConta} com vencimento em {faturas(index).Vencimento.ToShortDateString} as {Now.ToShortTimeString} 
 empresa: {conta.Empresa.Nome} cnpj: {conta.Empresa.CNPJ} fatura baixada: {faturas(index).Baixada} fatura pendente: {faturas(index).Pendente}")
                Try

                    If GerenciarLogin(conta) Then
                        ContaPage.BuscarFatura(faturas(index))
                    End If


                Catch ex As ErroLoginExcpetion
                    RaiseEvent Log($"Erro de login")
                    Me.ContaLogada = Nothing
                    Exit For

                Catch ex As FaturaNotDownloadedException
                    RaiseEvent Log($"Fatura não baixada")
                    Continue For

                Catch ex As PortalForaDoArException
                    RaiseEvent Log($"portal fora do ar")
                    Me.ContaLogada = Nothing
                    'WebdriverCt.ResetarWebdriver()
                    GoTo Inicio

                Catch ex As FaturaNaoDisponivelException
                    RaiseEvent Log($"fatura não disponível")
                    Continue For

                Catch ex As RoboFaturaException
                    RaiseEvent Log($"Outro erro de robofatura")
                    Continue For

#If Not DEBUG Then
                Catch ex As Exception
                RaiseEvent Log($"Outro erro de desconhecido {ex.message}")
                    Dim X As New RoboFaturaException(faturas(index), ex.Message + ex.StackTrace)
                    Me.ContaLogada = Nothing
                    Continue For
#End If

                End Try


            Next
        Next

    End Sub

    Private Function buscarFaturasPendentes(conta As Conta) As List(Of Fatura)

        Dim output = conta.Faturas.Where(Function(x) x.Pendente = True _
                                                  Or x.Baixada = False).ToList
        Return output

    End Function

    Protected MustOverride Function GerenciarLogin(conta As Conta) As Boolean

    Protected Overridable Sub ManejarFatura(fatura As Fatura) Handles ContaPage.FaturaBaixada

        RaiseEvent Log("Fatura baixada")
        TratadorDeFAturaPDF.executar(fatura)
        RaiseEvent Log("Fatura tratada")
    End Sub


    Protected Sub OnFaturaChecada(fatura As Fatura) Handles ContaPage.FaturaChecada
        GerRelDB.AtualizarContaComLogNaFatura(fatura, $"Fatura Checada {Now.ToShortTimeString}", True)

        RaiseEvent Log("Fatura checada")
    End Sub


    Protected Sub OnLoginRealizado(conta As Conta) Handles LoginPage.LoginRealizado ' fazer overrridable

        ContaLogada = conta

        RealizarLogNasContasCorrespondentes(conta)

        RaiseEvent Log($"Login com sucesso")

    End Sub

    Protected Overridable Sub RealizarLogNasContasCorrespondentes(Conta As Conta)

        GerRelDB.AtualizarContaComLogNaFatura(Conta.Faturas.First, $"Logado corretamente ", True)

    End Sub

    Public Shared Sub EnviarLog(texto As String)
        RaiseEvent Log(texto)
    End Sub

End Class

