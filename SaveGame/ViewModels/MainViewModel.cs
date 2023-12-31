using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IGDB;
using IGDB.Models;

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

        IGDBClient igdb;

        public MainViewModel()
        {
            igdb = new IGDBClient(
                Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
                Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")
            );

            GetRandomGames();
        }

        async partial void OnSearchQueryChanged(string value)
        {
            if(value == "")
            {
                SearchResults = Enumerable.Empty<Game>();
                return;
            }

            IsSearching = true;
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: $"fields *, screenshots.*, genres.*, videos.*, release_dates.*, involved_companies.company.*, cover.*; search \"{value}\"; limit 8;");
            SearchResults = games;
            IsSearching = false;
        }

        async void GetRandomGames()
        {
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields *, screenshots.*, genres.*, videos.*, release_dates.*, involved_companies.company.*, cover.*; sort rating desc; limit: 16;");
            RandomGames = games;
        }

        [RelayCommand]
        void CloseApp()
        {
            System.Windows.Application.Current.Shutdown();
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
    }
}
