using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IGDB;
using IGDB.Models;
using SaveGame.Services;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveGame.ViewModels
{
    partial class PlayViewModel : ObservableObject
    {
        [ObservableProperty]
        IEnumerable<Game> randomGames;

        [ObservableProperty]
        bool showGameDetailModal = false;

        [ObservableProperty]
        Game? gameDetail = null;

        IGDBClient igdb;

        public PlayViewModel()
        {
            igdb = new IGDBClient(
                Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
                Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")
            );

            GetRandomGames();

            using var context = new SQLiteService();
            Console.WriteLine(context.Play);
        }

        async void GetRandomGames()
        {
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields *, screenshots.*, genres.*, videos.*, release_dates.*, involved_companies.company.*, cover.*; sort rating desc; limit: 16;");
            RandomGames = games;
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
