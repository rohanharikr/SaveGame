using LiteDB;
using SaveGame.Models;
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

        public GameStore()
        {
            PlayGames.CollectionChanged += (s,e) => GamesChanged?.Invoke();
            PlayingGames.CollectionChanged += (s,e) => GamesChanged?.Invoke();
            PlayedGames.CollectionChanged += (s,e) => GamesChanged?.Invoke();
        }

        public void Retrieve()
        {
            using (var db = new LiteDatabase("Data.db"))
            {
                PlayGames = [..db.GetCollection<Game>("Play").FindAll().ToList()];
                PlayingGames = [..db.GetCollection<Game>("Playing").FindAll().ToList()];
                PlayedGames = [..db.GetCollection<Game>("Played").FindAll().ToList()];
            }
        }

        public void Remove(Game game)
        {
            var gameToRemove = PlayGames.SingleOrDefault(i => i.Id == game.Id);
            if (gameToRemove != null)
                PlayGames.Remove(gameToRemove);

            gameToRemove = PlayingGames.SingleOrDefault(i => i.Id == game.Id);
            if (gameToRemove != null)
                PlayingGames.Remove(gameToRemove);

            gameToRemove = PlayedGames.SingleOrDefault(i => i.Id == game.Id);
            if (gameToRemove != null)
                PlayedGames.Remove(gameToRemove);
        }

        public void AddToPlay(Game game)
        {
            Remove(game);
            game.PlayState = PlayStates.Play;
            PlayGames.Add(game);
            using (var db = new LiteDatabase("Data.db"))
            {
                var col = db.GetCollection<Game>("Play");
                col.Insert(game);
            }
        }

        public void AddToPlaying(Game game)
        {
            Remove(game);
            game.PlayState = PlayStates.Playing;
            PlayingGames.Add(game);
            using (var db = new LiteDatabase("Data.db"))
            {
                var col = db.GetCollection<Game>("Playing");
                col.Insert(game);
            }
        }

        public void AddToPlayed(Game game)
        {
            Remove(game);
            game.PlayState = PlayStates.Played;
            PlayedGames.Add(game);
            using (var db = new LiteDatabase("Data.db"))
            {
                var col = db.GetCollection<Game>("Played");
                col.Insert(game);
            }
        }
    }
}
