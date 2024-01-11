using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace SaveGame.Test.UI
{
    public class Tests : Setup
    {
        [Test]
        public void CanAddToPlay()
        {
            Assert.That(true, Is.True);
        }
    }
}