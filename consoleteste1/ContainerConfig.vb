Imports Autofac
Imports BibliotecaAutomacaoFaturas
Imports System.Reflection

Public Class ContainerConfig

    Public Shared Function Configure() As IContainer
        Dim builder = New ContainerBuilder


        builder.RegisterType(Of LoginPageTim)
        builder.RegisterType(Of RoboFaturasTIM)
        builder.RegisterType(Of ContaPageTim)
        builder.RegisterType(Of TratadorDeFaturas)
        builder.RegisterType(Of ConversorPDF)
        builder.RegisterType(Of Regexer)
        builder.RegisterType(Of GoogleDriveAPI)
        builder.RegisterType(Of ApiBitrix)
        builder.RegisterType(Of GeradorFatura)
        builder.RegisterType(Of DadosRegex)
        builder.RegisterType(Of LoginPageClaro)
        builder.RegisterType(Of ContaPageAlgar)
        builder.RegisterType(Of RoboFaturasClaro)

        'LoginPageClaro

        'builder.RegisterAssemblyTypes(Assembly.Load(NameOf(BibliotecaAutomacaoFaturas))) _
        '.As(Function(t) t.GetInterfaces.FirstOrDefault(Function(i) i.Name = "I" +    t.Name))


        '.Where(Function(t) t.Namespace.Contains("demolibrary")) essa declaração limita a atuação a um namespace específico

        Return builder.Build
    End Function
End Class

'Me.container = ContainerConfig.Configure

'Dim D = container.Resolve(Of IAppDomainSetup)


'Using x = container.BeginLifetimeScope
''    Dim app = x.Resolve(Of 'INTERFACE INICIAL)
''   APP.run
'End Using
