﻿Imports System.Net.Http
Imports System.Text.RegularExpressions
Imports System.Web

Public Class ApiBitrix
    Private Const hook = "https://4dconsultoria.bitrix24.com.br/rest/52/l3mea29nw1b1o21b/lists.element.add/?IBLOCK_TYPE_ID=lists&IBLOCK_ID=87"

    Async Function atualizaTriagem(ByVal idTriagem As Integer, ByVal REF As String, ByVal valor As Double, ByVal vencimento As Date, Optional ByVal creditos As Double = 0, Optional ByVal encargos As Double = 0) As Task(Of Integer)


        Dim vlrId = Now.TimeOfDay.TotalSeconds.ToString
        vlrId = "&ELEMENT_CODE=" + vlrId.ToString

        Dim encargosstr = "&fields[PROPERTY_571]=" + encargos.ToString

        Dim creditosstr = "&fields[PROPERTY_569]=" + creditos.ToString

        Dim valorstr = "&fields[PROPERTY_567]=" + valor.ToString

        Dim refstr = "&fields[NAME]=" + REF.ToString

        Dim vencimentostr = "&fields[PROPERTY_581]=" + vencimento.ToString("MM/dd/yyyy")

        Dim idTriagemstr = "&fields[PROPERTY_573]=" + idTriagem.ToString

        Dim api = hook + vlrId + encargosstr + creditosstr + valorstr + refstr + idTriagemstr + vencimentostr
        api = Replace(api, ",", ".")   'troca virgulas por pontos

        Dim MyRequest As New HttpClient
        Dim Respota As HttpResponseMessage = MyRequest.GetAsync(api).Result

        Dim CorpoResposta = Respota.Content.ReadAsStringAsync().Result



        If Respota.StatusCode = 200 Then
            Dim verifica As New Regex("\d+")
            Dim id = verifica.Match(CorpoResposta)



            Return id.Value
        Else
            Return -1
        End If


    End Function



End Class