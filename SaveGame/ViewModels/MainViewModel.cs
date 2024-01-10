using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaveGame.Models;
using SaveGame.Services;
using SaveGame.Stores;
using SaveGame.Views;
using System.Collections.ObjectModel;

namespace SaveGame.ViewModels
{
    partial class MainViewModel : ObservableObject
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

        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly GameStore _gameStore;

        public Game? GameDetail => _modalNavigationStore.Detail;
        public bool IsGameDetailModalOpen => _modalNavigationStore.IsOpen;

        readonly HomeView HomeView;
        readonly PlayView PlayView;
        readonly PlayingView PlayingView;
        readonly PlayedView PlayedView;

        readonly IGDBService _igdbService;

        [RelayCommand]
        void AddToPlay(Game game) => _gameStore.AddToPlay(game);

        [RelayCommand]
        void AddToPlaying(Game game) => _gameStore.AddToPlaying(game);

        [RelayCommand]
        void AddToPlayed(Game game) => _gameStore.AddToPlayed(game);
        
        [RelayCommand]
        void Remove(Game game) => _gameStore.Remove(game);

        [ObservableProperty]
        bool isRetrievingGames = true;

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

        private Timer _timer;
        partial void OnSearchQueryChanged(string value)
        {
            if (value == "")
            {
                SearchResults = new ObservableCollection<Game>();
                return;
            }

            IsSearching = true;

            Debounce(async () =>
            {
                SearchResults = new ObservableCollection<Game>(await _igdbService.SearchGame(value));
                IsSearching = false;
            }, 500);
        }

        private void Debounce(Action action, int millisecondsDelay)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            _timer = new Timer(state =>
            {
                action.Invoke();
            }, null, millisecondsDelay, Timeout.Infinite);
        }

        [RelayCommand]
        void GotoHomeView()
        {
            CurrentView = HomeView;
            IsSearchBarVisible = false;
            SearchQuery = "";
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
