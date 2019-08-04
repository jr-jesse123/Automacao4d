Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium.Chrome


Public MustInherit Class RoboBase
    Inherits DriverDependents

    Private driver As ChromeDriver
    Private ListaDeContas As List(Of Conta)
    Private WithEvents TratadorDeFatura As TratadorDeFaturas
    Public Event FaturaBaixada(ByVal sender As Object, ByVal e As EventArgs)
    Public Event FaturaPaga(ByVal sender As Object, ByVal e As EventArgs)
    Public Event FaturaEmAtraso(ByVal sender As Object, ByVal e As EventArgs)
    Private WithEvents LoginPage As ILoginPage
    Private WithEvents ContaPage As IContaPage
    Private ContaLogada As Conta


    Sub New(LoginPage As LoginPageTim, ContaPage As ContaPageTim, TratadordeFaturas As TratadorDeFaturas)
        Me.driver = WebdriverCt.Driver
        Me.TratadorDeFatura = TratadordeFaturas
        Me.LoginPage = LoginPage
        Me.ContaPage = ContaPage


        ListaDeContas = GerRelDB.Contas.Where(Function(conta)
                                                  Return conta.Operadora = OperadoraEnum.TIM And
                                                conta.TipoDeConta = TipoContaEnum.MOVEL
                                              End Function) _
                                                .OrderBy(Function(conta) conta.Empresa.CNPJ) _
                                                .OrderBy(Function(conta) conta.Gestores.First.CPF).ToList


    End Sub


    Sub run()

        For Each conta In ListaDeContas

            Dim faturas = conta.Faturas.Where(Function(x) x.Pendente = True _
                                                  Or x.Baixada = False).ToList
            For index = 0 To faturas.Count - 1
Inicio:
                Try
                    Dim Logado As Boolean

                    If ContaLogada Is Nothing Then
                        LoginPage.Logar(conta)
                    End If

                    Logado = ContaLogada.Equals(conta)
                    If Logado Then


                        'If faturas(index).Baixada = False Then
                        ContaPage.BuscarFatura(faturas(index)) 'mudar para fatura
                        'ElseIf faturas(index).Pendente = True Then
                        'ContaPage.RealizarChecagens(faturas(index))
                        'End If

                    Else



                        LoginPage.Logout()
                        If LoginPage.Logar(conta) = ResultadoLogin.Logado Then GoTo Inicio
                    End If

                Catch ex As ErroLoginExcpetion
                    Dim NrDeFaturasRestantesDaConta = conta.Faturas.Count - conta.Faturas.IndexOf(faturas(index))
                    index = NrDeFaturasRestantesDaConta - 1
                    Continue For

                Catch ex As ErroLoginExcpetion.FaturaNotDownloadedException
                    GerRelDB.AtualizarContaComLog(faturas(index), "Falha no Download da fatura")
                    Continue For

                Catch ex As PortalForaDoArException
                    WebdriverCt.ResetarWebdriver()
                    GoTo Inicio

                Catch ex As RoboFaturaException
                    WebdriverCt.ResetarWebdriver()
                    LoginPage.Logar(conta)
                    Continue For

#If RELEASE Then
                Catch ex As Exception
                    GerRelDB.EnviarLogFatura(faturas(index), ex.Message + ex.StackTrace)
                    Continue For
#Else

#End If

                End Try


            Next
        Next

    End Sub

    Private Sub ManejarFatura(fatura As Fatura) Handles ContaPage.FaturaBaixada

        TratadorDeFatura.executar(fatura)

    End Sub

    Private Sub OnFaturaChecada(fatura As Fatura) Handles ContaPage.FaturaChecada

        GerRelDB.AtualizarContaComLog(fatura, $"Fatura Checada {Now.ToShortTimeString}", True)

    End Sub


    Private Sub OnLoginRealizado(conta As Conta) Handles LoginPage.LoginRealizado
        conta.DadosOk = True

        GerRelDB.AtualizarContaComLog(conta.Faturas.First, $"Logado corretamente ", True)
        ContaLogada = conta

    End Sub



End Class


