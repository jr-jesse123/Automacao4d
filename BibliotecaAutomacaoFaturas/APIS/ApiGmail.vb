Imports BibliotecaAutomacaoFaturas

Friend Class ApiGmail

    Public Shared Sub NotificarDoracy(fatura As Fatura)

        Dim msg As String = "Olá Mestre Doracy, esta é uma mensagem atuática enviada pelo robô de faturas da 4d, para comunicar que a conta anexa neste e-mail não foi processada pelo fox prow no servidor.
O danilo está em cópia e pode fornecer qualquer coisa que vc precisar... valeuuuu!!"


        EnviaEmail(msg, New String() {"doracymarques@hotmail.com", "danilomeireles@hotmail.com"}, fatura)


    End Sub


    Shared Sub EnviaEmail(ByVal texto As String, ByRef destinatarios As String(), fatura As Fatura)
        Dim iMsg, iConf, Flds
        Dim schema

        Dim conta = GerRelDB.EncontrarContaDeUmaFatura(fatura)

        'Seta as variáveis, lembrando que o objeto Microsoft CDO deverá estar habilitado em Ferramentas->Referências->Microsoft CDO for Windows 2000 Library
        iMsg = CreateObject("CDO.Message")
        iConf = CreateObject("CDO.Configuration")
        Flds = iConf.Fields

        'Configura o componente de envio de email
        schema = "http://schemas.microsoft.com/cdo/configuration/"
        Flds.Item(schema & "sendusing") = 2
        'Configura o smtp
        Flds.Item(schema & "smtpserver") = "smtp.gmail.com"
        'Configura a porta de envio de email
        Flds.Item(schema & "smtpserverport") = 465
        Flds.Item(schema & "smtpauthenticate") = 1
        'Configura o email do remetente
        Flds.Item(schema & "sendusername") = "junior.jesse@gmail.com"
        'Configura a senha do email remetente
        Flds.Item(schema & "sendpassword") = "SenhaSegura12"
        Flds.Item(schema & "smtpusessl") = 1
        Flds.Update


        With iMsg
            'Email do destinatário
            .To = Join(destinatarios, ";")
            'Seu email
            .From = "junior.jesse@gmail.com"
            'Título do email
            .Subject = "AVISO DE CORTE CONTA: " & fatura.NrConta & " & CNPJ : " & conta.Empresa.CNPJ
            'Mensagem do e-mail, você pode enviar formatado em HTML
            .HTMLBody = texto
            'Seu nome ou apelido
            .Sender = "junior.jesse@gmail.com"
            'Nome da sua organização
            .Organization = "4D"
            'email de responder para
            '.ReplyTo = "teste@gmail.com"
            'Anexo a ser enviado na mensagem
            .AddAttachment(fatura.InfoDownloads.First.path)
            'Passa a configuração para o objeto CDO
            .Configuration = iConf
            'Envia o email
            .Send
        End With

        'Limpa as variáveis
        iMsg = Nothing
        iConf = Nothing
        Flds = Nothing


    End Sub
End Class
