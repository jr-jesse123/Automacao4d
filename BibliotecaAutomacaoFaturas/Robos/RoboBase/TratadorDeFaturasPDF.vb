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

    Protected Overrides Sub AdicionarInformacoesFatura(fatura As Fatura)

        For Each relatorio In fatura.Relatorios
            Dim nome = relatorio.GetType.Name
            Dim propriedades = fatura.GetType.GetProperties
            For Each propriedade In propriedades
                If nome.StartsWith(propriedade.Name) Then
                    If relatorio.Iniciado Then
                        propriedade.SetValue(fatura, relatorio.Resultado)
                    Else
                        propriedade.SetValue(fatura, 0)
                    End If
                End If
            Next
        Next

    End Sub

    Public Sub TratamentoBasicoDeFAtura(fatura As Fatura)

        EcontrarContaDaFatura(fatura)
        EncontrarPathUltimoArquivo()
        RenomearFatura(fatura)
        PosicionarFaturaNaPasta()
        PosicionarFaturaNoDrive(fatura)
    End Sub

End Class
