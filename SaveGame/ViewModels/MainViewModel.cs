using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IGDB;
using IGDB.Models;
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

        IGDBClient igdb;

        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly GameStore _gameStore;

        public Game? GameDetail => _modalNavigationStore.Detail;
        public bool IsGameDetailModalOpen => _modalNavigationStore.IsOpen;

        PlayView PlayView;
        PlayingView PlayingView;
        PlayedView PlayedView;

        public MainViewModel(ModalNavigationStore modalNavigationStore, GameStore gameStore)
        {
            igdb = new IGDBClient(
                Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
                Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")
            );

            PlayView = new PlayView(modalNavigationStore, gameStore);
            PlayingView = new PlayingView(modalNavigationStore, gameStore);
            PlayedView = new PlayedView(modalNavigationStore, gameStore);

            _modalNavigationStore = modalNavigationStore;
            _modalNavigationStore.DetailChanged += ModalNavigationStore_GameDetailChanged;

            GotoPlayView();
            _gameStore = gameStore;
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
                var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: $"fields *, screenshots.*, genres.*, videos.*, release_dates.*, involved_companies.company.*, cover.*; search \"{value}\"; limit 4;");
                SearchResults = games;
                IsSearching = false;
            }, null, 250, Timeout.Infinite);

        }

        [RelayCommand]
        void GotoPlayView()
        {
            CurrentView = PlayView;
        }

        [RelayCommand]
        void GotoPlayingView()
        {
            CurrentView = PlayingView;
        }

        [RelayCommand]
        void GotoPlayedView()
        {
            CurrentView = PlayedView;
        }

        [RelayCommand]
        void AddToPlay(Game game)
        {
            _gameStore.PlayGames.Add(game);
        }
    }
}
