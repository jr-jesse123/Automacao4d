Imports BibliotecaAutomacaoFaturas

Public Class GerRelDB
    Private Const _empresasCollection As String = "Empresas"
    Private Const _contasCollection As String = "Contas"
    Private Const _gestoresCollection As String = "Gestores"
    Private Const _ResponsaveisCollection As String = "Responsaveis"
    Private Const _UsuariosCollection As String = "Usuarios"
    Private Shared _conexao As New MongoDb("AUTOMACAO4D")
    Private Shared _empresas As List(Of Empresa)
    Public Shared Event BancoAtualizado()
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
        _conexao.DeleTarGestor(gestor)
        RaiseEvent BancoAtualizado()

    End Sub

    Public Sub removerConta(conta As Conta)

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
        _conexao.UpsertRecord(Empresa)
        RaiseEvent BancoAtualizado()
    End Sub

    Public Shared Sub UpsertConta(Conta As Conta)
        _conexao.UpsertRecord(Conta)
        RaiseEvent BancoAtualizado()
    End Sub

    Public Shared Sub UpsertGestor(gestor As Gestor)
        _conexao.UpsertRecord(gestor)
        RaiseEvent BancoAtualizado()
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
        _conexao.DeleRecord(Of Empresa)("Empresas", empresa.CNPJ)
        RaiseEvent BancoAtualizado()
    End Sub

    Public Shared Sub EnviarLogFatura(fatura As Fatura, Log As String, dadosok As Boolean)

        Dim conta = Contas.Where(Function(x) x.Faturas.Contains(fatura)).First

        conta.DadosOk = dadosok
        Conta.Faturas.Last.LogRobo.Add(Log)

        _conexao.UpsertRecord(conta)

    End Sub

    Public Shared Sub EnviarLogFatura(fatura As Fatura, Log As String)

        Dim conta = Contas.Where(Function(x) x.Faturas.Contains(fatura)).First

        fatura.LogRobo.Add($"{Log} em {Now.ToShortDateString} às {Now.ToShortTimeString}")
        _conexao.UpsertRecord(conta)

    End Sub

End Class



