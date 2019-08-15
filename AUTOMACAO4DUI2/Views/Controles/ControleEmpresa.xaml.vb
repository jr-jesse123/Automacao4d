Imports System.Collections.ObjectModel
Imports BibliotecaAutomacaoFaturas

Public Class ControleEmpresa

    Private _empresa As Empresa
    Public Property senhas As New ObservableCollection(Of DadosDeAcesso)

    Public Shared ReadOnly EmpresaProperty As DependencyProperty =
    DependencyProperty.Register("Empresa", GetType(Empresa), GetType(ControleEmpresa),
     New PropertyMetadata(New Empresa, AddressOf OnValueChanged))

    Public Property Empresa() As Empresa
        Get
            Return CType(GetValue(EmpresaProperty), Empresa)
        End Get
        Set(ByVal value As Empresa)
            SetValue(EmpresaProperty, value)

        End Set
    End Property

    Private Overloads Shared Sub OnValueChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)

        Dim controleEmpresa = CType(d, ControleEmpresa)
        If controleEmpresa IsNot Nothing Then controleEmpresa.OnValueChanged(e)

    End Sub

    Private Overloads Sub OnValueChanged(e As DependencyPropertyChangedEventArgs)
        Try
            RazaoSocial.Text = CType(e.NewValue, Empresa).Nome
            NomeFantasia.Text = CType(e.NewValue, Empresa).NomeFantasia
            CNPJ.Text = CType(e.NewValue, Empresa).CNPJ
            CNPJHOLDING.Text = CType(e.NewValue, Empresa).HoldingID

            IDBitrix.Text = CType(e.NewValue, Empresa).BitrixID
            For Each senha In CType(e.NewValue, Empresa).ListaSenhas
                senhas.Add(senha)
            Next


        Catch ex As NullReferenceException

            RazaoSocial.Text = CType(e.OldValue, Empresa).Nome
            NomeFantasia.Text = CType(e.OldValue, Empresa).NomeFantasia
            CNPJ.Text = CType(e.OldValue, Empresa).CNPJ
            CNPJHOLDING.Text = CType(e.OldValue, Empresa).HoldingID

            IDBitrix.Text = CType(e.OldValue, Empresa).BitrixID
            For Each senha In CType(e.OldValue, Empresa).ListaSenhas
                senhas.Add(senha)
            Next

        End Try


    End Sub

    Private Sub BotaoAdicionarSEnha_Click(sender As Object, e As RoutedEventArgs)
        Dim x As New AdicionarSenhaView(Empresa)

        AddHandler x.SenhasAlteradas, AddressOf AdicionarSEnhaView_ONSenhaAlterada

        x.ShowDialog()

    End Sub

    Private Sub AdicionarSEnhaView_ONSenhaAlterada()

        senhas.Clear()

        For Each senha In Empresa.ListaSenhas
            senhas.Add(senha)
        Next




    End Sub

    Private Sub BotaoRemoverSEnha_Click(sender As Object, e As RoutedEventArgs)

        If ListaSenhas.SelectedItem IsNot Nothing Then
            Empresa.ListaSenhas.Remove(ListaSenhas.SelectedItem)
            AdicionarSEnhaView_ONSenhaAlterada()
        End If


    End Sub
End Class
