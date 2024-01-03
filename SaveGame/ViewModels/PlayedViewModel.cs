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
    partial class PlayedViewModel : ObservableObject
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly GameStore _gameStore;

        public Game? GameDetail => _modalNavigationStore.Detail;
        public bool IsGameDetailModalOpen => _modalNavigationStore.IsOpen;

        public ObservableCollection<Game> PlayedGames => _gameStore.PlayedGames;

        public PlayedViewModel(ModalNavigationStore modalNavigationStore, GameStore gameStore)
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
            OnPropertyChanged(nameof(PlayedGames));
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
            _gameStore.PlayingGames.Add(game);
        }

        [RelayCommand]
        void AddToPlayed(Game game)
        {
            _gameStore.PlayedGames.Add(game);
        }
    }
}
