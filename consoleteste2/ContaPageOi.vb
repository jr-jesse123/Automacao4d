Imports BibliotecaAutomacaoFaturas

Public Class ContaPageOi
    Implements IContaPageOI

    Public Event FaturaBaixada As IContaPage.FaturaBaixadaEventHandler Implements IContaPage.FaturaBaixada
    Public Event FaturaChecada As IContaPage.FaturaChecadaEventHandler Implements IContaPage.FaturaChecada
    Public Event FaturaBaixadaPDF As IContaPage.FaturaBaixadaPDFEventHandler Implements IContaPage.FaturaBaixadaPDF

    Public Sub BuscarFatura(fatura As Fatura) Implements IContaPage.BuscarFatura
        Throw New NotImplementedException()
    End Sub
End Class