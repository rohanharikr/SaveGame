using SaveGame.Models;
using SaveGame.Services;
using System.Collections.ObjectModel;

namespace SaveGame.Stores
{
    public class GameStore
    {
        private ObservableCollection<Game> _playGames = [];
        public ObservableCollection<Game> PlayGames
        {
            get { return _playGames; }
            set
            {
                _playGames = value;
                GamesChanged?.Invoke();
            }
        }

        private ObservableCollection<Game> _playingGames = [];
        public ObservableCollection<Game> PlayingGames
        {
            get { return _playingGames; }
            set
            {
                _playingGames = value;
                GamesChanged?.Invoke();
            }
        }

        private ObservableCollection<Game> _playedGames = [];
        public ObservableCollection<Game> PlayedGames
        {
            get { return _playedGames; }
            set
            {
                _playedGames = value;
                GamesChanged?.Invoke();
            }
        }

        public event Action? GamesChanged;

        private readonly IGDBService _igdbService;

        public GameStore(IGDBService igdbService)
        {
            PlayGames.CollectionChanged += (s,e) => GamesChanged?.Invoke();
            PlayingGames.CollectionChanged += (s,e) => GamesChanged?.Invoke();
            PlayedGames.CollectionChanged += (s,e) => GamesChanged?.Invoke();

            _igdbService = igdbService;
        }

        public void Remove(Game game)
        {
            PlayGames.Remove(PlayGames.SingleOrDefault(i => i.Id == game.Id));
            PlayingGames.Remove(PlayingGames.SingleOrDefault(i => i.Id == game.Id));
            PlayedGames.Remove(PlayedGames.SingleOrDefault(i => i.Id == game.Id));
            RemoveFromDb(game);
        }


        public void AddToPlay(Game game)
        {
            Remove(game);
            game.PlayState = PlayStates.Play;
            PlayGames.Add(game);
            AddToDb(game);
        }

        public void AddToPlaying(Game game)
        {
            Remove(game);
            game.PlayState = PlayStates.Playing;
            PlayingGames.Add(game);
            AddToDb(game);
        }

        public void AddToPlayed(Game game)
        {
            Remove(game);
            game.PlayState = PlayStates.Played;
            PlayedGames.Add(game);
            AddToDb(game);
        }
        
        private async void AddToDb(Game game)
        {
            using SQLiteService context = new SQLiteService();
            GameDBSchema gameObj = new GameDBSchema()
            {
                Id = game.Id,
                PlayState = game.PlayState
            };
            context.Games.Add(gameObj);
            await context.SaveChangesAsync();

        }

        private async void RemoveFromDb(Game game)
        {
            using SQLiteService context = new SQLiteService();
            GameDBSchema gameObj = context.Games.SingleOrDefault(x => x.Id == game.Id);
            if (gameObj == null)
                return;
            context.Games.Remove(gameObj);
            await context.SaveChangesAsync();
        }

        public async Task RetreiveGamesFromDb()
        {
            using (SQLiteService context = new SQLiteService())
            {
                var dbGames = context.Games.ToList();
                if (dbGames.Count == 0)
                    return;

                var gameTasks = dbGames.Select(game => FetchGameAsync(game.Id)).ToList();
                var games = await Task.WhenAll(gameTasks);

                foreach (var game in games)
                {
                    PlayStates state = dbGames.Find(i => i.Id == game.Id).PlayState;
                    switch (state)
                    {
                        case PlayStates.Play:
                            game.PlayState = PlayStates.Play;
                            PlayGames.Add(game);
                            break;

                        case PlayStates.Playing:
                            game.PlayState = PlayStates.Playing;
                            PlayingGames.Add(game);
                            break;

                        case PlayStates.Played:
                            game.PlayState = PlayStates.Played;
                            PlayedGames.Add(game);
                            break;
                    }
                }
            }
        }

        private async Task<Game> FetchGameAsync(long? id)
        {
            //TBD Handle exceptions
            var result = await _igdbService.SearchGameById(id);
            return result;
        }
    }
}
