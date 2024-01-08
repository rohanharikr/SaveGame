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
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveGame.ViewModels
{
    partial class HomeViewModel : ObservableObject
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly GameStore _gameStore;

        public Game? GameDetail => _modalNavigationStore.Detail;
        public bool IsGameDetailModalOpen => _modalNavigationStore.IsOpen;

        [ObservableProperty]
        bool isFetchingUpcomingReleases = true;

        [ObservableProperty]
        IEnumerable<Game> upcomingReleases;

        [ObservableProperty]
        bool isFetchingRecentReleases = true;
        
        [ObservableProperty]
        IEnumerable<Game> recentReleases;

        [ObservableProperty]
        bool isFetchingHighRatedGames = true;

        [ObservableProperty]
        IEnumerable<Game> highRatedGames;

        [ObservableProperty]
        string greeting = "Good ";

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

        public HomeViewModel(ModalNavigationStore modalNavigationStore, GameStore gameStore, IGDBService igdbService)
        {
            GetGames(igdbService);

            greeting += TimeOfDay();

            _modalNavigationStore = modalNavigationStore;
            _gameStore = gameStore;

            _modalNavigationStore.DetailChanged += ModalNavigationStore_GameDetailChanged;
        }

        async void GetGames(IGDBService igdbService)
        {
            Task<IEnumerable<Game>> recentReleasesTask = igdbService.GetRecentReleases();
            Task<IEnumerable<Game>> upcomingReleasesTask = igdbService.GetUpcomingReleases();
            Task<IEnumerable<Game>> highRatedGamesTask = igdbService.GetHighRatedGames();

            await Task.WhenAll(recentReleasesTask, upcomingReleasesTask, highRatedGamesTask);

            RecentReleases = recentReleasesTask.Result;
            UpcomingReleases = upcomingReleasesTask.Result;
            HighRatedGames = highRatedGamesTask.Result;
        }

        string TimeOfDay()
        {
            int hour = DateTime.Now.Hour;
            if (hour >= 18)
                return "Evening";
            else if (hour >= 12)
                return "Afternoon";
            else
                return "Morning";
        }

        private void ModalNavigationStore_GameDetailChanged()
        {
            OnPropertyChanged(nameof(GameDetail));
            OnPropertyChanged(nameof(IsGameDetailModalOpen));
        }
    }
}
