﻿Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI
Imports BibliotecaAutomacaoFaturas
Imports BibliotecaAutomacaoFaturas.ErroLoginExcpetion
Imports System.Text.RegularExpressions
Imports System.Runtime.Serialization

Public Class ContaPageVIVOMOVEL

    Inherits DriverDependents
    Implements IContaPageVivoMovel


    Public Event FaturaBaixada(fatura As Fatura) Implements IContaPageVivoMovel.FaturaBaixada
    Public Event FaturaChecada(fatura As Fatura) Implements IContaPageVivoMovel.FaturaChecada
    Public Event FaturaBaixadaCSV(fatura As Fatura) Implements IContaPageVivoMovel.FaturaBaixadaCSV


    Public Sub BuscarFatura(fatura As Fatura) Implements IContaPage.BuscarFatura


        Dim PosicionadorCNPJVivoMovel As New PosicionadorCNPJVivoMovel(driver, fatura)
        PosicionadorCNPJVivoMovel.poscionarCNPJ(fatura)

        Dim PosicionadorProdutoVivo As New PosicionadorProdutoVivo(driver, fatura)
        PosicionadorProdutoVivo.posicionarProduto(ProdutosVivo.Movel, fatura)

        Dim posicionadorContaVivo As New posicionadorContaVivo(driver, fatura)
        posicionadorContaVivo.PosicionarConta(fatura)

        Dim horario = Now

        If fatura.Baixada = False Then
            DownloadFatura(fatura)
            If AguardaEConfirmaDwonload(60, horario) Then
                RaiseEvent FaturaBaixada(fatura)
            Else
                DownloadFatura(fatura)
                If AguardaEConfirmaDwonload(60, horario) Then
                    RaiseEvent FaturaBaixada(fatura)
                Else
                    Throw New FaturaNotDownloadedException(fatura, "FAlha no download da fatura", True)
                End If
                
                ChecharFatura(fatura)
                RaiseEvent FaturaChecada(fatura)
            End If
        End If


    End Sub

    Private Sub ChecharFatura(fatura As Fatura)

        Dim i = EncontrarIndiceFatura(fatura)

        If i = -1 Then
            If driver.FindElementByXPath("//*[@id='faturamovel_portlet_1.formGridFaturas']/div[1]/div[1]/div/div[3]/p[2]").Text = "Pago" Then
            End If
        Else

            If driver.FindElementByXPath($"//*[@id='linhaA{i}']/td[3]").Text = "Pago" Then
                fatura.Pendente = False
            End If
        End If

    End Sub

    Sub DownloadFatura(fatura As Fatura)


        Dim i = EncontrarIndiceFatura(fatura)

        'div[3]/img
        '//*[@id="divA1"]/div
        '//*[@id="divA1"]/div/img
        'abre o menu da última fatura

        'driver.FindElement(By.XPath($"//*[@id='linhaA{i}']/td[5]")).Click()


        If driver.FindElement(By.XPath($"//*[@id='linhaA{i}']/td[5]")).FindElements(By.ClassName("deslocamento-tres-pontos")).Count > 0 Then
            driver.FindElement(By.XPath($"//*[@id='linhaA{i}']/td[5]")).Click()
        Else
            driver.FindElement(By.XPath($"//*[@id='linhaB{i}']/td[4]")).Click()
        End If



        If ChecarPresenca(driver, $"//*[@id='divMenu{i}']") Then
            'verifica se está com formato de conta atualizada
            'caminho para conta contestada
            '//*[@id="divMenu1"]



            If ChecarPresenca(driver, $"//*[@id='downloadBoleto{i}']") Then 'verifica se a conta está atrasada
                ' caminho para conta atrasada
                driver.FindElement(By.XPath($"//*[@id='visualizarFatura{i}']")).Click()
                driver.FindElement(By.XPath($"//*[@id='doDownloadFatura']")).Click()
                ChecharFatura(fatura)


            Else
                'caminho para conta em dia
                driver.FindElement(By.XPath($"//*[@id='downloadFatura{i}']")).Click()
                ChecharFatura(fatura)

            End If

        Else
            driver.FindElement(By.XPath($"//*[@id='divA{i}']/div")).Click() 'caminho para conta orgiginal
            If ChecarPresenca(driver, $"//*[@id='divMenu{i}']") Then
                driver.FindElement(By.XPath($"//*[@id='downloadFatura{i}']")).Click()
                ChecharFatura(fatura)
                RaiseEvent FaturaBaixada(fatura)
            Else
                Throw New FaturaNaoDisponivelException(fatura, "impossível baixar fatura, fatura não encontrada", True)
            End If
        End If

    End Sub

    Private Function EncontrarIndiceFatura(fatura As Fatura) As Integer

        Dim indice As Integer = 10
        Try
            For i = 0 To 5

                Dim xPath = $"//*[@id='linhaA{i}']/td[2]"
                If driver.FindElementByXPath(xPath).Text = fatura.Vencimento.ToString("dd/MM/yyy") Then
                    indice = i
                    Exit For
                End If
            Next

        Catch ex As WebDriverException
            If ChecarPresenca(driver, "//*[@id='faturamovel_portlet_1.formGridFaturas']/div[1]/div[1]/div/div[3]/p[2]") Then
                Return -1
            End If
        End Try


        If indice = 10 Then
            Throw New FaturaNaoDisponivelException(fatura, "O vencimento informado não foi encontrado")
        Else
            Return indice
        End If

    End Function

    Sub checarBloqueio(fatura As Fatura)

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)

        Dim msg
        Dim sql

        'checar se as linhas estão bloqueadas
        If ChecarPresenca("//*[@id='faturamovel_portlet_1.formGridFaturas']/div", 1) And conta.Bloqueada = False <> 1 Then

            Dim destinatarios As String()
            destinatarios = {"jesse@quatrodconsultoria.com.br", "contato@quatrodconsultoria.com.br"} 'E-mails a serem informados do blqueio
            msg = "nosso sistema detectou que a conta " & conta.NrDaConta & " do cnpj " & conta.Empresa.CNPJ & " sofreu corte e precisa de atenção às " & Now
            Call EnviaEmail(msg, destinatarios, conta)

        End If

    End Sub
End Class

Friend Class PosicionadorProdutoVivo
    Private driver As ChromeDriver
    Private fatura As Fatura
    Private movel As ProdutosVivo

    Public Sub New(driver As ChromeDriver, fatura As Fatura)
        Me.driver = driver
        Me.fatura = fatura

    End Sub

    Friend Sub posicionarProduto(movel As ProdutosVivo, fatura As Fatura)

        If driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[2]/button/div/span[2]").Text = "Móvel" Then
            Exit Sub
        End If

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)
        Try
            driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[2]/button").Click()
        Catch ex As WebDriverException
            Stop
            Exit Sub
        End Try

        Dim produtosContainer = driver.FindElement(By.XPath("//*[@id='formSelectedItem']/ul"))

        Dim produto As IWebElement
        Try
            produto = driver.FindElementByLinkText("Móvel")
            produto.Click()
        Catch ex As WebDriverException

            driver.FindElementByXPath("//*[@id='headerSubmenu_1_2']/div/div[1]/div[2]/div[2]/button").Click()
            Throw New ProdutoNaoCadastradoException(conta, "Este tipo de produto não está cadastrado para este gestor", False)

        End Try



    End Sub
End Class

<Serializable>
Friend Class ProdutoNaoCadastradoException
    Inherits RoboFaturaException

    Public Sub New()
    End Sub

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Public Sub New(fatura As Fatura, message As String, Optional dadosok As Boolean = True, Optional innerException As Exception = Nothing)
        MyBase.New(fatura, message, dadosok, innerException)
    End Sub

    Public Sub New(gestor As Gestor, message As String, operadora As OperadoraEnum, tipo As TipoFaturaEnum, Optional dadosok As Boolean = True, Optional innerException As Exception = Nothing)
        MyBase.New(gestor, message, operadora, tipo, dadosok, innerException)
    End Sub

    Public Sub New(empresa As Empresa, message As String, operadora As OperadoraEnum, tipo As TipoFaturaEnum, Optional dadosok As Boolean = True, Optional innerException As Exception = Nothing)
        MyBase.New(empresa, message, operadora, tipo, dadosok, innerException)
    End Sub

    Public Sub New(conta As Conta, message As String, Optional dadosok As Boolean = True, Optional innerException As Exception = Nothing)
        MyBase.New(conta, message, dadosok, innerException)
    End Sub

    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class

Public Interface IContaPageVivoMovel
    Inherits IContaPage
End Interface

Public Enum ProdutosVivo
    VozFixa
    Internet
    Movel
End Enum
