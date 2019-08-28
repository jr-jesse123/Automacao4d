Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI
Imports BibliotecaAutomacaoFaturas
Imports BibliotecaAutomacaoFaturas.ErroLoginExcpetion
Imports System.Text.RegularExpressions


Public Class ContaPageVIVOMOVEL

    Inherits DriverDependents
    Implements IContaPageVivoMovel


    Public Event FaturaBaixada(fatura As Fatura) Implements IContaPageVivoMovel.FaturaBaixada
    Public Event FaturaChecada(fatura As Fatura) Implements IContaPageVivoMovel.FaturaChecada
    Public Event FaturaBaixadaPDF(fatura As Fatura) Implements IContaPageVivoMovel.FaturaBaixadaPDF


    Public Sub BuscarFatura(fatura As Fatura) Implements IContaPage.BuscarFatura


        Dim PosicionadorCNPJVivoMovel As New PosicionadorCNPJVivoMovel(driver, fatura)
        PosicionadorCNPJVivoMovel.poscionarCNPJ(fatura)

        Dim posicionadorContaVivo As New posicionadorContaVivo(driver, fatura)
        posicionadorContaVivo.PosicionarConta(fatura)

        Dim horario = Now

        If fatura.Baixada = False Then
            DownloadFatura(fatura)
            If AguardaEConfirmaDwonload(120, horario) Then
                RaiseEvent FaturaBaixada(fatura)
            End If
        Else
            ChecharFatura(fatura)
        End If



    End Sub

    Private Sub ChecharFatura(fatura As Fatura)

        Dim i = EncontrarIndiceFatura(fatura)

        If driver.FindElementByXPath($"//*[@id='linhaA{i}']/td[3]").Text = "Pago" Then
            fatura.Pendente = False
        End If

    End Sub

    Sub DownloadFatura(fatura As Fatura)


        Dim i = EncontrarIndiceFatura(fatura)

        'abre o menu da última fatura
        driver.FindElement(By.XPath($"//*[@id='linhaA{i}']/td[5]")).Click()

        If ChecarPresenca(driver, "$//*[@id='divMenu{i}']") Then 'verifica se está com formato de conta atualizada
            'caminho para conta contestada

            If ChecarPresenca(driver, $"//*[@id='downloadBoleto{i}']") Then 'verifica se a conta está atrasada
                ' caminho para conta atrasada
                driver.FindElement(By.XPath($"//*[@id='visualizarFatura{i}']")).Click()
                driver.FindElement(By.XPath($"//*[@id='doDownloadFatura']")).Click()
                ChecharFatura(fatura)
                RaiseEvent FaturaBaixada(fatura)

            Else
                'caminho para conta em dia
                driver.FindElement(By.XPath($"//*[@id='downloadFatura{i}']")).Click()
                ChecharFatura(fatura)
                RaiseEvent FaturaBaixada(fatura)
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

        For i = 0 To 5

            Dim xPath = $"//*[@id='linhaA{i}']/td[2]"
            If driver.FindElementByXPath(xPath).Text = fatura.Vencimento.ToString("dd/MM/yyy") Then
                indice = i
                Exit For
            End If
        Next

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

Public Interface IContaPageVivoMovel
    Inherits IContaPage
End Interface
