﻿Imports LibAutoFaturasStantard.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas.ErroLoginExcpetion" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas.ErroLoginExcpetion
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas.ErroLoginExcpetion" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports System.Text.RegularExpressions

Public Class ContaPageVIVOMOVEL

    Inherits DriverDependents
    Implements IContaPageVivoMovel


    Public Event FaturaBaixada(fatura As Fatura) Implements IContaPageVivoMovel.FaturaBaixada
    Public Event FaturaChecada(fatura As Fatura) Implements IContaPageVivoMovel.FaturaChecada
    Public Event FaturaBaixadaCSV(fatura As Fatura) Implements IContaPageVivoMovel.FaturaBaixadaCSV


    Public Sub BuscarFatura(fatura As Fatura) Implements IContaPage.BuscarFatura
        Me.driver = WebdriverCt.Driver

        Dim PosicionadorCNPJVivoMovel As New PosicionadorCNPJVivoMovel(driver, fatura)
        PosicionadorCNPJVivoMovel.poscionarCNPJ(fatura)

        Dim PosicionadorProdutoVivo As New PosicionadorProdutoVivo(driver, fatura)
        PosicionadorProdutoVivo.posicionarProduto("Móvel", fatura)

        Dim posicionadorContaVivo As New posicionadorContaVivo(driver, fatura)
        posicionadorContaVivo.PosicionarConta(fatura)

        Dim horario = DateTime.Now

        If fatura.Baixada = False Then
            DownloadFatura(fatura)
            If AguardaEConfirmaDwonload(60, horario) Then
                RaiseEvent FaturaBaixada(fatura)
            Else
                DownloadFatura(fatura)
                If AguardaEConfirmaDwonload(60, horario) Then
                    RaiseEvent FaturaBaixada(fatura)
                Else
                    Throw New FaturaNotDownloadedException(fatura, "Falha no download da fatura")
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
                Throw New FaturaNaoDisponivelException(fatura, "impossível baixar fatura, fatura não encontrada")
            End If
        End If

    End Sub

    Private Function EncontrarIndiceFatura(fatura As Fatura) As Integer

        Dim indice As Integer = 10
        Try
            For i = 0 To 5
                Dim VENCIMENTO As String
                Try
                    VENCIMENTO = driver.FindElementByXPath($"//*[@id='linhaB{i}']/td[1]").Text ' PROCURA POR DATAS ALTERADAS POR CONTESTAÇÃO
                Catch ex As Exception
                    VENCIMENTO = driver.FindElementByXPath($"//*[@id='linhaA{i}']/td[2]").Text ' PROCURA POR DATAS ORIGINAIS
                End Try

                Dim xPath = $"//*[@id='linhaA{i}']/td[2]"
                If VENCIMENTO = fatura.Vencimento.ToString("dd/MM/yyy") Then
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

            Dim ultimoVencimento

            Try
                ultimoVencimento = driver.FindElementByXPath($"//*[@id='linhaA0']/td[2]").Text
            Catch ex As Exception
                ultimoVencimento = "nenhumca conta disponibilizada"
            Finally
#Disable Warning BC42104 ' Variável "ultimoVencimento" é usada antes de receber um valor. Uma exceção de referência nula poderia resultar em runtime.
                Throw New FaturaNaoDisponivelException(fatura, $"Fatura não disponível, último vencimento foi: {ultimoVencimento}")
#Enable Warning BC42104 ' Variável "ultimoVencimento" é usada antes de receber um valor. Uma exceção de referência nula poderia resultar em runtime.
            End Try

        Else
            Return indice
        End If

    End Function

    Sub checarBloqueio(fatura As Fatura)

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)

        Dim msg
#Disable Warning BC42024 ' Variável local não utilizada: "sql".
        Dim sql
#Enable Warning BC42024 ' Variável local não utilizada: "sql".

        'checar se as linhas estão bloqueadas
#Disable Warning BC42322 ' Podem ocorrer erros de runtime ao converter 'String' para 'IWebDriver'.
        If ChecarPresenca("//*[@id='faturamovel_portlet_1.formGridFaturas']/div", 1) And conta.Bloqueada = False <> 1 Then
#Enable Warning BC42322 ' Podem ocorrer erros de runtime ao converter 'String' para 'IWebDriver'.

            Dim destinatarios As String()
            destinatarios = {"jesse@quatrodconsultoria.com.br", "contato@quatrodconsultoria.com.br"} 'E-mails a serem informados do blqueio
            msg = "nosso sistema detectou que a conta " & conta.NrDaConta & " do cnpj " & conta.Empresa.CNPJ & " sofreu corte e precisa de atenção às " & DateTime.Now
            'Call EnviaEmail(msg, destinatarios, conta)

        End If

    End Sub
End Class

Public Interface IContaPageVivoMovel
    Inherits IContaPage
End Interface


