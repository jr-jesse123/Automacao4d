Imports BibliotecaAutomacaoFaturas

Public Class GerRelDB
    Private Const _empresasCollection As String = "Empresas"
    Private Const _contasCollection As String = "Contas"
    Private Const _gestoresCollection As String = "Gestores"
    Private Const _ResponsaveisCollection As String = "Responsaveis"


    Private Const _UsuariosCollection As String = "Usuarios"
    Private Shared _conexao As New MongoDb("AUTOMACAO4D")
    Public Shared Event BancoAtualizado()

    Private Shared _empresas As List(Of Empresa)
    Public Shared Property Empresas() As List(Of Empresa)
        Get
            If _empresas Is Nothing Then
                ObterClientesEstruturados()
            End If
            Return _empresas
        End Get
        Set(ByVal value As List(Of Empresa))
            _empresas = value
        End Set
    End Property

    Private Shared _contas As List(Of Conta)
    Public Shared Property Contas() As List(Of Conta)
        Get
            If _contas Is Nothing Then
                ObterClientesEstruturados()
            End If
            Return _contas
        End Get
        Set(ByVal value As List(Of Conta))
            _contas = value
        End Set
    End Property

    Public Shared Sub Refresh()
        ObterClientesEstruturados()
        RaiseEvent BancoAtualizado()
    End Sub

    Private Shared _gestores As List(Of Gestor)

    Public Shared Property Gestores() As List(Of Gestor)
        Get
            If _gestores Is Nothing Then
                ObterClientesEstruturados()
            End If
            Return _gestores
        End Get
        Set(ByVal value As List(Of Gestor))
            _gestores = value
        End Set
    End Property


    Shared Sub ObterClientesEstruturados() Handles Me.BancoAtualizado
        Empresas = _conexao.LoadRecords(Of Empresa)(_empresasCollection)
        Contas = _conexao.LoadRecords(Of Conta)(_contasCollection)
        Gestores = _conexao.LoadRecords(Of Gestor)(_gestoresCollection)

        For Each conta In Contas

            If conta.GeradorFatura.GerarObjetoFaturaSeElegivel(conta) Then
                _conexao.UpsertRecord(conta)
            End If
        Next

        CriarRelacioanmentosEmpresasXcontas(Empresas, Contas)
        CriarRelacionamentosContasXGestores(Contas, Gestores)

    End Sub

    Public Shared Sub removerGestor(gestor As Gestor)


        For Each _conta In Contas

            If _conta.Gestores.Contains(gestor) Then
                Throw New Exception("O Gestor não pode ser excluído, pois contém faturas. Excluas as faturas previamente!")
            End If
        Next

        _conexao.DeleTarGestor(gestor)
        RaiseEvent BancoAtualizado()

    End Sub

    Public Shared Sub removerConta(conta As Conta)

        _conexao.DeleTarConta(conta)
        RaiseEvent BancoAtualizado()

    End Sub

    Private Shared Sub CriarRelacionamentosContasXGestores(contas As List(Of Conta), gestores As List(Of Gestor))

        For Each gestor In gestores
            For Each conta In contas
                If conta.GestoresID.Contains(gestor.CPF) Then
                    conta.Gestores.Add(gestor)
                End If
            Next
        Next


    End Sub

    Private Shared Sub CriarRelacioanmentosEmpresasXcontas(empresas As List(Of Empresa), contas As List(Of Conta))

        For Each empresa In empresas
            For Each e In empresas
                If empresa.CNPJ = e.HoldingID Then
                    empresa.Filiais.Add(e)
                End If
            Next

            For Each conta In contas
                If empresa.CNPJ = conta.EmpresaID Then
                    empresa.Contas.Add(conta)
                    conta.Empresa = empresa
                End If
            Next
        Next

    End Sub

    Public Shared Sub UpsertEmpresa(Empresa As Empresa)
        If _conexao.ChecarExistencia(Empresa) Then
            _conexao.UpsertRecord(Empresa)
            RaiseEvent BancoAtualizado()
        Else
            Throw New Exception("Unidade Não CAdastrada Na base")
        End If
    End Sub

    Public Shared Sub UpsertConta(Conta As Conta)
        If _conexao.ChecarExistencia(Conta) Then
            _conexao.UpsertRecord(Conta)
            RaiseEvent BancoAtualizado()
        Else
            Throw New Exception("Unidade Não CAdastrada Na base")
        End If
    End Sub

    Public Shared Sub UpsertGestor(gestor As Gestor)
        If _conexao.ChecarExistencia(gestor) Then
            _conexao.UpsertRecord(gestor)
            RaiseEvent BancoAtualizado()
        Else
            Throw New Exception("Unidade Não CAdastrada Na base")
        End If

    End Sub
    Public Sub SalvarClientesEstruturado(ParamArray values() As Empresa)

        For Each empresa In Empresas
            UpsertEmpresa(empresa)

            For Each conta In empresa.Contas
                UpsertConta(conta)
            Next

            For Each Gestor In empresa.Gestores
                UpsertGestor(Gestor)
            Next

        Next

        RaiseEvent BancoAtualizado()
    End Sub

    Public Shared Sub RemoverEmpresa(empresa As Empresa)

        For Each _conta In Contas

            If _conta.Empresa.Equals(empresa) Then
                Throw New Exception("O Cliente não pode ser excluído, pois contém contas. Excluas as contas previamente!")
            End If

        Next

        _conexao.DeleRecord(Of Empresa)("Empresas", empresa.CNPJ)
        RaiseEvent BancoAtualizado()
    End Sub

    'mudar o nome para atualizarFaturacomlog
    Public Shared Sub AtualizarContaComLogNaFatura(fatura As Fatura, Log As String, Optional dadosok As Boolean = True)

        Dim conta = Contas.Where(Function(x) x.Faturas.Contains(fatura)).First

        conta.DadosOk = dadosok
        fatura.LogRobo.Add($"{Log} em {Now.ToShortDateString} às {Now.ToShortTimeString}")

        _conexao.UpsertRecord(conta)

    End Sub
    ''' <summary>
    ''' Esta função é utilizada para eventos que englobam toda a conta, como erro de login
    ''' </summary>
    ''' <param name="conta">objeto conta</param>
    ''' <param name="log">log opcional a ser persistido em todas as faturas</param>
    Public Shared Sub AtualizarContaComLogEmTodasAsFaturas(conta As Conta, Optional log As String = "", Optional dadosok As Boolean = True)

        conta.DadosOk = dadosok

        If log.Length > 0 Then
            For Each fatura In conta.Faturas
                fatura.LogRobo.Add($"{log} em {Now.ToShortDateString} às {Now.ToShortTimeString}")
            Next

        End If

        _conexao.UpsertRecord(conta)

    End Sub

    Shared Function SelecionarContasRobos(Robo As Object) As List(Of Conta)

        Dim operadora = Robo.Operadora
        Dim tipodeconta = Robo.TipoDeConta

        Dim output = Contas.Where(Function(conta)
                                      Return conta.Operadora = operadora And
                                                    conta.TipoDeConta = tipodeconta
                                  End Function) _
                                                .OrderBy(Function(conta) conta.Faturas.Last.Baixada) _
                                                .OrderBy(Function(conta) conta.Empresa.CNPJ) _
                                                .OrderBy(Function(conta) conta.Gestores.First.CPF).ToList

        Return output

    End Function


    Public Shared Sub AdicionarEmpresa(Empresa As Empresa)
        If _conexao.ChecarExistencia(Empresa) Then
            Throw New Exception("Unidade já existente")
        Else
            _conexao.UpsertRecord(Empresa)
            RaiseEvent BancoAtualizado()
        End If

    End Sub

    Public Shared Sub AdicionarConta(Conta As Conta)
        If _conexao.ChecarExistencia(Conta) Then
            Throw New Exception("Unidade já existente")
        Else
            _conexao.UpsertRecord(Conta)
        '    RaiseEvent BancoAtualizado()
        End If
    End Sub

    Public Shared Sub AdicionarGestor(gestor As Gestor)

        If _conexao.ChecarExistencia(gestor) Then
            Throw New Exception("Unidade já existente")
        Else
            _conexao.UpsertRecord(gestor)
            RaiseEvent BancoAtualizado()
        End If
    End Sub

    Public Shared Function EncontrarContaDeUmaFatura(fatura As Fatura) As Conta
        Return Contas.Where(Function(c) c.NrDaConta = fatura.NrConta).First
    End Function

End Class



