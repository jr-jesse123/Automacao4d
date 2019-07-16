Imports OpenQA.Selenium.Chrome
Imports BibliotecaAutomacaoFaturas.Utilidades
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Support.UI

Public Class ContaPageClaro
    Private driver As ChromeDriver
    Private _seletorConta As SelectElement
    Private Property SeletorConta() As SelectElement
        Get
            If _seletorConta Is Nothing Then
                _seletorConta = New SelectElement(driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[5]/select"))
            End If
            Return _seletorConta
        End Get
        Set(ByVal value As SelectElement)
            _seletorConta = value
        End Set
    End Property

    Private _seletorFatura As SelectElement
    Private Property SeletorFatura() As SelectElement
        Get
            If _seletorFatura Is Nothing Then
                _seletorFatura = New SelectElement(driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[5]/select"))
            End If
            Return _seletorFatura
        End Get
        Set(ByVal value As SelectElement)
            _seletorFatura = value
        End Set
    End Property


    Public Sub New()
        Me.driver = WebdriverCt.Driver
    End Sub


    Public Sub BaixarUltimaFatura(Conta As String)
        _seletorConta.SelectByText(Conta)
        driver.FindElementByXPath("/html/body/center/form/table/tbody/tr[6]/td/input").Click()
    End Sub

    Public Sub BaixarTodasFaturas()

        Throw New NotImplementedException

        AbrirnovoLink(driver, By.XPath(""))
        driver.FindElementById("download").Click()


    End Sub

End Class
