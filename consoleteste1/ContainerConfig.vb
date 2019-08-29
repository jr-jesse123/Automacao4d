Imports Autofac
Imports BibliotecaAutomacaoFaturas
Imports consoleteste2
Imports System.Reflection

Public Class ContainerConfig

    Public Shared Function Configure() As IContainer
        Dim builder = New ContainerBuilder


        builder.RegisterType(Of LoginPageTim).As(Of ILoginPageTim)()
        builder.RegisterType(Of RoboFaturasVIVOMOVEL)
        builder.RegisterType(Of ContaPageTim).As(Of IContaPageTim)()
        builder.RegisterType(Of TratadorDeFaturasPDF)
        builder.RegisterType(Of ConversorPDF)
        builder.RegisterType(Of Regexer)
        builder.RegisterType(Of GoogleDriveAPI)
        builder.RegisterType(Of ApiBitrix)
        builder.RegisterType(Of GeradorFatura)
        builder.RegisterType(Of DadosRegex)
        builder.RegisterType(Of LoginPageClaro).As(Of IloginPageClaro)()
        builder.RegisterType(Of ContaPageClaro).As(Of IContaPageClaro)()
        builder.RegisterType(Of RoboFaturasClaro)
        builder.RegisterType(Of RoboFaturasALGAR)
        builder.RegisterType(Of LoginPageAlgar).As(Of IloginPageAlgar)()
        builder.RegisterType(Of ContaPageAlgar).As(Of IContaPageAlgar)()
        builder.RegisterType(Of TratadorDeFaturasCsv)
        builder.RegisterType(Of LoginPageVivoMovel).As(Of IloginPageVIVOMOVEL)()
        builder.RegisterType(Of ContaPageVIVOMOVEL).As(Of IContaPageVivoMovel)()



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
