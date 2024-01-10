using IGDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
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

        public GameStore()
        {
            PlayGames.CollectionChanged += (s,e) => GamesChanged?.Invoke();
            PlayingGames.CollectionChanged += (s,e) => GamesChanged?.Invoke();
            PlayedGames.CollectionChanged += (s,e) => GamesChanged?.Invoke();
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

        private void RetrieveFromDb()
        {
            using SQLiteService context = new();
            PlayGames = [.. context.Play];
            PlayingGames = [.. context.Playing];
            PlayedGames = [.. context.Played];
        }

        private static void RemoveFromDb(Game game)
        {
            using SQLiteService context = new();
            Game existingGame = context.Play
                .FirstOrDefault(g => g.Id == game.Id) ??
                context.Playing
                    .FirstOrDefault(g => g.Id == game.Id) ??
                context.Played
                    .FirstOrDefault(g => g.Id == game.Id)!;

            if (existingGame != null)
            {
                context.Remove(existingGame);
                context.SaveChanges();
            }
        }

        private static void AddToDb(Game game)
        {
            using SQLiteService context = new();
            context.Play.Add(game);
            context.SaveChanges();
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
    }
}
