﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IGDB;
using IGDB.Models;
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

        long GetDateTimeInMs()
        {
            DateTime currentDate = DateTime.Now;
            long ticks = currentDate.Ticks;
            long milliseconds = ticks / TimeSpan.TicksPerMillisecond;
            return milliseconds;
        }

        async void GetUpcomingReleases()
        {
            var dateTimeInMs = GetDateTimeInMs();
            IEnumerable<Game> games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: $"fields *, screenshots.*, genres.*, videos.*, release_dates.*, involved_companies.company.*, cover.*; where cover.url != null; limit 5;");
            UpcomingReleases = games;
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