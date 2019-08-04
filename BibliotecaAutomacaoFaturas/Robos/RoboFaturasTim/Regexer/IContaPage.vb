Imports BibliotecaAutomacaoFaturas

Public Interface IContaPage
    Event FaturaBaixada(fatura As Fatura)
    Event FaturaChecada(fatura As Fatura)
    Sub BuscarFatura(Fatura As Fatura)
    Function ProcurarNasDemaisFaturas(fatura As Fatura) As Boolean
End Interface
