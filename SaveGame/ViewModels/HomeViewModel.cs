 using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaveGame.Models;
using SaveGame.Services;
using SaveGame.Stores;
using System.Collections.ObjectModel;

namespace SaveGame.ViewModels
{
    partial class HomeViewModel : ObservableObject
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly GameStore _gameStore;

        [ObservableProperty]
        ObservableCollection<Game> suggestedGames;
        [ObservableProperty]
        bool isFetchingUpcomingReleases = true;

        [ObservableProperty]
        ObservableCollection<Game> upcomingReleases;

        [ObservableProperty]
        bool isFetchingRecentReleases = true;

        [ObservableProperty]
        ObservableCollection<Game> recentReleases;

        [ObservableProperty]
        bool isFetchingHighRatedGames = true;

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

            greeting = "Good " + TimeOfDay();

            _modalNavigationStore = modalNavigationStore;
            _gameStore = gameStore;

            _gameStore.GamesChanged += SuggestGames;
            _gameStore.GamesChanged += UpdateGameStates;
        }

        private ObservableCollection<Game> GamesWithUpdatedState(ObservableCollection<Game> games)
        {
            if (games == null)
                return new ObservableCollection<Game>();

            foreach (var game in games)
            {
                if (_gameStore.PlayGames.FirstOrDefault(i => i.Id == game.Id) != null)
                    game.PlayState = PlayStates.Play;
                else if (_gameStore.PlayingGames.FirstOrDefault(i => i.Id == game.Id) != null)
                    game.PlayState = PlayStates.Playing;
                else if (_gameStore.PlayedGames.FirstOrDefault(i => i.Id == game.Id) != null)
                    game.PlayState = PlayStates.Played;
                else
                    game.PlayState = PlayStates.None;
            }
            return new ObservableCollection<Game>(games);
        }

        void UpdateGameStates()
        {
            //SuggestGames does not need to be updated as tracked games do not show up here
            HighRatedGames = GamesWithUpdatedState(HighRatedGames);
            RecentReleases = GamesWithUpdatedState(RecentReleases);
            UpcomingReleases = GamesWithUpdatedState(UpcomingReleases);
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
               //games w/ highest inetersections gets highest priority in list
               .OrderByDescending(group => group.Count())
               .SelectMany(group => group)
               //remove duplicate suggestions
               .DistinctBy(g => g.Id)
               //do not suggest games that are already being tracked
               .Where(game => game.PlayState == PlayStates.None)
               .Take(5)
               .ToList();

            SuggestedGames = new ObservableCollection<Game>(suggestedGamesProritsed);
            if(SuggestedGames.Count > 0 )
                SuggestedGames[0].PlayState = PlayStates.Played;
        }

        async void GetGames(IGDBService igdbService)
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
