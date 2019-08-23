Imports BibliotecaAutomacaoFaturas

Public Interface ILoginPage
    Event LoginRealizado(conta As Conta)
    Sub Logar(conta As Conta)
    Sub logout()
    Function ObterDadosDeAcesso(conta As Conta) As DadosDeAcesso
End Interface
