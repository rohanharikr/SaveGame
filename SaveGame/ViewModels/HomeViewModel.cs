using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaveGame.Models;
using SaveGame.Services;
using SaveGame.Stores;

namespace SaveGame.ViewModels
{
    partial class HomeViewModel : ObservableObject
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly GameStore _gameStore;

        [ObservableProperty]
        bool isFetchingUpcomingReleases = true;

        [ObservableProperty]
        IEnumerable<Game>? upcomingReleases;

        [ObservableProperty]
        bool isFetchingRecentReleases = true;

        [ObservableProperty]
        IEnumerable<Game>? recentReleases;

        [ObservableProperty]
        bool isFetchingHighRatedGames = true;

        [ObservableProperty]
        IEnumerable<Game>? highRatedGames;

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

            greeting = "Good " + TimeOfDay();

            _modalNavigationStore = modalNavigationStore;
            _gameStore = gameStore;
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

        private static string TimeOfDay()
        {
            int hour = DateTime.Now.Hour;
            if (hour >= 18)
                return "Evening";
            else if (hour >= 12)
                return "Afternoon";
            else
                return "Morning";
        }
    }
}
