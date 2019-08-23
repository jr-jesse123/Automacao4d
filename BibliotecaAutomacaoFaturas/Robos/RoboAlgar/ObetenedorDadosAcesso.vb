Public Class ObtenedorDadosAcesso
    Shared Function ObterDadosAcesso(Conta As Conta) As DadosDeAcesso
        Dim output As DadosDeAcesso
        Try
            output = Conta.Empresa.ListaSenhas.Where(Function(lista)
                                                         Return lista.Operadora = Conta.Operadora And
                                                         lista.Tipo = Conta.TipoDeConta
                                                     End Function).First

        Catch ex As Exception

            output = New DadosDeAcesso With {
                .Login = Conta.Empresa.LoginContaOnline,
                .Senha = Conta.Empresa.SEnhaContaOnline}

        End Try

        Return output
    End Function
End Class