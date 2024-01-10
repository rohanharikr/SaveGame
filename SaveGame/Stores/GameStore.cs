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
            //TBD Create LiteDBService
            using var db = new LiteDatabase("Data.db");
            foreach (var game in db.GetCollection<Game>("Play").FindAll())
                PlayGames.Add(game);
            foreach (var game in db.GetCollection<Game>("Playing").FindAll())
                PlayingGames.Add(game);
            foreach (var game in db.GetCollection<Game>("Played").FindAll())
                PlayedGames.Add(game);
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

            //TBD Create LiteDBService
            using var db = new LiteDatabase("Data.db");
            var playDb = db.GetCollection<Game>("Play");
            var playingDb = db.GetCollection<Game>("Playing");
            var playedDb = db.GetCollection<Game>("Played");

            var query = Query.EQ("_id", game.Id);
            var isInPlay = playDb.Exists(query);
            var isInPlaying = playingDb.Exists(query);
            var isInPlayed = playedDb.Exists(query);
            var isInPlayed2 = playedDb.Exists(query);

            if (isInPlay)
                playDb.Delete(game.Id);
            else if (isInPlaying)
                playingDb.Delete(game.Id);
            else if (isInPlayed)
                playedDb.Delete(game.Id);
        }

        public void AddToPlay(Game game)
        {
            Remove(game);
            game.PlayState = PlayStates.Play;
            PlayGames.Add(game);
            
            //TBD Create LiteDBService
            using var db = new LiteDatabase("Data.db");
            var col = db.GetCollection<Game>("Play");
            col.Insert(game);
        }

        public void AddToPlaying(Game game)
        {
            Remove(game);
            game.PlayState = PlayStates.Playing;
            PlayingGames.Add(game);
            
            //TBD Create LiteDBService
            using var db = new LiteDatabase("Data.db");
            var col = db.GetCollection<Game>("Playing");
            col.Insert(game);
        }

        public void AddToPlayed(Game game)
        {
            Remove(game);
            game.PlayState = PlayStates.Played;
            PlayedGames.Add(game);
            
            //TBD Create LiteDBService
            using var db = new LiteDatabase("Data.db");
            var col = db.GetCollection<Game>("Played");
            col.Insert(game);
        }

        public ObservableCollection<Game> UpdateGameState(ObservableCollection<Game> games)
        {
            if (games == null)
                return [];

            foreach (var game in games)
            {
                if (PlayGames.FirstOrDefault(i => i.Id == game.Id) != null)
                    game.PlayState = PlayStates.Play;
                else if (PlayingGames.FirstOrDefault(i => i.Id == game.Id) != null)
                    game.PlayState = PlayStates.Playing;
                else if (PlayedGames.FirstOrDefault(i => i.Id == game.Id) != null)
                    game.PlayState = PlayStates.Played;
                else
                    game.PlayState = PlayStates.None;
            }
            return new ObservableCollection<Game>(games);
        }
    }
}
