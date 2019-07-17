Imports System.IO
Imports System.Text.RegularExpressions
Imports Acrobat
Imports iText7

Public Class ConversorPDF
    Private Filepath As String
    Private PaginaAtual = 0
    Private AppAcrobat As AcroApp
    Private objAVDoc As New AcroAVDoc
    Private objPDDoc As New AcroPDDoc
    Private objPage As AcroPDPage
    Private objSelection As AcroPDTextSelect
    Private objHighlight As AcroHiliteList
    Private pageNum As Long
    Private strText As String = ""

    Sub New(FilePath As String)
        Me.Filepath = FilePath

    End Sub


    Public Function ConverterPagina()
        'prepara a conexão com o adobe
        If (objAVDoc.Open(Filepath, "")) Then
            objPDDoc = objAVDoc.GetPDDoc

            objPage = objPDDoc.AcquirePage(PaginaAtual)
            objHighlight = New AcroHiliteList
            objHighlight.Add(0, 15000) ' Adjust this up if it's not getting all the text on the page
            objSelection = objPage.CreatePageHilite(objHighlight)

            If Not objSelection Is Nothing Then
                For tCount = 0 To objSelection.GetNumText - 1
                    strText = strText & objSelection.GetText(tCount)
                Next tCount
            End If

            Return strText
        Else
            Return Nothing
        End If
    End Function

    Public Function ObterNumeroDePaginas() As Integer
        If (objAVDoc.Open(FilePath, "")) Then
            objPDDoc = objAVDoc.GetPDDoc
            Return objPDDoc.GetNumPages()
        Else
            Return -1
        End If
    End Function


End Class

