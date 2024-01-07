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

        [RelayCommand]
        void ShowGameDetailModal(Game game)
        {
            _modalNavigationStore.Detail = game;
        }

        [RelayCommand]
        void CloseGameDetailModal()
        {
            _modalNavigationStore.Detail = null;
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

        [RelayCommand]
        void Remove(Game game)
        {
            if (_gameStore.PlayGames.Contains(game))
                _gameStore.PlayGames.Remove(game);
            else if (_gameStore.PlayingGames.Contains(game))
                _gameStore.PlayingGames.Remove(game);
            else if (_gameStore.PlayedGames.Contains(game))
                _gameStore.PlayedGames.Remove(game);
        }

        [RelayCommand]
        void AddToPlay(Game game)
        {
            Remove(game);
            _gameStore.PlayGames.Add(game);
        }

        [RelayCommand]
        void AddToPlaying(Game game)
        {
            Remove(game);
            _gameStore.PlayingGames.Add(game);
        }

        [RelayCommand]
        void AddToPlayed(Game game)
        {
            Remove(game);
            _gameStore.PlayedGames.Add(game);
        }
    }
}
