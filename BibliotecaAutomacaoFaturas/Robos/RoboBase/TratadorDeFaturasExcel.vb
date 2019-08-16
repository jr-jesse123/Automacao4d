Imports System.Data.OleDb
Imports System.IO
Imports System.IO.Compression
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

        ZipFile.ExtractToDirectory(ArquivoPath, WebdriverCt._folderContas)
        ArquivoPath = ArquivoPath.Replace(Path.GetExtension(ArquivoPath), "*.csv")
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