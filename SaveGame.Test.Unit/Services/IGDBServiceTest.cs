using SaveGame.Services;

namespace SaveGame.Test.Unit.Services
{
    public class IGDBServiceTest
    {
        private IGDBService _igdbService;

        public IGDBServiceTest()
        {
            GameStore
            _igdbService = new IGDBService(this);
        }

        [Test]
        public void SearchReturnsRelevantResults()
        {

        }
    }
}
