Imports LibAutoFaturasStantard

Public Class ContasView

    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        OperadoraCB.ItemsSource = [Enum].GetNames(GetType(OperadoraEnum))
        TipoDeContaCB.ItemsSource = [Enum].GetNames(GetType(TipoContaEnum))
        SubtipoCB.ItemsSource = [Enum].GetNames(GetType(SubtipoEnum))
    End Sub

End Class
