Public Class GeradorFatura

    Private proximoMes, mesAnterior, ProximoAno, AnoAnterior As Integer
    Private VencimentoProximoMes, VencimentoUltimoMes, VencimentoMesAtaual As Date
    Private DiffProximoMes, DiffUltimoMes, DiffMesAtual As Integer

    Friend Function GerarObjetoFaturaSeElegivel(conta As Conta) As Boolean


        Dim NovoVencimento As Date
        Dim CriarFatura As Boolean

        DefinirProximoMesMmesAnterior(conta.Vencimento)

        CriarFatura = DecidirSeCriaFatura(conta)



        If CriarFatura Then

            If DiffMesAtual <= DiffUltimoMes And DiffMesAtual <= DiffProximoMes Then
                NovoVencimento = VencimentoMesAtaual
            ElseIf DiffUltimoMes <= DiffMesAtual And DiffUltimoMes <= DiffProximoMes Then
                NovoVencimento = VencimentoUltimoMes
            ElseIf DiffProximoMes <= DiffMesAtual And DiffProximoMes <= DiffUltimoMes Then
                NovoVencimento = VencimentoProximoMes
            End If

            Dim fatura As New Fatura With {.Vencimento = NovoVencimento, .Baixada = False, .Tratada = False, .Pendente = True, .Aprovada = False,
                .Conferida = False, .NrConta = conta.NrDaConta}
            fatura.LogRobo.Add("Fatura criada em: " + Now.ToShortDateString)

            If fatura.Vencimento = "01/01/0001" Then
                Throw New Exception("Erro ao criar Data de Vencimento")
            End If

            Try
                conta.Faturas.Add(fatura)
            Catch ex As NullReferenceException
                conta.Faturas = New List(Of Fatura) From {
                    fatura
                }
            End Try


        End If

        Return CriarFatura

    End Function

    Private Function DecidirSeCriaFatura(conta As Conta) As Boolean

        Dim output As Boolean

        If conta.Faturas Is Nothing Then
            Return True
        ElseIf conta.Faturas.Count = 0 Then
            Return True

        ElseIf conta.Faturas.Last.Vencimento.Day <> conta.Vencimento Then
            Return True

        ElseIf conta.Faturas.LastOrDefault Is Nothing Then
            output = True

        ElseIf conta.faturas.LastOrDefault.Vencimento > Today Then
            output = False

        ElseIf (DiffProximoMes <= 15 And VencimentoProximoMes > Today) Or
            (DiffMesAtual <= 15 And VencimentoMesAtaual > Today) Then

            output = True
        Else
            output = False
        End If

        Return output

    End Function

    Private Sub DefinirProximoMesMmesAnterior(Vencimento As Integer)


        If Today.Month = 1 Then
            mesAnterior = 12
            proximoMes = 2
            ProximoAno = Today.Year
            AnoAnterior = Today.Year - 1
        ElseIf Today.Month = 12 Then
            mesAnterior = 11
            proximoMes = 1
            ProximoAno = Today.Year + 1
            AnoAnterior = Today.Year
        Else
            mesAnterior = Today.Month - 1
            proximoMes = Today.Month + 1
            ProximoAno = Today.Year
            AnoAnterior = Today.Year
        End If

        VencimentoProximoMes = New Date(ProximoAno, proximoMes, Vencimento)
        VencimentoUltimoMes = New Date(AnoAnterior, mesAnterior, Vencimento)
        VencimentoMesAtaual = New Date(Today.Year, Today.Month, Vencimento)

        DiffProximoMes = Math.Abs(DateDiff(DateInterval.Day, Today, VencimentoProximoMes))
        DiffUltimoMes = Math.Abs(DateDiff(DateInterval.Day, Today, VencimentoUltimoMes))
        DiffMesAtual = Math.Abs(DateDiff(DateInterval.Day, Today, VencimentoMesAtaual))

    End Sub
End Class
