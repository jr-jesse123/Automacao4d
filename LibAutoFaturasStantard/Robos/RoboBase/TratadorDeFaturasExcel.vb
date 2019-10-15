#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "System.Data.OleDb" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports System.Data.OleDb
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "System.Data.OleDb" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports System.IO
Imports System.IO.Compression
Imports System.Text.RegularExpressions
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.

Public Class TratadorDeFaturasCsv
    Inherits TratadorDeFAturasBase

    Public Sub New(DriveApi As GoogleDriveAPI, ApiBitrix As ApiBitrix)
        MyBase.New(DriveApi, ApiBitrix)
    End Sub

    Protected Overrides Property extensaodoarquivo As String = ".zip"

    Public Overrides Function ConverterPdfParaTxtEextrairRelatorios(FATURA As Fatura) As String
        Dim ArquivoPath = FATURA.InfoDownloads.First.path
        Dim ListaEventos As List(Of String()) = File.ReadAllLines(ArquivoPath).Select(Function(a) a.Split(";")).ToList
        Dim relatorioCsv As New RelatorioCsv(ListaEventos)

        FATURA.RelatoriosExcel = relatorioCsv

        Return ""

    End Function

    Public Overrides Sub ExtrairArquivoFaturaSeNecessario(fatura As Fatura)
        Dim ArquivoPath = fatura.InfoDownloads.First.path
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
            ArquivoPath = ArquivoPath.Replace("zip", "csv")
            extensaodoarquivo = ".zip"
        End Try
    End Sub


    Protected Overrides Sub AdicionarInformacoesFatura(fatura As Fatura)

        Dim total As Double = fatura.RelatoriosExcel.Compute("SUM(VALOR_BRUTO)", "")
        Dim creditos As Double = 0 'fatura.RelatoriosExcel.Compute("SUM(TARIFA)", "TARIFA < 0")
        Dim encargos As Double = 0 'fatura.RelatoriosExcel.Compute("SUM(TARIFA)", "TARIFA < 0")

        fatura.Total = total
        fatura.Creditos = creditos
        fatura.Encargos = encargos



    End Sub

    Public Overrides Sub ProcessarFaturaFox(fatura As Fatura)
        Throw New NotImplementedException()
    End Sub

End Class
