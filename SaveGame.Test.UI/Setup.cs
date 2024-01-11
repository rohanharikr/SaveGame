using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveGame.Test.UI
{
    public class Setup
    {
        WindowsDriver<WindowsElement> driver;
        string appWorkingDirPath;
        string appPath;
        AppiumOptions options;

        public Setup()
        {
            appWorkingDirPath = Path.GetFullPath(@"../../../../SaveGame/bin/Debug/net8.0-windows");
            appPath = Path.Combine(appWorkingDirPath, "SaveGame.exe");
            options = new AppiumOptions();
            options.AddAdditionalCapability("app", appPath);
            options.AddAdditionalCapability("appWorkingDir", appWorkingDirPath);
        }

        [SetUp]
        public void StartUp()
        {
            driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
        }
    }
}
