Imports OpenQA.Selenium.Chrome

Public MustInherit Class DriverDependents
    Protected driver As ChromeDriver
    Sub New()
        Me.driver = WebdriverCt.Driver
    End Sub

End Class
