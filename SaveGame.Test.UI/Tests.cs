using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace SaveGame.Test.UI
{
    public class Tests : BaseTest
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
            Actions action = new(driver);
            action.ContextClick(firstHighRatedGame).SendKeys(Keys.Down).SendKeys(Keys.Enter).Build().Perform();

            //Navigate to play view
            var playNavButton = driver.FindElementByAccessibilityId("PlayNavButton");
            playNavButton.Click();
            var playGames = WaitForElements("PlayGame");
            
            //Verify game
            Assert.That(playGames, Has.Count.EqualTo(1));
            playGames.First().Click();
            Assert.That(WaitForElement("GameName").Text, Is.EqualTo(gamename));
        }

        [Test]
        public void AddToPlaying()
        {
            var highRatedGames = WaitForElements("HighRatedGame");
            var firstHighRatedGame = highRatedGames.First();
            firstHighRatedGame.Click(); //Get name of game from modal
            string gamename = WaitForElement("GameName").Text;
            driver.Keyboard.PressKey(Keys.Escape); //Press escape key to close modal

            //Add game
            Actions action = new(driver);
            action.ContextClick(firstHighRatedGame).SendKeys(Keys.Down).SendKeys(Keys.Down).SendKeys(Keys.Enter).Build().Perform();

            //Navigate to play view
            var playNavButton = driver.FindElementByAccessibilityId("PlayingNavButton");
            playNavButton.Click();
            var playingGames = WaitForElements("PlayingGame");

            //Verify game
            Assert.That(playingGames, Has.Count.EqualTo(1));
            playingGames.First().Click();
            Assert.That(WaitForElement("GameName").Text, Is.EqualTo(gamename));
        }

        [Test]
        public void AddToPlayed()
        {
            var highRatedGames = WaitForElements("HighRatedGame");
            var firstHighRatedGame = highRatedGames.First();
            firstHighRatedGame.Click(); //Get name of game from modal
            string gamename = WaitForElement("GameName").Text;
            driver.Keyboard.PressKey(Keys.Escape); //Press escape key to close modal

            //Add game
            Actions action = new(driver);
            action.ContextClick(firstHighRatedGame)
                .SendKeys(Keys.Down).SendKeys(Keys.Down).SendKeys(Keys.Down) //Played
                .SendKeys(Keys.Enter).Build().Perform();

            //Navigate to played view
            var playedNavButton = driver.FindElementByAccessibilityId("PlayedNavButton");
            playedNavButton.Click();
            var playedGames = WaitForElements("PlayedGame");

            //Verify game
            Assert.That(playedGames, Has.Count.EqualTo(1));
            playedGames.First().Click();
            Assert.That(WaitForElement("GameName").Text, Is.EqualTo(gamename));
        }

        [Test]
        public void SuggestGame()
        {
            var highRatedGames = WaitForElements("HighRatedGame");
            var firstHighRatedGame = highRatedGames.First();

            //Add game
            Actions action = new(driver);
            action.ContextClick(firstHighRatedGame)
                .SendKeys(Keys.Down) //Play
                .SendKeys(Keys.Enter).Build().Perform();

            //Games are suggested
            var suggestedGame = WaitForElements("SuggestedGame");
            Assert.That(suggestedGame, Is.Not.Empty);
        }

        [Test]
        public void SearchGame()
        {
            //Navigate to play view
            var playNavButton = driver.FindElementByAccessibilityId("PlayedNavButton");
            playNavButton.Click();

            //Search for game
            string searchGameName = "Bloodborne";
            var searchTextBox = driver.FindElementByAccessibilityId("SearchTextBox");
            searchTextBox.Click();
            driver.Keyboard.SendKeys(searchGameName);

            //Verify search results
            var searchedGames = WaitForElements("SearchedGame");
            Assert.That(searchedGames, Is.Not.Empty);
            searchedGames.First().Click();
            Assert.That(WaitForElement("GameName").Text, Is.EqualTo(searchGameName));
        }

        [Test]
        public void PlayToPlayedStatus()
        {
            var highRatedGames = WaitForElements("HighRatedGame");
            var firstHighRatedGame = highRatedGames.First();
            firstHighRatedGame.Click(); //Get name of game from modal
            string gamename = WaitForElement("GameName").Text;
            driver.Keyboard.PressKey(Keys.Escape); //Press escape key to close modal

            //Add game
            Actions addGameAction = new(driver);
            addGameAction.ContextClick(firstHighRatedGame)
                .SendKeys(Keys.Down) //Play
                .SendKeys(Keys.Enter)
                .Build().Perform();

            //Navigate to play view
            var playNavButton = driver.FindElementByAccessibilityId("PlayNavButton");
            playNavButton.Click();

            //Change status to playing
            var playGames = WaitForElements("PlayGame");
            Actions changeStatusAction = new(driver);
            changeStatusAction.ContextClick(playGames.First())
                .SendKeys(Keys.Down).SendKeys(Keys.Down).SendKeys(Keys.Down) //Played
                .SendKeys(Keys.Enter).Build().Perform();
            playGames = driver.FindElementsByAccessibilityId("PlayGame");
            Assert.That(playGames, Is.Empty);

            //Navigate to play view
            var playedNavButton = driver.FindElementByAccessibilityId("PlayedNavButton");
            playedNavButton.Click();

            //Verify game
            var playedGames = WaitForElements("PlayedGame");
            Assert.That(playedGames, Has.Count.EqualTo(1));
            playedGames.First().Click();
            Assert.That(WaitForElement("GameName").Text, Is.EqualTo(gamename));
        }

        [Test]
        public void RemoveGame()
        {
            var highRatedGames = WaitForElements("HighRatedGame");
            var firstHighRatedGame = highRatedGames.First();

            //Add game
            Actions addGameAction = new(driver);
            addGameAction.ContextClick(firstHighRatedGame).SendKeys(Keys.Down).SendKeys(Keys.Enter).Build().Perform();

            //Navigate to play view
            var playNavButton = driver.FindElementByAccessibilityId("PlayNavButton");
            playNavButton.Click();

            //Remove game
            var playGames = WaitForElements("PlayGame");
            Actions removeGameAction = new(driver);
            removeGameAction.ContextClick(playGames.First())
                .SendKeys(Keys.Down).SendKeys(Keys.Down).SendKeys(Keys.Down).SendKeys(Keys.Down) //Remove
                .SendKeys(Keys.Enter)
                .Build().Perform();
            playGames = driver.FindElementsByAccessibilityId("PlayGame");
            Assert.That(playGames, Is.Empty);
        }
    }
}