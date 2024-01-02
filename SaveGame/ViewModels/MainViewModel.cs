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
        bool showGameDetailModal = false;

        [ObservableProperty]
        Game? gameDetail = null;

        [ObservableProperty]
        object? currentView = null;

        private readonly ModalNavigationStore _modalNavigationStore;
        public bool CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;
        public bool IsModalOpen => _modalNavigationStore.IsOpen;

        IGDBClient igdb;

        public MainViewModel(ModalNavigationStore modalNavigationStore)
        {
            igdb = new IGDBClient(
                Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
                Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")
            );

            _modalNavigationStore = modalNavigationStore;
            _modalNavigationStore.CurrentViewModelChanged += ModalNavigationStore_CurrentViewModelChanged;

            GotoPlayView();
        }

        private void ModalNavigationStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentModalViewModel));
            OnPropertyChanged(nameof(IsModalOpen));
        }

        async partial void OnSearchQueryChanged(string value)
        {
            if (value == "")
            {
                SearchResults = Enumerable.Empty<Game>();
                return;
            }

            IsSearching = true;
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: $"fields *, screenshots.*, genres.*, videos.*, release_dates.*, involved_companies.company.*, cover.*; search \"{value}\"; limit 4;");
            SearchResults = games;
            IsSearching = false;
        }

        [RelayCommand]
        void OpenGameDetailModal(Game? game)
        {
            GameDetail = game;
            ShowGameDetailModal = true;
        }

        [RelayCommand]
        void CloseGameDetailModal()
        {
            ShowGameDetailModal = false;
            GameDetail = null;
            return;
        }

        [RelayCommand]
        void GotoPlayView()
        {
            CurrentView = new PlayView();
        }

        [RelayCommand]
        void GotoPlayingView()
        {
            CurrentView = new PlayingView();
            _modalNavigationStore.CurrentViewModel = true;
        }

        [RelayCommand]
        void GotoPlayedView()
        {
            CurrentView = new PlayedView();
            _modalNavigationStore.Close();
        }
    }
}
