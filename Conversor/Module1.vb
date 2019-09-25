Imports BibliotecaAutomacaoFaturas

Module Module1

    Sub Main()

        Console.WriteLine("digite o nome ou o path do arquivo pdf a ser convertido")
        Dim arquivopdf As String = Console.ReadLine

        Dim conversor As New LeitorPDF(New Regexer)

        Try
            conversor.ConverterPdfParaTxt(arquivopdf, arquivopdf.Replace("pdf", "txt"))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.ReadLine()
            Throw
        End Try


        Console.WriteLine("conversão concluida")
        Console.ReadLine()

    End Sub

End Module
