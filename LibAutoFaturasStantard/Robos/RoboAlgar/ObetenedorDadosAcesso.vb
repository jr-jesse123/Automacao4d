Public Class ObtenedorDadosAcesso

    ''' <summary>
    ''' Este método retorna a primeira informação  dos dados de acesso da empresa de uma conta,
    ''' que coincidam com a operadora e o tipo de conta do objeto conta enviado.
    ''' para acessar os dados do gestor é necessário incluir o segundo parametro boolean como true
    ''' </summary>
    ''' <param name="Conta"> conta a ser verificada </param>
    ''' <param name="Gestor"> Defina verdadeiro para obter os dados de cesso do gestor ao inves da empresa</param>
    ''' <returns></returns>
    Shared Function ObterDAdosAcessoEmpresa(Conta As Conta) As DadosDeAcesso
        Dim output As DadosDeAcesso

        Try

                output = Conta.Empresa.ListaSenhas.Where(Function(lista)
                                                             Return lista.Operadora = Conta.Operadora And
                                                             lista.Tipo = Conta.TipoDeConta
                                                         End Function).First

            Catch ex As Exception

#Disable Warning BC40000 ' '"Public Property LoginContaOnline As String" está obsoleto: "A senha utilizada diretamente pela empresa está obsoleta, por favor utiliza a listaSenhas filtrando pela operadora que deseja buscar".
#Disable Warning BC40000 ' '"Public Property SEnhaContaOnline As String" está obsoleto: "A senha utilizada diretamente pela empresa está obsoleta, por favor utiliza a listaSenhas filtrando pela operadora que deseja buscar".
                output = New DadosDeAcesso With {
                    .Login = Conta.Empresa.LoginContaOnline,
                    .Senha = Conta.Empresa.SEnhaContaOnline}
#Enable Warning BC40000 ' '"Public Property SEnhaContaOnline As String" está obsoleto: "A senha utilizada diretamente pela empresa está obsoleta, por favor utiliza a listaSenhas filtrando pela operadora que deseja buscar".
#Enable Warning BC40000 ' '"Public Property LoginContaOnline As String" está obsoleto: "A senha utilizada diretamente pela empresa está obsoleta, por favor utiliza a listaSenhas filtrando pela operadora que deseja buscar".

            End Try


        Return output
    End Function

    Shared Function ObterDAdosAcessoGestor(Conta As Conta) As DadosDeAcesso
        Dim output As DadosDeAcesso

        output = Conta.Gestores.First.ListaSenhas.Where(Function(lista)
                                                                Return lista.Operadora = Conta.Operadora And
                                                             lista.Tipo = Conta.TipoDeConta
                                                            End Function).First

        Return output
    End Function

End Class