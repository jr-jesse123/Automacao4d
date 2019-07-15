Imports OpenQA.Selenium.Chrome


Public Class RoboFaturasTIM
    Private ListaDeContas As List(Of Conta)
    Private Driver As ChromeDriver
    Private TratadorDeFatura As TratadorDeFaturas
    Public Event FaturaBaixada(ByVal sender As Object, ByVal e As EventArgs)
    Public Event FaturaPaga(ByVal sender As Object, ByVal e As EventArgs)
    Public Event FaturaEmAtraso(ByVal sender As Object, ByVal e As EventArgs)
    Private WithEvents LoginPage As LoginPageTim
    Private WithEvents ContaPage As ContaPageTim
    Private ContaLogada As Conta

    Sub New(LoginPage As LoginPageTim, ContaPage As ContaPageTim)

        Me.LoginPage = LoginPage
        Me.ContaPage = ContaPage

        Driver = ContainerWebdriver.Driver
        ListaDeContas = GerRelDB.Contas.Where(Function(conta) conta.Operadora = OperadoraEnum.TIM And
                                                conta.TipoDeConta = TipoContaEnum.MOVEL) _
                                                .OrderBy(Function(conta) conta.Empresa.CNPJ) _
                                                .OrderBy(Function(conta) conta.Gestores.First.CPF).ToList

    End Sub


    Sub run()

        If Not LoginPage.Logar(ListaDeContas.First) = ResultadoLogin.PaginaForaDoar Then

            For Each conta In ListaDeContas
                If ContaLogada.Empresa.Equals(conta.Empresa) Then
ContaLogada:
                    If ContaLogada.Equals(conta) Then
                        ContaPage.BaixarUltimaFatura(conta)
                    Else
                        Stop
                    End If
                Else
                    LoginPage.Logout()
                    If LoginPage.Logar(conta) = ResultadoLogin.Logado Then GoTo ContaLogada
                End If

            Next

        End If
    End Sub

    Private Sub ManejarFatura(conta As Conta) Handles ContaPage.FaturaBaixada
        TratadorDeFaturas.RenomearFatura(conta)
        TratadorDeFaturas.PosicionarFaturaNaPasta(conta)
        TratadorDeFaturas.ConverterPdfParaTxt(conta)
        TratadorDeFaturas.ProcessarTxt(conta)
    End Sub


    Private Sub OnLoginRealizado(conta As Conta) Handles LoginPage.LoginRealizado
        ContaLogada = conta
    End Sub


End Class


