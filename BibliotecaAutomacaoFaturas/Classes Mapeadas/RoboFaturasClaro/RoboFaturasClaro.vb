Imports OpenQA.Selenium.Chrome

Public Class RoboFaturasClaro
    Private ListaDeContas As List(Of Conta)
    Private Driver As ChromeDriver = ContainerWebdriver.Driver
    Public Event FaturaBaixada(ByVal sender As Object, ByVal e As EventArgs)
    Public Event FaturaPaga(ByVal sender As Object, ByVal e As EventArgs)
    Public Event FaturaEmAtraso(ByVal sender As Object, ByVal e As EventArgs)
    Private LoginPage As LoginPageClaro
    Private ContaPage As ContaPageClaro
    Private PagamentosPage As PagamentosPageClaro

    Sub New(LoginPage As LoginPageClaro, ContaPage As ContaPageClaro)
        Me.LoginPage = LoginPage
        Me.ContaPage = ContaPage
    End Sub

    Public Sub Run()

    End Sub




    Private Sub LogOut()
        Driver.FindElementByXPath("/html/body/form/table/tbody/tr[2]/td/table/tbody/tr/td[7]/a").Click()
    End Sub


End Class
