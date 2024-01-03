﻿using System;
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
        public Game? GameDetail => _modalNavigationStore.Detail;
        public bool IsGameDetailModalOpen => _modalNavigationStore.IsOpen;

        public MainViewModel(ModalNavigationStore modalNavigationStore)
        {
            igdb = new IGDBClient(
                Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
                Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")
            );

            _modalNavigationStore = modalNavigationStore;
            _modalNavigationStore.DetailChanged += ModalNavigationStore_GameDetailChanged;

            GotoPlayView();
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
        void GotoPlayView()
        {
            CurrentView = new PlayView();
        }

        [RelayCommand]
        void GotoPlayingView()
        {
            CurrentView = new PlayingView();
        }

        [RelayCommand]
        void GotoPlayedView()
        {
            CurrentView = new PlayedView();
        }
    }
}
