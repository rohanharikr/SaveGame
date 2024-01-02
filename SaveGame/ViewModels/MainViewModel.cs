﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IGDB;
using IGDB.Models;
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

        IGDBClient igdb;

        public MainViewModel()
        {
            igdb = new IGDBClient(
                Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
                Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")
            );

            GotoPlayView();
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
        }

        [RelayCommand]
        void GotoPlayedView()
        {
            CurrentView = new PlayedView();
        }
    }
}
