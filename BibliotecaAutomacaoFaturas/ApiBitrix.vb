Imports System.Net.Http
Imports System.Web

Public Class ApiBitrix
    Private Const hook = "https://4dconsultoria.bitrix24.com.br/rest/52/l3mea29nw1b1o21b/lists.element.add/?IBLOCK_TYPE_ID=lists&IBLOCK_ID=87"

    Async Function atualizaTriagem(ByVal idTriagem As Integer, ByVal REF As String, ByVal valor As Double, ByVal vencimento As String, Optional ByVal creditos As Double = 0, Optional ByVal encargos As Double = 0) As Task(Of Boolean)


        Dim vlrId = Now.TimeOfDay.TotalSeconds
        vlrId = "&ELEMENT_CODE=" & vlrId

        Dim encargosstr = "&fields[PROPERTY_571]=" & encargos

        Dim creditosstr = "&fields[PROPERTY_569]=" & creditos

        Dim valorstr = "&fields[PROPERTY_567]=" & valor

        Dim refstr = "&fields[NAME]=" & REF

        Dim vencimentostr = "&fields[PROPERTY_581]=" & Mid(vencimento, 4, 3) & Left(vencimento, 2) & Right(vencimento, 5)

        Dim idTriagemstr = "&fields[PROPERTY_573]=" & idTriagem

        Dim api = hook & vlrId & encargosstr & creditosstr & valorstr & refstr & idTriagemstr & vencimentostr
        api = Replace(api, ",", ".")   'troca virgulas por pontos

        Dim MyRequest As New HttpClient


        'coloca a instância do WinHTTP na memória
        'Set MyRequest = CreateObject("WinHttp.WinHttpRequest.5.1")
        Dim Respota As HttpResponseMessage = Await MyRequest.GetAsync(api)

        Respota.ToString()

        If Respota.ToString Like "*result*" Then
            Return True
        Else
            Return False
        End If


    End Function



End Class
