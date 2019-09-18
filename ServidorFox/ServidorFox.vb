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

                Dim data = LeitorClient.ReadLine
                Try
                    ExecutarProcesso(data)
                    writer.WriteLine("OK")

                Catch ex As Exception
                    writer.WriteLine(ex.Message)
                Finally
                    writer.Flush()
                    Client.Close()
                End Try


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
        Dim Mes = tokens(2).Substring(0, 2)

        LimparPastaDestino(operadora, NrFatura)

        AtivarFoxProw(operadora)

        Dim path As String = CriarPastaNoWebApp(operadora, NrFatura, Mes)


        'CopiarArquivosParaPastaDaEmpresa(operadora, NrFatura, path, Mes)

        CopiarArquivosParaPastaWebApp(operadora, NrFatura, path, Mes)




    End Function

    Private Sub CopiarArquivosParaPastaDaEmpresa(operadora As String, nrFatura As String, path As String, mes As String)

        Dim arquivos = Directory.GetFiles($"\\servidor\4D_CONSULTORIA\AUTO\{operadora}_REL")

        For Each arquivo In arquivos
            If arquivo.Contains(nrFatura) And arquivo.Contains(mes) Then
                Dim nome = IO.Path.GetFileName(arquivo)
                File.Copy(arquivo, $"{path}\{nome}")
            End If
        Next



    End Sub

    Private Sub LimparPastaDestino(operadora As String, nrConta As String)
        Dim arquivos = Directory.GetFiles($"\\servidor\4D_CONSULTORIA\AUTO\{operadora}_REL")

        If File.Exists($"c:\sistema4d\{operadora}dados\o{nrConta}.dbf") Then
            File.Delete($"c:\sistema4d\{operadora}dados\o{nrConta}.dbf")
        End If

        For Each arquivo In arquivos
            File.Delete(arquivo)
        Next

    End Sub

    Private Sub CopiarArquivosParaPastaWebApp(operadora As String, nrFatura As String, path As String, mes As String)

        Dim arquivos = Directory.GetFiles($"\\servidor\4D_CONSULTORIA\AUTO\{operadora}_REL")

        For Each arquivo In arquivos
            If arquivo.Contains(nrFatura) And arquivo.Contains(mes) Then
                Dim nome = IO.Path.GetFileName(arquivo)
                File.Copy(arquivo, $"{path}\{nome}")
            End If
        Next


    End Sub

    Private Function CriarPastaNoWebApp(operadora As String, nrFatura As String, mes As String) As String

        'criar uma pasta no drive compartilhado.
        Dim path = $"W:\4D\{operadora}\{nrFatura}\{mes}"
        Directory.CreateDirectory(path)

        Return path

    End Function

    Private Sub AtivarFoxProw(operadora As String)
        Dim concluido As Boolean
        Dim tempomax As Integer = 300
        Dim cont As Integer

        'limpar pasta de relatórios

        Dim arquivos = Directory.GetFiles($"\\Servidor\4d_consultoria\AUTO\{operadora}_REL")

        For Each arquivo In arquivos
            File.Delete(arquivo)
        Next

        '**********

inicio:
        Dim ProcessoFox As New Process
        ProcessoFox.StartInfo.FileName = $"\\Servidor\4d_consultoria\AUTO\{operadora}SQL.lnk"
        ProcessoFox.Start()

        While Not concluido
            concluido = ProcessoFox.WaitForExit(1000)
            cont += 1
            If cont > tempomax Then
                ProcessoFox.Kill()
                Throw New Exception("O Servidor FoxProw não conseguiu procesar esta fatura no prazo máximo de 5 minutos")

            End If
        End While



        arquivos = Directory.GetFiles($"\\servidor\4D_CONSULTORIA\AUTO\{operadora}_REL")


        If cont < 2 Then
            GoTo inicio
        ElseIf arquivos.Count = 0 Then
            Throw New Exception("O Servidor não conseguiu processar a fatura.")

        End If

    End Sub
End Class
