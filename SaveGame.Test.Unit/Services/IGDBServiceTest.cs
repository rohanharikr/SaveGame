using IGDB.Models;
using SaveGame.Services;
using SaveGame.Stores;

namespace SaveGame.Test.Unit.Services
{
    public class IGDBServiceTest
    {
        readonly private IGDBService _igdbService;

        public IGDBServiceTest()
        {
            GameStore gameStore = new(); //Dummy store
            _igdbService = new IGDBService(gameStore);
        }

        [Test]
        public async Task SearchReturnsRelevantResults()
        {
            string searchQuery = "Dark";
            IEnumerable<Game> result = await _igdbService.SearchGame(searchQuery);
            Assert.That(result.Count(), Is.EqualTo(5));
            foreach (Game game in result)
                Assert.That(game.Name, Does.Contain(searchQuery));
        }

        [Test]
        public async Task GetsRecentReleases()
        {
            IEnumerable<Game> result = await _igdbService.GetRecentReleases();
            Assert.That(result.Count(), Is.EqualTo(5));
            int unixTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            int yearInUnixTime = 31536000;
            int pastYearUnixTime = unixTime - yearInUnixTime;

            foreach (Game game in result)
            {
                if (game.FirstReleaseDate == null)
                    continue;

                Assert.That(game.FirstReleaseDate.Value.ToUnixTimeSeconds(), Is.GreaterThan(pastYearUnixTime));
                Assert.That(game.FirstReleaseDate.Value.ToUnixTimeSeconds(), Is.LessThan(unixTime));
            }
        }

        [Test]
        public async Task GetsUpcomingReleases()
        {
            IEnumerable<Game> result = await _igdbService.GetUpcomingReleases();
            Assert.That(result.Count(), Is.EqualTo(5));
            int unixTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            foreach (Game game in result)
            {
                if (game.FirstReleaseDate == null)
                    continue;

                Assert.That(game.FirstReleaseDate.Value.ToUnixTimeSeconds(), Is.GreaterThan(unixTime));
            }
        }
    }
}
