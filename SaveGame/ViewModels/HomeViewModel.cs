using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IGDB;
using IGDB.Models;
using Newtonsoft.Json.Linq;
using SaveGame.Services;
using SaveGame.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveGame.ViewModels
{
    partial class HomeViewModel : ObservableObject
    {
        IGDBClient igdb;

        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly GameStore _gameStore;

        public Game? GameDetail => _modalNavigationStore.Detail;
        public bool IsGameDetailModalOpen => _modalNavigationStore.IsOpen;

        public ObservableCollection<Game> PlayGames => _gameStore.PlayGames;

        [ObservableProperty]
        IEnumerable<Game> upcomingReleases;

        [ObservableProperty]
        bool isFetchingUpcomingReleases = true;

        public HomeViewModel(ModalNavigationStore modalNavigationStore, GameStore gameStore)
        {
            igdb = new IGDBClient(
                Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
                Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")
            );

            GetUpcomingReleases();

            _modalNavigationStore = modalNavigationStore;
            _gameStore = gameStore;

            _modalNavigationStore.DetailChanged += ModalNavigationStore_GameDetailChanged;
            _gameStore.GamesChanged += GameStore_GamesChanged;
        }

        async void GetUpcomingReleases()
        {
            IsFetchingUpcomingReleases = true;

            Int32 unixTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query:
                    $"fields name, involved_companies.developer, involved_companies.company.name," +
                        $"screenshots.image_id, screenshots.url, cover.url, cover.image_id," +
                        $"summary, genres.name, genres.slug, release_dates.y;" +

                    $"where screenshots >= 3 & genres > 0 & summary != null & name ~ *\"\"* & version_parent = null & parent_game = null &" +
                        $"(follows > 25 | hypes > 25) & first_release_date > {unixTime} & involved_companies.developer = true;" +

                    $"sort first_release_date asc;" +

                    $"limit 5;");

            UpcomingReleases = games;

            IsFetchingUpcomingReleases = false;
        }

        private void ModalNavigationStore_GameDetailChanged()
        {
            OnPropertyChanged(nameof(GameDetail));
            OnPropertyChanged(nameof(IsGameDetailModalOpen));
        }

        private void GameStore_GamesChanged()
        {
            OnPropertyChanged(nameof(PlayGames));
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

        [RelayCommand]
        void AddToPlaying(Game game)
        {
            _gameStore.PlayGames.Remove(game);
            _gameStore.PlayingGames.Add(game);
        }

        [RelayCommand]
        void AddToPlayed(Game game)
        {
            _gameStore.PlayGames.Remove(game);
            _gameStore.PlayedGames.Add(game);
        }

        [RelayCommand]
        void Remove(Game game)
        {
            _gameStore.PlayGames.Remove(game);
        }
    }
}
