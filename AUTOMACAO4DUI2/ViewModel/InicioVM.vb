

Imports AUTOMACAO4DUI2
Imports BibliotecaAutomacaoFaturas

Public Class InicioVM

    Public Property InicioBtnClientesCommand As InicioBtnClientesCommand
    Public Property InicioBtnContasCommand As InicioBtnContasCommand
    Public Property InicioBtnGestoresCommand As InicioBtnGestoresCommand
    Public Property InicioBtnLogRoboCommand As InicioBtnLogRoboCommand


    Sub New()
        InicioBtnClientesCommand = New InicioBtnClientesCommand(Me)
        InicioBtnContasCommand = New InicioBtnContasCommand(Me)
        InicioBtnGestoresCommand = New InicioBtnGestoresCommand(Me)
        InicioBtnLogRoboCommand = New InicioBtnLogRoboCommand(Me)
    End Sub


End Class

Public Class InicioBtnLogRoboCommand
    Implements ICommand

    Private inicioVM As InicioVM

    Public Sub New(inicioVM As InicioVM)
        Me.inicioVM = inicioVM
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        Dim x As New FaturasView(GerRelDB.Contas)
        x.ShowDialog()


    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function
End Class
