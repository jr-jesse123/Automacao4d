Public Class GeradorFatura

    Private proximoMes, mesAnterior, ProximoAno, AnoAnterior As Integer
    Private VencimentoProximoMes, VencimentoUltimoMes, VencimentoMesAtaual As Date
    Private DiffProximoMes, DiffUltimoMes, DiffMesAtual As Integer

    Public Function GerarObjetoFaturaSeElegivel(conta As Conta) As Boolean

        Dim teste

        teste = True

        Dim NovoVencimento As Date
        Dim CriarFatura As Boolean

        Dim fatura As Fatura

        DefinirProximoMesMmesAnterior(conta.Vencimento)

        CriarFatura = DecidirSeCriaFatura(conta)


        If CriarFatura Then


            If conta.Faturas IsNot Nothing And conta.Faturas.Count > 0 Then

                Dim vencimentoAnterior As Date = conta.Faturas.Last.Vencimento

                If conta.Vencimento = vencimentoAnterior.Day Then
                    NovoVencimento = vencimentoAnterior.AddMonths(1)
                Else
                    If DateTime.Today.Month = vencimentoAnterior.Month Then
                        If conta.Vencimento > vencimentoAnterior.Day Then
                            NovoVencimento = vencimentoAnterior.AddDays(conta.Vencimento - vencimentoAnterior.Day)
                        Else
                            NovoVencimento = vencimentoAnterior.AddDays(conta.Vencimento - vencimentoAnterior.Day).AddMonths(1)
                        End If
                    Else
                        Dim fatura1 As New Fatura With {.Vencimento = NovoVencimento, .Baixada = False, .Tratada = False, .Pendente = True, .Aprovada = False,
                .Conferida = False, .NrConta = conta.NrDaConta}
                        fatura1.LogRobo.Add("Fatura criada em: " + DateTime.Now.ToShortDateString)
                        fatura1.LogRobo.Add("Fatura Criada para buscar valores proporcionais: " + DateTime.Now.ToShortDateString)
                        conta.Faturas.Add(fatura1)

                        NovoVencimento = vencimentoAnterior.AddDays(conta.Vencimento - vencimentoAnterior.Day).AddMonths(1)
                    End If

                End If

            Else

                If DiffMesAtual <= DiffUltimoMes And DiffMesAtual <= DiffProximoMes Then
                    NovoVencimento = VencimentoMesAtaual
                ElseIf DiffUltimoMes <= DiffMesAtual And DiffUltimoMes <= DiffProximoMes Then
                    NovoVencimento = VencimentoUltimoMes
                ElseIf DiffProximoMes <= DiffMesAtual And DiffProximoMes <= DiffUltimoMes Then
                    NovoVencimento = VencimentoProximoMes
                End If

            End If


            fatura = New Fatura With {.Vencimento = NovoVencimento, .Baixada = False, .Tratada = False, .Pendente = True, .Aprovada = False,
               .Conferida = False, .NrConta = conta.NrDaConta}
            fatura.LogRobo.Add("Fatura criada em: " + DateTime.Now.ToShortDateString)



            If fatura.Vencimento = "01/01/0001" Or fatura.Vencimento = Nothing Then
                Throw New Exception("Erro ao criar Data de Vencimento")

            ElseIf fatura.Vencimento.ToShortDateString = conta.Faturas.Last.Vencimento.ToShortDateString Then
                Throw New Exception("Erro ao criar fatura, data de vencimento repetida")
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
        Dim NrDias As Integer

        Dim today = DateTime.Today

        If conta.TipoDeConta = TipoContaEnum.FIXA Then
            NrDias = 25
        Else
            NrDias = 15
        End If



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

        ElseIf (DiffProximoMes <= NrDias And VencimentoProximoMes > Today) Or
            (DiffMesAtual <= NrDias And VencimentoMesAtaual > Today) Then

            output = True
        Else
            output = False
        End If

        Return output

    End Function

    Private Sub DefinirProximoMesMmesAnterior(Vencimento As Integer)

        Dim today = DateTime.Today

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



        DiffProximoMes = Math.Abs(CalcularDistanciaDataAtual(VencimentoProximoMes))
        DiffUltimoMes = Math.Abs(CalcularDistanciaDataAtual(VencimentoUltimoMes))
        DiffMesAtual = Math.Abs(CalcularDistanciaDataAtual(VencimentoMesAtaual))

    End Sub

    Private Function CalcularDistanciaDataAtual(Vencimento As Date) As Integer
        Dim today = DateTime.Today

        Dim difdias As Integer = Vencimento.Day - today.Day
        Dim difmes As Integer = Vencimento.Month - today.Month
        Dim difano As Integer = Vencimento.Year - today.Year

        Return difdias + (difmes * 30) + (difano * 360)



    End Function
End Class
