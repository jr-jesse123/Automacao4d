Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Public Class ServidorFox

    'server config
    Public ServerIP As IPAddress = IPAddress.Parse("192.168.244.112")
    Public ServerPort As Integer = 64000
    Public Server As TcpListener

    Public IsListenning As Boolean = True

    ' CLIENTS
    Private Client As TcpClient



    Public Sub New()
        Server = New TcpListener(ServerIP, ServerPort)
        Server.Start()

    End Sub


    Public Sub iniciar()
        'CREATE LOOP
        Do Until IsListenning = False
            'ACCPT INCOMING CONNECTIONS
            If Server.Pending Then
                Client = Server.AcceptTcpClient
                Dim LeitorClient = New StreamReader(Client.GetStream)
                Dim writer = New StreamWriter(Client.GetStream)

                Dim data = LeitorClient.ReadToEnd
                Try
                    ExecutarProcesso(data)
                    writer.WriteLine("Feito")

                Catch ex As Exception
                    writer.WriteLine("Falha")
                End Try

                writer.Flush()
                Thread.Sleep(100)

            End If


            ' REDUCE CPU USAGE
            Thread.Sleep(100)

        Loop
    End Sub

    Private Function ExecutarProcesso(data As String) As Integer

        Dim tokens = data.Split(";")
        Dim operadora = tokens(0)
        Dim NrFatura = tokens(1)
        Dim Mes = tokens(2)

        Dim concluido As Boolean

        Dim ProcessoFox As New Process
        ProcessoFox.StartInfo.FileName = $"\\Servidor\4d_consultoria\AUTO\{operadora}SQL.lnk"
        ProcessoFox.Start()

        While Not concluido
            concluido = ProcessoFox.WaitForExit(1000)
        End While

        Dim arquivos = Directory.GetFiles("\\servidor\4D_CONSULTORIA\AUTO\VIVO_REL")

        'criar uma pasta no drive compartilhado.
        Dim path = $"C:\pastaTeste\{operadora}\{NrFatura}\{Mes}"
        Directory.CreateDirectory(path)

        For Each arquivo In arquivos
            If arquivo.Contains(NrFatura) And arquivo.Contains(Mes) Then
                Dim nome = IO.Path.GetFileName(arquivo)
                File.Move(arquivo, $"{path}\{nome}")
            End If
        Next

        Dim output = Directory.GetFiles("\\servidor\4D_CONSULTORIA\AUTO\VIVO_REL")

    End Function
End Class
