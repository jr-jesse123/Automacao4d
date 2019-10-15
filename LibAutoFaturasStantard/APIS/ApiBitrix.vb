Imports System.Net.Http
Imports System.Text.RegularExpressions
Imports System.Web

Public Class ApiBitrix
    Private Const hook = "https://4dconsultoria.bitrix24.com.br/rest/52/l3mea29nw1b1o21b/lists.element.add/?IBLOCK_TYPE_ID=lists&IBLOCK_ID=87"

#Disable Warning BC42356 ' Este método assíncrono não tem operadores 'Await' e será executado de modo síncrono. Considere o uso do operador 'Await' para aguardar chamadas à API não bloqueadoras ou 'Await Task.Run(...)' para fazer trabalhos associados à CPU em um thread em segundo plano.
    Async Function atualizaTriagem(ByVal idTriagem As Integer, ByVal REF As String, ByVal valor As Double, ByVal vencimento As Date, Optional ByVal creditos As Double = 0, Optional ByVal encargos As Double = 0) As Task(Of Integer)
#Enable Warning BC42356 ' Este método assíncrono não tem operadores 'Await' e será executado de modo síncrono. Considere o uso do operador 'Await' para aguardar chamadas à API não bloqueadoras ou 'Await Task.Run(...)' para fazer trabalhos associados à CPU em um thread em segundo plano.

        If (vencimento.Month - REF) > 1 And Not Math.Abs(vencimento.Month - REF) = -11 Then
            Throw New Exception($"a referencia informada foi {REF} e o vencimento informado foi {vencimento}, erro lançado por segurança")
        End If


        Dim vlrId = DateTime.Now.TimeOfDay.TotalSeconds.ToString
        vlrId = "&ELEMENT_CODE=" + vlrId.ToString

        Dim encargosstr = "&fields[PROPERTY_571]=" + encargos.ToString

        Dim creditosstr = "&fields[PROPERTY_569]=" + creditos.ToString

        Dim valorstr = "&fields[PROPERTY_567]=" + valor.ToString

        Dim refstr = "&fields[NAME]=" + REF.ToString

        Dim vencimentostr = "&fields[PROPERTY_581]=" + vencimento.ToString("MM/dd/yyyy")

        Dim idTriagemstr = "&fields[PROPERTY_573]=" + idTriagem.ToString

        Dim api = hook + vlrId + encargosstr + creditosstr + valorstr + refstr + idTriagemstr + vencimentostr
        api = api.Replace(",", ".")   'troca virgulas por pontos

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
