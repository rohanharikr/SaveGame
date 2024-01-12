using SaveGame.Services;
using SaveGame.Stores;
using SaveGame.ViewModels;

namespace SaveGame.Test.Unit.ViewModels
{
    public class HomeViewModelTest : BaseTest
    {
        HomeViewModel _homeViewModel;
        IGDBService _igdbService;
        ModalNavigationStore _modalNavigationStore;
        GameStore _gameStore;

        public HomeViewModelTest()
        {
            //Dummy stores/services
            _modalNavigationStore = new ModalNavigationStore();
            _gameStore = new GameStore();
            _igdbService = new IGDBService(_gameStore);
        }


        [SetUp]
        public void Setup()
        {
            //Start every test on a clean state
            _homeViewModel = new HomeViewModel(_modalNavigationStore, _gameStore, _igdbService);
        }

        [Test]
        [Ignore("TBD")]
        public void GetsGames()
        {
            _homeViewModel.GetGames(_igdbService);
            
            //TBD Wait for results to come back

            Assert.That(_homeViewModel.SuggestedGames, Is.Empty);
            Assert.That(_homeViewModel.HighRatedGames, Has.Count.EqualTo(5));
            Assert.That(_homeViewModel.RecentReleases, Has.Count.EqualTo(5));
            Assert.That(_homeViewModel.UpcomingReleases, Has.Count.EqualTo(5));
        }
    }
}
