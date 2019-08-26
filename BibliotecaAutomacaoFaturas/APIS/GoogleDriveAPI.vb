Imports System.Threading
Imports Google
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Download
Imports Google.Apis.Drive.v3
Imports Google.Apis.Drive.v3.Data
Imports Google.Apis.Logging
Imports Google.Apis.Services
Imports Google.Apis.Upload
Imports Google.Apis.Util.Store


Public Class GoogleDriveAPI

    ' If modifying these scopes, delete your previously saved credentials
    ' at ~/.credentials/drive-dotnet-quickstart.json
    Property Scopes As String() = {DriveService.Scope.DriveReadonly, DriveService.Scope.Drive}
    Property ApplicationName As String = "Drive API .NET Quickstart"

    Public Sub DeleteFile(id As String)

        Dim driveservice = GetService()
        driveservice.Files.Delete(id)

    End Sub

    Public Function GetFiles()

        Dim driveservice = GetService()
        Dim ListaArquivos As New List(Of Google.Apis.Drive.v3.Data.File)

        Dim request = driveservice.Files.List
        request.PageSize = 1000

        Dim arquivos = request.Execute()

        request.PageToken = arquivos.NextPageToken
        Do Until String.IsNullOrEmpty(request.PageToken)

            For Each arquivo In arquivos.Files
                ListaArquivos.Add(arquivo)
            Next

            'Listarquivos.AddRange(arquivos)
            request.PageToken = arquivos.NextPageToken
            arquivos = request.Execute()

        Loop

        Return ListaArquivos

    End Function

    Public Function Upload(NomeDoArquivo As String, PastaId As String, ArquivoPath As String) As String

        Dim fileMetadata = New File With {
        .Name = NomeDoArquivo,
        .Parents = New List(Of String) _
        (New String() {PastaId})
        }

        Dim request As FilesResource.CreateMediaUpload

        Dim driveservice = GetService()

        Using stream = New System.IO.FileStream(ArquivoPath, System.IO.FileMode.Open)

            request = driveservice.Files.Create(fileMetadata, stream, "application/pdf")

            request.Fields = "id"

            request.Upload()
        End Using

        Dim file = request.ResponseBody

        If file IsNot Nothing Then
            Return file.Id
        Else
            Return ""
        End If

    End Function

    Private Function GetService() As DriveService

        Dim credential As UserCredential

        Using stream = New IO.FileStream("credentials.json", IO.FileMode.Open, IO.FileAccess.Read)

            ' The file token.json stores the user's access and refresh tokens, and is created
            ' automatically when the authorization flow completes for the first time.
            Dim credPath As String = "token.json"
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                    Scopes, "user", CancellationToken.None, New FileDataStore(credPath, True)).Result

        End Using

        ' Create Drive API service.

        Dim driveservice = New DriveService(New BaseClientService.Initializer() With
            {
                .HttpClientInitializer = credential,
                .ApplicationName = ApplicationName
            })

        Return driveservice

    End Function

End Class

