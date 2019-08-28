Imports BibliotecaAutomacaoFaturas

Public Class LoginPageOi
    Implements ILoginPageOI

    Public Event LoginRealizado As ILoginPage.LoginRealizadoEventHandler Implements ILoginPage.LoginRealizado

    Public Sub logout() Implements ILoginPage.logout
        Throw New NotImplementedException()
    End Sub

    Public Sub Logar(conta As Conta) Implements ILoginPage.Logar
        Throw New NotImplementedException()
    End Sub
End Class
