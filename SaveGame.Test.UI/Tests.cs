using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace SaveGame.Test.UI
{
    public class Tests : Setup
    {
        [Test]
        public void AddToPlay()
        {
            var highRatedGames = WaitForElements("HighRatedGame");
            var firstHighRatedGame = highRatedGames.First();
            firstHighRatedGame.Click(); //Get name of game from modal
            string gamename = WaitForElement("GameName").Text;
            driver.Keyboard.PressKey(Keys.Escape); //Press escape key to close modal
            
            //Add game
            Actions action = new Actions(driver);
            action.ContextClick(firstHighRatedGame).SendKeys(Keys.Down).SendKeys(Keys.Enter).Build().Perform();

            //Navigate to play view
            var playNavButton = driver.FindElementByAccessibilityId("PlayNavButton");
            playNavButton.Click();
            var playGames = WaitForElements("PlayGame");
            
            //Verify game
            Assert.That(playGames.Count, Is.EqualTo(1));
            playGames.First().Click();
            Assert.That(WaitForElement("GameName").Text, Is.EqualTo(gamename));
        }
    }
}