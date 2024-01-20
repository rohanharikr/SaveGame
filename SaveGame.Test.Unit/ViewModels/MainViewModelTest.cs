using SaveGame.Services;
using SaveGame.Stores;
using SaveGame.ViewModels;

namespace SaveGame.Test.Unit.ViewModels
{
    public class MainViewModelTest : BaseTest
    {
        MainViewModel _mainViewModel;

        readonly IGDBService _igdbService;
        readonly ModalNavigationStore _modalNavigationStore;
        readonly GameStore _gameStore;

        public MainViewModelTest()
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
            _mainViewModel = new MainViewModel(_modalNavigationStore, _gameStore, _igdbService);
        }

        [Test]
        [Ignore("TBD")]
        public void ResetsSearch()
        {
            _mainViewModel.SearchQuery = "Bloodborne";

            //TBD Wait for results to come back

            _mainViewModel.ResetSearch();
            Assert.IsEmpty(_mainViewModel.SearchQuery);
            Assert.That(_mainViewModel.SearchResults, Is.Zero);
        }

        [Test]
        [Ignore("TBD")]
        public void SearchGame()
        {
            _mainViewModel.SearchQuery = "Bloodborne";
            Assert.IsTrue(_mainViewModel.IsSearching);

            //TBD Wait for results to come back

            Assert.That(_mainViewModel.SearchResults, Is.Not.Empty);
        }
    }
}
