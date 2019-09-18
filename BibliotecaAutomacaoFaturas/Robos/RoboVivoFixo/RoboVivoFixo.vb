Imports BibliotecaAutomacaoFaturas

Public Class RoboVivoFixo
    Inherits RoboBase

    Public Sub New(LoginPage As ILoginPage, ContaPage As IContaPage, tratadorpdf As TratadorDeFaturasPDF, Operadora As OperadoraEnum, Tipo As TipoContaEnum)
        MyBase.New(LoginPage, ContaPage, tratadorpdf, Operadora, Tipo)
    End Sub

    Protected Overrides Sub RealizarLogNasContasCorrespondentes(Conta As Conta)
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Function GerenciarLogin(conta As Conta) As Boolean
        Throw New NotImplementedException()
    End Function
End Class
