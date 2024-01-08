using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IGDB;
using IGDB.Models;
using SaveGame.Services;
using SaveGame.Stores;
using SaveGame.Views;

namespace SaveGame.ViewModels
{
    partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        string searchQuery = "";

        [ObservableProperty]
        bool isSearching = false;

        [ObservableProperty]
        IEnumerable<Game> searchResults;

        [ObservableProperty]
        IEnumerable<Game> randomGames;
        
        [ObservableProperty]
        object? currentView = null;

        [ObservableProperty]
        bool isSearchBarVisible = false;

        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly GameStore _gameStore;

        public Game? GameDetail => _modalNavigationStore.Detail;
        public bool IsGameDetailModalOpen => _modalNavigationStore.IsOpen;

        HomeView HomeView;
        PlayView PlayView;
        PlayingView PlayingView;
        PlayedView PlayedView;

        IGDBService _igdbService;

        [RelayCommand]
        void Remove(Game game) => _gameStore.Remove(game);

        [RelayCommand]
        void AddToPlay(Game game) => _gameStore.AddToPlay(game);

        [RelayCommand]
        void AddToPlaying(Game game) => _gameStore.AddToPlaying(game);

        [RelayCommand]
        void AddToPlayed(Game game) => _gameStore.AddToPlayed(game);

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

            GotoHomeView();
            _gameStore = gameStore;
            _igdbService = igdbService;
        }

        private void ModalNavigationStore_GameDetailChanged()
        {
            OnPropertyChanged(nameof(GameDetail));
            OnPropertyChanged(nameof(IsGameDetailModalOpen));
        }

        private static Timer timer;
        partial void OnSearchQueryChanged(string value)
        {
            if (value == "")
            {
                SearchResults = Enumerable.Empty<Game>();
                return;
            }
            
            IsSearching = true;

            timer?.Change(Timeout.Infinite, Timeout.Infinite);
            timer = new Timer(async state =>
            {
                SearchResults = await _igdbService.SearchGame(value);
                IsSearching = false;
            }, null, 500, Timeout.Infinite);

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
