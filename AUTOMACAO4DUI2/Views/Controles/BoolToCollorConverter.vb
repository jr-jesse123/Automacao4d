Imports System.Globalization

Public Class BoolToCollorConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

        Dim condicao = CType(value, Boolean)
        Dim output
        If condicao Then
            output = New SolidColorBrush(CType(ColorConverter.ConvertFromString("#73E34D"), Color))
        Else
            output = New SolidColorBrush(CType(ColorConverter.ConvertFromString("#ff5c5c"), Color))
        End If

        Return output

    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
