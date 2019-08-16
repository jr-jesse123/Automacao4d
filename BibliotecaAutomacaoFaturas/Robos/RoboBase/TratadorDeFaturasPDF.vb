Imports System.IO
Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas

Public Class TratadorDeFaturasPDF
    Inherits TratadorDeFAturasBase
    Private ConversorPDF As ConversorPDF
    Public Sub New(DriveApi As GoogleDriveAPI, ConversorPDF As ConversorPDF, ApiBitrix As ApiBitrix)

        MyBase.New(DriveApi, ApiBitrix)
        Me.ConversorPDF = ConversorPDF

    End Sub

    Protected Overrides Property extensaodoarquivo As String = ".pdf"

    Protected Overrides Sub ExtrairInformacoesDaFatura(FATURA As Fatura)

        Dim x As New FileInfo(DestinoPath +
                              Path.GetFileName(ArquivoPath.Replace(".pdf", ".txt")))
        x.Delete()

        ConversorPDF.ConverterPdfParaTxt(ArquivoPath, DestinoPath + Path.GetFileName(ArquivoPath), FATURA)
    End Sub

    Protected Overrides Sub ExtrairFaturaSeNecessario()
        'esta clase não precisa fazer nada neste caso pois as faturas já vem prontas para consumo
    End Sub

    Protected Overrides Sub ProcessarFatura()

    End Sub
End Class
