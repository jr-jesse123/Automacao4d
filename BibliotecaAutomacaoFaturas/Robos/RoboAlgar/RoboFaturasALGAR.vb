Public Class RoboFaturasALGAR
    Public Operadora = OperadoraEnum.ALGAR
    Public TipoDeConta = TipoContaEnum.FIXA
    Private ListaDeContas As List(Of Conta)
    Private WithEvents TratadorDeFAturaPDF As TratadorDeFaturasPDF
    Private WithEvents TratadorDeFaturaCsv As TratadorDeFaturasCsv
    Private WithEvents LoginPage As LoginPageAlgar
    Private WithEvents ContaPage As ContaPageAlgar
    Private ContaLogada As Conta
    Public Event LoginRealizado(conta As Conta)

    Sub New(LoginPage As LoginPageAlgar, ContaPage As ContaPageAlgar, TratadordeFaturas As TratadorDeFaturasCsv, TratadorDeFaturaPDF As TratadorDeFaturasPDF)
        Me.TratadorDeFaturaCsv = TratadordeFaturas
        Me.TratadorDeFAturaPDF = TratadorDeFaturaPDF
        Me.LoginPage = LoginPage
        Me.ContaPage = ContaPage

        ListaDeContas = GerRelDB.SelecionarContasRobos(Me)

    End Sub


    Sub run()

        For Each conta In ListaDeContas

            Debug.WriteLine(ListaDeContas.IndexOf(conta))

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
                    
                    Continue For

                Catch ex As PortalForaDoArException
                    WebdriverCt.ResetarWebdriver()
                    GoTo Inicio

                Catch ex As RoboFaturaException
                    Continue For

#If Not DEBUG Then
                Catch ex As Exception
                    Dim X As New RoboFaturaException(faturas(index), ex.Message + ex.StackTrace)
                    Me.ContaLogada = Nothing
                    Continue For
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

            Logado = ContaLogada.Empresa.CNPJ.Contains(conta.Empresa.CNPJ.Substring(0, 9))
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

    Private Sub ManejarFatura(fatura As Fatura) Handles ContaPage.FaturaBaixada

        TratadorDeFAturaPDF.TratamentoBasicoDeFAtura(fatura)
        TratadorDeFaturaCsv.executar(fatura)
    End Sub

    Private Sub ManejarFaturaPDF(fatura As Fatura) Handles ContaPage.FaturaBaixadaPDF

        TratadorDeFAturaPDF.executar(fatura)

    End Sub

    Private Sub OnFaturaChecada(fatura As Fatura) Handles ContaPage.FaturaChecada

        GerRelDB.AtualizarContaComLog(fatura, $"Fatura Checada {Now.ToShortTimeString}", True)

    End Sub


    Private Sub OnLoginRealizado(conta As Conta) Handles LoginPage.LoginRealizado

        Dim ContasAssociadas = GerRelDB.Contas.Where(Function(c) c.Empresa.CNPJ.Contains(conta.Empresa.CNPJ.Substring(0, 9))).ToList

        For Each _conta In ContasAssociadas
            Dim senhaContaDaLista = _conta.Empresa.ListaSenhas.Where(Function(s) s.Operadora = Me.Operadora And s.Tipo = Me.TipoDeConta).First
            Dim senhaContaLogada = conta.Empresa.ListaSenhas.First
            If senhaContaDaLista.Login = senhaContaLogada.Login Then
                conta.DadosOk = True
                GerRelDB.AtualizarContaComLog(conta.Faturas.First, $"Logado corretamente ", True)
                ContaLogada = conta
            End If
        Next

    End Sub
End Class


Public Enum TipoFaturaEnum
    PDF
    CSV
End Enum

