Imports System.IO
Imports BibliotecaAutomacaoFaturas
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




    Sub New(LoginPage As LoginPageTim, ContaPage As ContaPageTim, TratadordeFaturas As TratadorDeFaturas)

        Me.TratadorDeFatura = TratadordeFaturas
        Me.LoginPage = LoginPage
        Me.ContaPage = ContaPage

        Driver = WebdriverCt.Driver
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
                        ContaPage.PrepararDownloadUltimaFatura(conta)
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

        TratadorDeFatura.executar(conta)

    End Sub


    Private Sub OnLoginRealizado(conta As Conta) Handles LoginPage.LoginRealizado
        ContaLogada = conta
    End Sub


End Class


Public Class TratadorDeFaturas
    Private ArquivoPath As String
    Private DestinoPath As String = "C:\SISTEMA4D\TIM"
    Private conta As Conta
    Private ConversorPDF As ConversorPDF

    Sub New(ConversorPDF As ConversorPDF)
        Me.ConversorPDF = ConversorPDF
    End Sub

    Public Sub RenomearFatura()

        Dim NomeArquivo = Path.GetFileNameWithoutExtension(ArquivoPath)
        Dim novoNome As String = ArquivoPath.Replace(NomeArquivo, conta.NrDaConta.ToString)

        Rename(ArquivoPath, novoNome)
        ArquivoPath = novoNome

    End Sub

    Public Sub PosicionarFaturaNaPasta()
        Dim x As New FileInfo(ArquivoPath)
        x.CopyTo(conta.Pasta + Path.GetFileName(ArquivoPath))
    End Sub

    Public Sub ConverterPdfParaTxt()

        ConversorPDF.getTextFromPDF(ArquivoPath, DestinoPath)

    End Sub

    Public Sub ProcessarTxt()

    End Sub

    Friend Sub executar(conta As Conta)
        Me.conta = conta
        EncontrarPathUltimoArquivo()
        RenomearFatura()
        PosicionarFaturaNaPasta()
        ConverterPdfParaTxt()
        ProcessarTxt()


    End Sub

    Private Sub EncontrarPathUltimoArquivo()

        Dim arquivos As String() = Directory.GetFiles(WebdriverCt._folderContas)

        Dim ultimoArquivo As FileInfo
        For Each arquivo As String In arquivos
            Dim arquivoAtual As New FileInfo(arquivo)
            If ultimoArquivo Is Nothing Then
                ultimoArquivo = arquivoAtual
            ElseIf ultimoArquivo.CreationTime < arquivoAtual.CreationTime Then
                ultimoArquivo = arquivoAtual
            End If



        Next


        ArquivoPath = ultimoArquivo.FullName

    End Sub


End Class


