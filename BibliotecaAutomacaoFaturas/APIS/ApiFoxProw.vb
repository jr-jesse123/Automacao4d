Imports System.IO
Imports System.Net.Sockets

''' <summary>
''' api feita para obterprocessamento de fatura posicionada na pasta compartilhada do servidor.
''' </summary>
Public Class ApiClienteFoxProw

    Public Client As TcpClient
    Public DataStream As StreamWriter
    Public reader As StreamReader

    Public Sub New()
        'CLIENT 
        Client = New TcpClient("192.168.244.112", 64000)
        DataStream = New StreamWriter(Client.GetStream)
        reader = New StreamReader(Client.GetStream)
    End Sub
    ''' <summary>
    ''' Solicita Processo de fatura ao servidor
    ''' </summary>
    ''' <param name="operadora"></param>
    ''' <param name="NrFatura"></param>
    ''' <param name="Ref"></param>
    ''' <returns>String com o resultado do processamento.</returns>
    Public Function SolicitarProcessamento(operadora As String, NrFatura As String, Ref As String) As String

        Dim data = $"{operadora};{NrFatura};{Ref}"
        DataStream.WriteLine(data)
        DataStream.Flush()

        Threading.Thread.Sleep(100)

        Dim resposta = reader.ReadLine
        Return resposta


        Stop

    End Function


End Class
