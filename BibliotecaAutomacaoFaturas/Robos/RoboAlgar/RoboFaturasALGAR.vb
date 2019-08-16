Public Class RoboFaturasALGAR
    Public Operadora = OperadoraEnum.ALGAR
    Public TipoDeConta = TipoContaEnum.FIXA
    Private ListaDeContas As List(Of Conta)
    Private WithEvents TratadorDeFAturaPDF As TratadorDeFaturasPDF
    Private WithEvents TratadorDeFaturaCsv As TratadorDeFaturasCsv
    Private WithEvents LoginPage As LoginPageAlgar
    Private WithEvents ContaPage As ContaPagAlgar
    Private ContaLogada As Conta
    Public Event LoginRealizado(conta As Conta)

    Sub New(LoginPage As LoginPageAlgar, ContaPage As ContaPagAlgar, TratadordeFaturas As TratadorDeFaturasCsv, TratadorDeFaturaPDF As TratadorDeFaturasPDF)
        Me.TratadorDeFaturaCsv = TratadordeFaturas
        Me.TratadorDeFAturaPDF = TratadorDeFaturaPDF
        Me.LoginPage = LoginPage
        Me.ContaPage = ContaPage

        ListaDeContas = GerRelDB.SelecionarContasRobos(Me)



    End Sub


    Sub run()

        For Each conta In ListaDeContas

            Dim faturas = conta.Faturas.Where(Function(x) x.Pendente = True _
                                                  Or x.Baixada = False).ToList
            For index = 0 To faturas.Count - 1
Inicio:
                Try

                    If GerenciarLogin(conta) Then
                        ContaPage.BuscarFatura(faturas(index))
                    End If


                Catch ex As ErroLoginExcpetion
                    Me.ContaLogada = Nothing
                    Exit For

                Catch ex As FaturaNotDownloadedException
                    GerRelDB.AtualizarContaComLog(faturas(index), "Falha no Download da fatura")
                    Continue For

                Catch ex As PortalForaDoArException
                    WebdriverCt.ResetarWebdriver()
                    GoTo Inicio

                Catch ex As RoboFaturaException
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

    Private Function GerenciarLogin(conta As Conta) As Boolean

        Dim Logado As Boolean

        If ContaLogada Is Nothing Then
            LoginPage.Logar(conta)
        End If

        If ContaLogada IsNot Nothing Then
            Logado = ContaLogada.Empresa.Equals(conta.Empresa)
        End If


        If Logado Then
            Return True
        Else
            LoginPage.Logout()
            If LoginPage.Logar(conta) = ResultadoLogin.Logado Then
                Return True
            Else Return False
            End If
        End If
    End Function

    Private Sub ManejarFatura(fatura As Fatura, Optional TipoFatura As TipoFaturaEnum = TipoFaturaEnum.PDF) Handles ContaPage.FaturaBaixada

        If TipoFatura = TipoFaturaEnum.PDF Then
            TratadorDeFaturaCsv.executar(fatura)
        ElseIf TipoFatura = TipoFaturaEnum.PDF Then
            TratadorDeFaturaCsv.executar(fatura)
        End If




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


Public Enum TipoFaturaEnum
    PDF
    CSV
End Enum

