
Imports BibliotecaAutomacaoFaturas

Friend Class RoboFaturasOI
    Inherits RoboBase

    Public Sub New(LoginPage As ILoginPageOI, ContaPage As IContaPageOI, TratadorDeFaturaPDF As TratadorDeFaturasPDF, Operadora As OperadoraEnum, Tipo As TipoContaEnum)
        MyBase.New(LoginPage, ContaPage, TratadorDeFaturaPDF, Operadora, Tipo)
    End Sub

    Protected Overrides Function GerenciarLogin(conta As Conta) As Boolean
        Throw New NotImplementedException()
    End Function
End Class
