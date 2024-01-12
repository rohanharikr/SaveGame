using NUnit.Framework;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;

namespace SaveGame.Test.UI
{
    public class Setup
    {
        readonly public WindowsDriver<WindowsElement> driver;
        readonly string appWorkingDirPath;
        readonly string appPath;

        const int MAX_RETRIES = 15;
        readonly TimeSpan RETRY_INTERVAL = TimeSpan.FromSeconds(1);


        readonly AppiumOptions options;

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
            //Remove DB
            File.Delete(Path.Combine(appWorkingDirPath, "Data.db"));

            driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
        }

        public WindowsElement WaitForElement(string Id)
        {
            WindowsElement element;
            for (int i = 0; i < MAX_RETRIES; i++)
            {
                Thread.Sleep(RETRY_INTERVAL);
                try
                {
                    element = driver.FindElementByAccessibilityId(Id);
                    return element;
                }
                catch (Exception)
                {
                    //retrying...
                }
            }
            throw new Exception("Element not found");
        }

        public IReadOnlyCollection<WindowsElement> WaitForElements(string Id)
        {
            IReadOnlyCollection<WindowsElement> elements;
            for (int i = 0; i < MAX_RETRIES; i++)
            {
                Thread.Sleep(RETRY_INTERVAL);
                elements = driver.FindElementsByAccessibilityId(Id);
                if (elements.Count > 0)
                    return elements;
            }
            throw new Exception("Elements not found");
        }
    }
}
