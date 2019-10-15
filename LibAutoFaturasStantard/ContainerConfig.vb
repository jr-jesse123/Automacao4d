Imports Autofac
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports BibliotecaAutomacaoFaturas
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "BibliotecaAutomacaoFaturas" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
#Disable Warning BC40056 ' Namespace ou tipo especificado na Imports "consoleteste2" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports consoleteste2
#Enable Warning BC40056 ' Namespace ou tipo especificado na Imports "consoleteste2" não contém membro público ou não pode ser encontrado. Certifique-se que o namespace ou o tipo está definido e contém pelo menos um membro público. Certifique-se que o nome do elemento importado não usa alias.
Imports System.Reflection

Public Class ContainerConfig

    Public Shared Function Configure() As IContainer
        Dim builder = New ContainerBuilder


        builder.RegisterType(Of LoginPageTim).As(Of ILoginPageTim)()
        builder.RegisterType(Of RoboFaturasVIVOMOVEL)
        builder.RegisterType(Of ContaPageTim).As(Of IContaPageTim)()
        builder.RegisterType(Of TratadorDeFaturasPDF)
        builder.RegisterType(Of LeitorPDF)
        builder.RegisterType(Of Regexer)
        builder.RegisterType(Of GoogleDriveAPI)
        builder.RegisterType(Of ApiBitrix)
        builder.RegisterType(Of GeradorFatura)
        builder.RegisterType(Of DadosRegex)
        builder.RegisterType(Of LoginPageClaro).As(Of IloginPageClaro)()
        builder.RegisterType(Of ContaPageClaro).As(Of IContaPageClaro)()
        builder.RegisterType(Of RoboFaturasClaro)
        builder.RegisterType(Of RoboFaturasTIM)
        'builder.RegisterType(Of RoboFaturasALGAR)
        'builder.RegisterType(Of LoginPageAlgar).As(Of IloginPageAlgar)()
        'builder.RegisterType(Of ContaPageAlgar).As(Of IContaPageAlgar)()
        builder.RegisterType(Of TratadorDeFaturasCsv)
        builder.RegisterType(Of LoginPageVivoMovel).As(Of IloginPageVIVOMOVEL)()
        builder.RegisterType(Of ContaPageVIVOMOVEL).As(Of IContaPageVivoMovel)()
        builder.RegisterType(Of RoboFaturasTIM)
        builder.RegisterType(Of RoboVivoFixo)
        builder.RegisterType(Of LoginPageVivoFixo).As(Of IloginPageVivoFixo)()
        builder.RegisterType(Of ContaPageVivoFixo).As(Of IContaPageVivoFixo)()
        builder.RegisterType(Of RoboFaturasOI)
        builder.RegisterType(Of ContaPageOi).As(Of IContaPageOI)()
        builder.RegisterType(Of LoginPageOi).As(Of ILoginPageOI)()


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
