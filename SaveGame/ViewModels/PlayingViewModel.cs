using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaveGame.Models;
using SaveGame.Stores;
using System.Collections.ObjectModel;

namespace SaveGame.ViewModels
{
    partial class PlayingViewModel(ModalNavigationStore modalNavigationStore, GameStore gameStore) : ObservableObject
    {
        private readonly ModalNavigationStore _modalNavigationStore = modalNavigationStore;
        private readonly GameStore _gameStore = gameStore;

        public ObservableCollection<Game> PlayingGames => _gameStore.PlayingGames;

        [RelayCommand]
        void Remove(Game game) => _gameStore.Remove(game);

        [RelayCommand]
        void AddToPlay(Game game) => _gameStore.AddToPlay(game);

        [RelayCommand]
        void AddToPlayed(Game game) => _gameStore.AddToPlayed(game);

        [RelayCommand]
        void ShowGameDetailModal(Game game) => _modalNavigationStore.Show(game);
    }
}
