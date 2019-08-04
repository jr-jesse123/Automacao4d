Imports BibliotecaAutomacaoFaturas

Public Interface ILoginPage
    Event LoginRealizado(conta As Conta)
    Sub Logout()
    Function Logar(conta As Conta) As ResultadoLogin
End Interface
