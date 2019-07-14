Imports BibliotecaAutomacaoFaturas

Public Class ControleEmpresa

    Private _empresa As Empresa


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
            LoginContaOnline.Text = CType(e.NewValue, Empresa).LoginContaOnline
            txtBoxSenhaContaOnline.Text = CType(e.NewValue, Empresa).SEnhaContaOnline
            IDBitrix.Text = CType(e.NewValue, Empresa).BitrixID
        Catch ex As NullReferenceException

            RazaoSocial.Text = CType(e.OldValue, Empresa).Nome
            NomeFantasia.Text = CType(e.OldValue, Empresa).NomeFantasia
            CNPJ.Text = CType(e.OldValue, Empresa).CNPJ
            CNPJHOLDING.Text = CType(e.OldValue, Empresa).HoldingID
            LoginContaOnline.Text = CType(e.OldValue, Empresa).LoginContaOnline
            txtBoxSenhaContaOnline.Text = CType(e.OldValue, Empresa).SEnhaContaOnline
            IDBitrix.Text = CType(e.OldValue, Empresa).BitrixID
        End Try


    End Sub

End Class
