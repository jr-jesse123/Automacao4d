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



    Public Function Upload(NomeDoArquivo As String, PastaId As String, ArquivoPath As String) As String

        Dim fileMetadata = New File With {
        .Name = NomeDoArquivo,
        .Parents = New List(Of String) _
        (New String() {PastaId})
        }

        Dim request As FilesResource.CreateMediaUpload

        Dim credential As UserCredential

        Using stream = New IO.FileStream("credentials.json", IO.FileMode.Open, IO.FileAccess.Read)

            ' The file token.json stores the user's access and refresh tokens, and is created
            ' automatically when the authorization flow completes for the first time.
            Dim credPath As String = "token.json"
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                    Scopes, "user", CancellationToken.None, New FileDataStore(credPath, True)).Result
            Console.WriteLine("Credential file saved to: " + credPath)
        End Using

        ' Create Drive API service.
        Dim service = New DriveService(New BaseClientService.Initializer() With
            {
                .HttpClientInitializer = credential,
                .ApplicationName = ApplicationName
            })


        Using stream = New System.IO.FileStream(ArquivoPath, System.IO.FileMode.Open)

            Dim driveservice = New DriveService(New BaseClientService.Initializer() With
            {
                .HttpClientInitializer = credential,
                .ApplicationName = ApplicationName
            })


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
End Class



'Public Sub Main()

'    Dim credential As UserCredential

'    Using stream = New IO.FileStream("credentials.json", IO.FileMode.Open, IO.FileAccess.Read)

'        ' The file token.json stores the user's access and refresh tokens, and is created
'        ' automatically when the authorization flow completes for the first time.
'        Dim credPath As String = "token.json"
'        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
'                Scopes, "user", CancellationToken.None, New FileDataStore(credPath, True)).Result
'        Console.WriteLine("Credential file saved to: " + credPath)
'    End Using

'    ' Create Drive API service.
'    Dim service = New DriveService(New BaseClientService.Initializer() With
'        {
'            .HttpClientInitializer = credential,
'            .ApplicationName = ApplicationName
'        })

'    ' Define parameters of request.
'    Dim listRequest As FilesResource.ListRequest = service.Files.List()
'    'listRequest.PageSize = 10
'    listRequest.Fields = "nextPageToken, files(id, name)"

'    ' List files.
'    Dim files As IList(Of File) = listRequest.Execute().Files ' ' era .files

'    Console.WriteLine("Files:")

'    If files IsNot Nothing AndAlso files.Count > 0 Then

'        For Each arquivo In files
'            Console.WriteLine("{0} ({1})", arquivo.Name, arquivo.Id)
'        Next

'    Else

'        Console.WriteLine("No files found.")

'        Console.Read()
'    End If
'End Sub