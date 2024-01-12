using NUnit.Framework;

namespace SaveGame.Test.UI
{
    public class Tests : Setup
    {
        [Test]
        public void CanAddToPlay()
        {
            var highRatedGames = WaitForElements("HighRatedGame");
            highRatedGames.First().Click();
            string gamename = WaitForElement("GameName").Text;
            Console.WriteLine(gamename);
        }
    }
}