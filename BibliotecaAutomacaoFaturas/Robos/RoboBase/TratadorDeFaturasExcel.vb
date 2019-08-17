Imports System.Data.OleDb
Imports System.IO
Imports System.IO.Compression
Imports System.Text.RegularExpressions
Imports BibliotecaAutomacaoFaturas

Public Class TratadorDeFaturasCsv
    Inherits TratadorDeFAturasBase

    Public Sub New(DriveApi As GoogleDriveAPI, ApiBitrix As ApiBitrix)
        MyBase.New(DriveApi, ApiBitrix)
    End Sub

    Protected Overrides Property extensaodoarquivo As String = ".zip"

    Protected Overrides Sub ExtrairInformacoesDaFatura(FATURA As Fatura)
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Sub ExtrairFaturaSeNecessario()




        Try
            ZipFile.ExtractToDirectory(ArquivoPath, WebdriverCt._folderContas)
        Catch ex As IOException

            Dim verifica As New Regex("(\w:.+)'")
            Dim PathArquivoAntigo = verifica.Match(ex.Message).Groups(1).Value
            Dim ArquivoAntigo As New FileInfo(PathArquivoAntigo)
            ArquivoAntigo.Delete()
            ZipFile.ExtractToDirectory(ArquivoPath, WebdriverCt._folderContas)
        Finally
            extensaodoarquivo = ".csv"
            EncontrarPathUltimoArquivo()
        End Try
    End Sub

    Protected Overrides Sub ProcessarFatura()
        Dim ListaEventos As List(Of String()) = File.ReadAllLines(ArquivoPath).Select(Function(a) a.Split(";")).ToList

    End Sub
End Class

Public Class RelatorioCsv
    Inherits DataTable
    Sub New(ListaEventos As List(Of String()))

        For i = 0 To ListaEventos.Count - 1
            If i = 0 Then
                For Each coluna In ListaEventos(0)
                    Me.Columns.Add(coluna)
                Next
            Else
                For z = 0 To ListaEventos(i).Count - 1
                    Dim linha = Me.NewRow
                    linha(ListaEventos(0)(z)) = ListaEventos(i)(z)
                Next
            End If


        Next

    End Sub

End Class