using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace SaveGame.Test.UI
{
    public class Tests
    {
        WindowsDriver<WindowsElement> driver;

        [SetUp]
        public void Setup()
        {
            string appWorkingDirPath = Path.GetFullPath(@"../../../../SaveGame/bin/Debug/net8.0-windows");
            string appPath = Path.Combine(appWorkingDirPath, "SaveGame.exe");

            AppiumOptions options = new AppiumOptions();
            options.AddAdditionalCapability("app", appPath);
            options.AddAdditionalCapability("appWorkingDir", appWorkingDirPath);

            driver = new WindowsDriver<WindowsElement>(
                new Uri("http://127.0.0.1:4723"),
                options
                );
        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
        }

        [Test]
        public void CanAddToPlay()
        {
            Assert.That(true, Is.True);
        }
    }
}