using IGDB.Models;
using SaveGame.Services;
using SaveGame.Stores;

namespace SaveGame.Test.Unit.Services
{
    public class IGDBServiceTest
    {
        private IGDBService _igdbService;

        public IGDBServiceTest()
        {
            GameStore gameStore = new GameStore();
            _igdbService = new IGDBService(gameStore);
        }

        [Test]
        public async Task SearchReturnsRelevantResults()
        {
            string searchQuery = "Dark";
            IEnumerable<Game> results = await _igdbService.SearchGame(searchQuery);
            Assert.That(results.Count(), Is.EqualTo(5));
            foreach (Game game in results)
                Assert.That(game.Name, Does.Contain(searchQuery));
        }
    }
}
