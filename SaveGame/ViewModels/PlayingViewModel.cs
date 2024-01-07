using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IGDB;
using IGDB.Models;
using SaveGame.Services;
using SaveGame.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveGame.ViewModels
{
    partial class PlayingViewModel : ObservableObject
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly GameStore _gameStore;

        public Game? GameDetail => _modalNavigationStore.Detail;
        public bool IsGameDetailModalOpen => _modalNavigationStore.IsOpen;

        public ObservableCollection<Game> PlayingGames => _gameStore.PlayingGames;

        public PlayingViewModel(ModalNavigationStore modalNavigationStore, GameStore gameStore)
        {
            _modalNavigationStore = modalNavigationStore;
            _gameStore = gameStore;

            _modalNavigationStore.DetailChanged += ModalNavigationStore_GameDetailChanged;
            _gameStore.GamesChanged += GameStore_GamesChanged;
        }

        private void ModalNavigationStore_GameDetailChanged()
        {
            OnPropertyChanged(nameof(GameDetail));
            OnPropertyChanged(nameof(IsGameDetailModalOpen));
        }

        private void GameStore_GamesChanged()
        {
            OnPropertyChanged(nameof(PlayingGames));
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
        void Remove(Game game)
        {
            if (_gameStore.PlayGames.Contains(game))
                _gameStore.PlayGames.Remove(game);
            else if (_gameStore.PlayingGames.Contains(game))
                _gameStore.PlayingGames.Remove(game);
            else if (_gameStore.PlayedGames.Contains(game))
                _gameStore.PlayedGames.Remove(game);
        }

        [RelayCommand]
        void AddToPlay(Game game)
        {
            Remove(game);
            _gameStore.PlayGames.Add(game);
        }

        [RelayCommand]
        void AddToPlaying(Game game)
        {
            Remove(game);
            _gameStore.PlayingGames.Add(game);
        }

        [RelayCommand]
        void AddToPlayed(Game game)
        {
            Remove(game);
            _gameStore.PlayedGames.Add(game);
        }
    }
}
