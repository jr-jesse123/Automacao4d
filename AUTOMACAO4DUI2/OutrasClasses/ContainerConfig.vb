Imports Autofac
Imports LibAutoFaturasStantard

Public Class ContainerConfig

    Public Shared Function Configure() As IContainer
        Dim builder = New ContainerBuilder

        'builder.RegisterType(Of ContainerWebdriver)().As(Of IContainerWebdriver)()

        Return builder.Build
    End Function
End Class

'Me.container = ContainerConfig.Configure

'Dim D = container.Resolve(Of IAppDomainSetup)


'Using x = container.BeginLifetimeScope
''    Dim app = x.Resolve(Of 'INTERFACE INICIAL)
''   APP.run
'End Using
