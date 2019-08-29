
Imports BibliotecaAutomacaoFaturas


Public Class RoboFaturasOI
        Inherits RoboBase

    Public Sub New(LoginPage As ILoginPageOI, ContaPage As IContaPageOI, TratadorDeFaturaPDF As TratadorDeFaturasPDF)
        MyBase.New(LoginPage, ContaPage, TratadorDeFaturaPDF, 3, 10)
    End Sub

    Protected Overrides Function GerenciarLogin(conta As Conta) As Boolean
            Throw New NotImplementedException()
        End Function
    End Class
