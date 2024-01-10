using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaveGame.Models;
using SaveGame.Stores;
using System.Collections.ObjectModel;

namespace SaveGame.ViewModels
{
    partial class PlayedViewModel(ModalNavigationStore modalNavigationStore, GameStore gameStore) : ObservableObject
    {
        private readonly ModalNavigationStore _modalNavigationStore = modalNavigationStore;
        private readonly GameStore _gameStore = gameStore;

        public ObservableCollection<Game> PlayedGames => _gameStore.PlayedGames;

        [RelayCommand]
        void Remove(Game game) => _gameStore.Remove(game);

        [RelayCommand]
        void AddToPlay(Game game) => _gameStore.AddToPlay(game);

        [RelayCommand]
        void AddToPlaying(Game game) => _gameStore.AddToPlaying(game);

        [RelayCommand]
        void ShowGameDetailModal(Game game) => _modalNavigationStore.Show(game);
    }
}
