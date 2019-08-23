Imports BibliotecaAutomacaoFaturas

Public Interface IContaPage
    Event FaturaBaixada(fatura As Fatura)
    Event FaturaChecada(fatura As Fatura)
    Event FaturaBaixadaPDF(fatura As Fatura)
    Sub BuscarFatura(fatura As Fatura)
End Interface
