using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IGDB;
using IGDB.Models;
using SaveGame.Services;
using SaveGame.Stores;
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

        IGDBClient igdb;

        private readonly ModalNavigationStore _modalNavigationStore;
        public Game? GameDetail => _modalNavigationStore.Detail;
        public bool IsGameDetailModalOpen => _modalNavigationStore.IsOpen;

        public PlayViewModel(ModalNavigationStore modalNavigationStore)
        {
            igdb = new IGDBClient(
                Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
                Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")
            );

            _modalNavigationStore = modalNavigationStore;
            _modalNavigationStore.DetailChanged += ModalNavigationStore_GameDetailChanged;

            GetRandomGames();
        }

        private void ModalNavigationStore_GameDetailChanged()
        {
            OnPropertyChanged(nameof(GameDetail));
            OnPropertyChanged(nameof(IsGameDetailModalOpen));
        }

        async void GetRandomGames()
        {
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields *, screenshots.*, genres.*, videos.*, release_dates.*, involved_companies.company.*, cover.*; sort rating desc; limit: 16;");
            RandomGames = games;
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
    }
}
