Imports System.IO
Imports System.Reflection
Imports LibGit2Sharp
Public Class Git2Sharp



    Sub teste()

        Dim output As String

        Dim asm As Assembly = Assembly.GetExecutingAssembly()

        Dim binFolder As String = System.IO.Path.GetDirectoryName(asm.Location)

        Do
            Exit Do
        Loop


        Dim GitFolder = Path.GetDirectoryName(binFolder + "\..\..")

        Using repo = New Repository(GitFolder)

            Console.WriteLine(repo.Branches)

        End Using

    End Sub

End Class
