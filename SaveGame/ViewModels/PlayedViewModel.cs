using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaveGame.Models;
using SaveGame.Stores;
using System.Collections.ObjectModel;

namespace SaveGame.ViewModels
{
    partial class PlayedViewModel : ObservableObject
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly GameStore _gameStore;

        public Game? GameDetail => _modalNavigationStore.Detail;
        public bool IsGameDetailModalOpen => _modalNavigationStore.IsOpen;

        public ObservableCollection<Game> PlayedGames => _gameStore.PlayedGames;

        [RelayCommand]
        void Remove(Game game) => _gameStore.Remove(game);

        [RelayCommand]
        void AddToPlay(Game game) => _gameStore.AddToPlay(game);

        [RelayCommand]
        void AddToPlaying(Game game) => _gameStore.AddToPlaying(game);

        [RelayCommand]
        void ShowGameDetailModal(Game game) => _modalNavigationStore.Show(game);

        [RelayCommand]
        void CloseGameDetailModal() => _modalNavigationStore.Close();

        private void GameStore_GamesChanged() => OnPropertyChanged(nameof(PlayedGames));

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

    }
}
