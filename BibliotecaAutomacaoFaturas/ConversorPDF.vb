Imports System.IO
Imports Acrobat
Imports iText7

Public Class ConversorPDF

    Public Sub getTextFromPDF(FilePath As String, DestinationPath As String)

        Dim AppAcrobat As AcroApp
        Dim objAVDoc As New AcroAVDoc
        Dim objPDDoc As New AcroPDDoc
        Dim objPage As AcroPDPage
        Dim objSelection As AcroPDTextSelect
        Dim objHighlight As AcroHiliteList
        Dim pageNum As Long
        Dim strText As String

        strText = "" ' esvazia a variável

        'prepara a conexão com o adobe
        If (objAVDoc.Open(FilePath, "")) Then
            objPDDoc = objAVDoc.GetPDDoc

            'abre o pdf uma página por vez, salva a página convertida na pasta da vivo
            For pageNum = 0 To objPDDoc.GetNumPages() - 1

                objPage = objPDDoc.AcquirePage(pageNum)
                objHighlight = New AcroHiliteList
                objHighlight.Add(0, 15000) ' Adjust this up if it's not getting all the text on the page
                objSelection = objPage.CreatePageHilite(objHighlight)

                If Not objSelection Is Nothing Then
                    For tCount = 0 To objSelection.GetNumText - 1
                        strText = strText & objSelection.GetText(tCount)
                    Next tCount
                End If

                Using x As New StreamWriter(DestinationPath, True)
                    x.Write(strText)
                End Using

                strText = "" ' esvazia a variável
            Next
        End If

    End Sub
End Class