using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IGDB;
using SaveGame.Models;
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

        [RelayCommand]
        void Remove(Game game) => _gameStore.Remove(game);

        [RelayCommand]
        void AddToPlay(Game game) => _gameStore.AddToPlay(game);

        [RelayCommand]
        void AddToPlayed(Game game) => _gameStore.AddToPlayed(game);

        [RelayCommand]
        void ShowGameDetailModal(Game game) => _modalNavigationStore.Show(game);

        [RelayCommand]
        void CloseGameDetailModal() => _modalNavigationStore.Close();

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
    }
}
