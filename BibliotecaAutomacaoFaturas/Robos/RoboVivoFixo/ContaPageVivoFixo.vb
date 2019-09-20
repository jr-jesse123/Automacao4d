Public Class ContaPageVivoFixo
    Implements IContaPageVivoFixo

    Public Event FaturaBaixada(fatura As Fatura) Implements IContaPage.FaturaBaixada
    Public Event FaturaChecada(fatura As Fatura) Implements IContaPage.FaturaChecada
    Public Event FaturaBaixadaCSV(fatura As Fatura) Implements IContaPage.FaturaBaixadaCSV

    Public Sub BuscarFatura(fatura As Fatura) Implements IContaPage.BuscarFatura
        Throw New NotImplementedException()
    End Sub
End Class