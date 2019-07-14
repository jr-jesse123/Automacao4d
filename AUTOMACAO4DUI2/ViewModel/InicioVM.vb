

Public Class InicioVM

    Public Property InicioBtnClientesCommand As InicioBtnClientesCommand
    Public Property InicioBtnContasCommand As InicioBtnContasCommand
    Public Property InicioBtnGestoresCommand As InicioBtnGestoresCommand


    Sub New()
        InicioBtnClientesCommand = New InicioBtnClientesCommand(Me)
        InicioBtnContasCommand = New InicioBtnContasCommand(Me)
        InicioBtnGestoresCommand = New InicioBtnGestoresCommand(Me)
    End Sub


    End Class

