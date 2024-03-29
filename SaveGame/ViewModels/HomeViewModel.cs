﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaveGame.Models;
using SaveGame.Services;
using SaveGame.Stores;
using System.Collections.ObjectModel;

namespace SaveGame.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly GameStore _gameStore;

        [ObservableProperty]
        ObservableCollection<Game> suggestedGames;

        [ObservableProperty]
        ObservableCollection<Game> upcomingReleases;

        [ObservableProperty]
        ObservableCollection<Game> recentReleases;

        [ObservableProperty]
        ObservableCollection<Game> highRatedGames;

        [ObservableProperty]
        string greeting;

        [RelayCommand]
        void AddToPlay(Game game) => _gameStore.AddToPlay(game);

        [RelayCommand]
        void AddToPlaying(Game game) => _gameStore.AddToPlaying(game);

        [RelayCommand]
        void AddToPlayed(Game game) => _gameStore.AddToPlayed(game);
        
        [RelayCommand]
        void Remove(Game game) => _gameStore.Remove(game);

        [RelayCommand]
        void ShowGameDetailModal(Game game) => _modalNavigationStore.Show(game);

        public HomeViewModel(ModalNavigationStore modalNavigationStore, GameStore gameStore, IGDBService igdbService)
        {
            GetGames(igdbService);

            greeting += $"Good {Utility.TimeOfDay()}";

            _modalNavigationStore = modalNavigationStore;
            _gameStore = gameStore;

            _gameStore.GamesChanged += SuggestGames;
            _gameStore.GamesChanged += UpdateGameStates;

            SuggestedGames = [];
            HighRatedGames = [];
            RecentReleases = [];
            UpcomingReleases = [];
        }

        void UpdateGameStates()
        {
            //SuggestGames do not need to be updated as tracked games do not show up there
            HighRatedGames = _gameStore.UpdateGameState(HighRatedGames);
            RecentReleases = _gameStore.UpdateGameState(RecentReleases);
            UpcomingReleases = _gameStore.UpdateGameState(UpcomingReleases);
        }

        void SuggestGames()
        {
            if (_gameStore == null)
                return;

            List<Game> playSimilarGames = _gameStore.PlayGames
                .SelectMany(game => game.SimilarGames?.Values ?? Enumerable.Empty<Game>())
                .ToList();

            List<Game> playingSimilarGames = _gameStore.PlayingGames
                .SelectMany(game => game.SimilarGames?.Values ?? Enumerable.Empty<Game>())
                .ToList();

            List<Game> playedSimilarGames = _gameStore.PlayedGames
                .SelectMany(game => game.SimilarGames?.Values ?? Enumerable.Empty<Game>())
                .ToList();

            List<Game> allSuggestedGames =
            [
                ..playSimilarGames,
                ..playingSimilarGames,
                ..playedSimilarGames,
            ];

            IEnumerable<Game> suggestedGamesProritsed = allSuggestedGames
               .GroupBy(game => game.Id)
               //games w/ highest inetersection occurrences gets highest priority in list
               .OrderByDescending(group => group.Count())
               .SelectMany(group => group)
               //remove duplicate suggestions
               .DistinctBy(g => g.Id)
               //do not suggest games that are already being tracked
               .Where(game => game.PlayState == PlayStates.None)
               .Take(5)
               .ToList();

            SuggestedGames = new ObservableCollection<Game>(suggestedGamesProritsed);
        }

        public async void GetGames(IGDBService igdbService)
        {
            SuggestGames();

            Task<IEnumerable<Game>> recentReleasesTask = igdbService.GetRecentReleases();
            Task<IEnumerable<Game>> upcomingReleasesTask = igdbService.GetUpcomingReleases();
            Task<IEnumerable<Game>> highRatedGamesTask = igdbService.GetHighRatedGames();

            await Task.WhenAll(recentReleasesTask, upcomingReleasesTask, highRatedGamesTask);
            
            RecentReleases = new ObservableCollection<Game>(recentReleasesTask.Result);
            UpcomingReleases = new ObservableCollection<Game>(upcomingReleasesTask.Result);
            HighRatedGames = new ObservableCollection<Game>(highRatedGamesTask.Result);
        }
    }
}
