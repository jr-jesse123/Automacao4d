Imports System.Runtime.CompilerServices

Module Extensoes

    <Extension()>
    Public Sub SomarRotinas(Rotinas As Action())

        For Each Rotina In Rotinas
            Rotina.Invoke
        Next

    End Sub

    <Extension()>
    Public Function RemoverCaracter(ByVal aString As String, ParamArray TextosARetirar As String())

        Dim output As String = aString
        For Each texto In TextosARetirar
            output = output.Replace(texto, "")
        Next

        Return output
    End Function

End Module
