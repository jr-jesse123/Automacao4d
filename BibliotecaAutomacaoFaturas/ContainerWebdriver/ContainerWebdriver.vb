Imports BibliotecaAutomacaoFaturas
Imports OpenQA.Selenium.Chrome

Public Class WebdriverCt
    Private Shared _driver As ChromeDriver
    Public Shared ReadOnly _folderContas As String = AppDomain.CurrentDomain.BaseDirectory + "CONTAS"
    Public Shared ReadOnly Property Driver As ChromeDriver
        Get
            If _driver Is Nothing Then
                _driver = PrepararWebDriver()
            End If
            Return _driver
        End Get
    End Property

    Shared Sub ResetarWebdriver()
        For Each Window In Driver.WindowHandles
            Driver.SwitchTo.Window(Window)
            Driver.Close()
        Next
        Driver.Quit()

        _driver = PrepararWebDriver()


    End Sub

    Private Shared Function PrepararWebDriver() As ChromeDriver
        PrepararPastas()
        Dim ChromeOptions = New ChromeOptions()
        ChromeOptions.AddArguments("--incognito")
        ChromeOptions.AddArgument("no-sandbox")
        ChromeOptions.AddUserProfilePreference("download.default_directory", _folderContas)
        ChromeOptions.AddUserProfilePreference("download.prompt_for_download", False)
        'ChromeOptions.AddArgument("--headless")
        Dim Driver = New ChromeDriver(ChromeOptions)
        Driver.Manage.Timeouts.ImplicitWait = New TimeSpan(0, 0, 5)
        Driver.Manage.Timeouts.PageLoad = New TimeSpan(0, 10, 0)
        Driver.Manage.Timeouts.AsynchronousJavaScript = New TimeSpan(0, 3, 0)
        Driver.Manage.Window.Maximize()



        Return Driver

    End Function


    Private Shared Sub PrepararPastas()

        If (Not System.IO.Directory.Exists(_folderContas)) Then
            System.IO.Directory.CreateDirectory(_folderContas)
        End If

    End Sub

End Class
