Imports System.ComponentModel
Imports LibAutoFaturasStantard

Public Class ControleConta
    Implements INotifyPropertyChanged
    Private _conta As Conta

    Property Empresas As List(Of Empresa)
    Property Gestores As List(Of Gestor)
    Property Operadora As OperadoraEnum

    Public Property Gestor() As Gestor
        Get
            Return Conta.Gestores.FirstOrDefault
        End Get
        Set(ByVal value As Gestor)

            Conta.Gestores.Clear()
            Conta.Gestores.Add(value)
            Conta.GestoresID.Clear()
            Conta.GestoresID.Add(Gestor.CPF)
        End Set
    End Property


    Public Property Empresa() As Empresa
        Get
            Return Conta.Empresa
        End Get
        Set(ByVal value As Empresa)
            Conta.Empresa = value
            Conta.EmpresaID = value.CNPJ
        End Set
    End Property



    Public Shared ReadOnly ContaProperty As DependencyProperty =
    DependencyProperty.Register("Conta", GetType(Conta), GetType(ControleConta),
     New PropertyMetadata(New Conta() With {.NrDaConta = "teste"}, AddressOf OnValueChanged))


    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Property Conta() As Conta
        Get
            Return CType(GetValue(ContaProperty), Conta)
        End Get
        Set(ByVal value As Conta)
            SetValue(ContaProperty, value)

            If GestorCB.SelectedItem IsNot Nothing Then
                value.Gestores.Clear()
                value.Gestores.Add(GestorCB.SelectedItem)
            End If

            NotifyPropertyChanged("Conta")
        End Set
    End Property

    Private Overloads Shared Sub OnValueChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)

        Dim Controle = CType(d, ControleConta)
        If Controle IsNot Nothing Then Controle.OnValueChanged(e)

    End Sub

    Private Overloads Sub OnValueChanged(e As DependencyPropertyChangedEventArgs)

        CbEmpresa.SelectedItem = CType(e.NewValue, Conta).Empresa
        GestorCB.SelectedItem = CType(e.NewValue, Conta).Gestores.First
        NrDaconta.Text = CType(e.NewValue, Conta).NrDaConta
        Target.Text = CType(e.NewValue, Conta).Target
        IdBitrix.Text = CType(e.NewValue, Conta).ContaTriagemBitrixID
        Vencimento.Text = CType(e.NewValue, Conta).Vencimento
        Departamento.Text = CType(e.NewValue, Conta).Setor
        Pasta.Text = CType(e.NewValue, Conta).Pasta
        Drive.Text = CType(e.NewValue, Conta).Drive
        OperadoraCB.SelectedItem = CType(e.NewValue, Conta).Operadora
        TipoDeConta.SelectedItem = CType(e.NewValue, Conta).TipoDeConta
        SubtipoCB.SelectedItem = CType(e.NewValue, Conta).Subtipo



    End Sub


    Sub New()



        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        Empresas = GerRelDB.Empresas
        Gestores = GerRelDB.Gestores

        CbEmpresa.ItemsSource = Empresas
        GestorCB.ItemsSource = Gestores



        OperadoraCB.ItemsSource = [Enum].GetValues(GetType(OperadoraEnum))
        TipoDeConta.ItemsSource = [Enum].GetValues(GetType(TipoContaEnum))
        SubtipoCB.ItemsSource = [Enum].GetValues(GetType(SubtipoEnum))

        'CbEmpresa.SetBinding()
    End Sub


    Private Sub NotifyPropertyChanged(ByVal info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub
End Class
