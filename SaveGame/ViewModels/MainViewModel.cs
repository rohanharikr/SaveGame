using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaveGame.Models;
using SaveGame.Services;
using SaveGame.Stores;
using SaveGame.Views;
using System.Collections.ObjectModel;

namespace SaveGame.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        string searchQuery = "";

        [ObservableProperty]
        bool isSearching = false;

        [ObservableProperty]
        ObservableCollection<Game>? searchResults;

        [ObservableProperty]
        object? currentView = null;

        [ObservableProperty]
        bool isSearchBarVisible = false;

        Game? GameDetail => _modalNavigationStore.Detail;
        bool IsGameDetailModalOpen => _modalNavigationStore.IsOpen;

        #region Views
        readonly HomeView HomeView;
        readonly PlayView PlayView;
        readonly PlayingView PlayingView;
        readonly PlayedView PlayedView;
        #endregion

        #region Services
        readonly IGDBService _igdbService;
        #endregion

        #region Stores
        readonly ModalNavigationStore _modalNavigationStore;
        readonly GameStore _gameStore;
        #endregion

        [RelayCommand]
        void ShowGameDetailModal(Game game) => _modalNavigationStore.Show(game);

        [RelayCommand]
        void CloseGameDetailModal() => _modalNavigationStore.Close();

        public MainViewModel(ModalNavigationStore modalNavigationStore, GameStore gameStore, IGDBService igdbService)
        {
            HomeView = new HomeView(modalNavigationStore, gameStore, igdbService);
            PlayView = new PlayView(modalNavigationStore, gameStore);
            PlayingView = new PlayingView(modalNavigationStore, gameStore);
            PlayedView = new PlayedView(modalNavigationStore, gameStore);

            _modalNavigationStore = modalNavigationStore;
            _modalNavigationStore.DetailChanged += ModalNavigationStore_GameDetailChanged;
            _gameStore = gameStore;
            _igdbService = igdbService;

            _gameStore.Retrieve();

            _gameStore.GamesChanged += GameStore_GamesChanged;

            GotoHomeView();
        }

        [RelayCommand]
        void AddToPlay(Game game)
        {
            _gameStore.AddToPlay(game);
            ResetSearch();
        }

        [RelayCommand]
        void AddToPlaying(Game game)
        {
            _gameStore.AddToPlaying(game);
            ResetSearch();
        }

        [RelayCommand]
        void AddToPlayed(Game game)
        {
            _gameStore.AddToPlayed(game);
            ResetSearch();
        }

        [RelayCommand]
        void Remove(Game game)
        {
            _gameStore.Remove(game);
            ResetSearch();
        }

        public void ResetSearch()
        {
            SearchQuery = "";
            SearchResults = [];
        }

        private void GameStore_GamesChanged()
        {
            if (SearchResults == null)
                return;

            SearchResults = _gameStore.UpdateGameState(SearchResults);
        }

        private void ModalNavigationStore_GameDetailChanged()
        {
            OnPropertyChanged(nameof(GameDetail));
            OnPropertyChanged(nameof(IsGameDetailModalOpen));
        }

        partial void OnSearchQueryChanged(string value)
        {
            if (value == "")
            {
                SearchResults = new ObservableCollection<Game>();
                return;
            }

            IsSearching = true;

            //Stop spamming API for every keypress - wait for 500ms after every key press before calling API 
            Utility.Debounce(async () =>
            {
                SearchResults = new ObservableCollection<Game>(await _igdbService.SearchGame(value));
                IsSearching = false;
            }, 500);
        }

        [RelayCommand]
        void GotoHomeView()
        {
            CurrentView = HomeView;
            IsSearchBarVisible = false;
            ResetSearch();
        }

        [RelayCommand]
        void GotoPlayView()
        {
            CurrentView = PlayView;
            IsSearchBarVisible = true;
        }

        [RelayCommand]
        void GotoPlayingView()
        {
            CurrentView = PlayingView;
            IsSearchBarVisible = true;
        }

        [RelayCommand]
        void GotoPlayedView()
        {
            CurrentView = PlayedView;
            IsSearchBarVisible = true;
        }
    }
}
