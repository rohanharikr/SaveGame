using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SaveGame.Models;

namespace SaveGame.Stores
{
    public class GameStore
    {
        private ObservableCollection<Game> _playGames = new();
        public ObservableCollection<Game> PlayGames
        {
            get { return _playGames; }
            set
            {
                _playGames = value;
                GamesChanged?.Invoke();
            }
        }

        private ObservableCollection<Game> _playingGames = new();
        public ObservableCollection<Game> PlayingGames
        {
            get { return _playingGames; }
            set
            {
                _playingGames = value;
                GamesChanged?.Invoke();
            }
        }

        private ObservableCollection<Game> _playedGames = new();
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

        public void Remove(Game game)
        {
            if (PlayGames.Contains(game))
                PlayGames.Remove(game);
            else if (PlayingGames.Contains(game))
                PlayingGames.Remove(game);
            else if (PlayedGames.Contains(game))
                PlayedGames.Remove(game);
        }

        public void AddToPlay(Game game)
        {
            Remove(game);
            PlayGames.Add(game);
        }

        public void AddToPlaying(Game game)
        {
            Remove(game);
            PlayingGames.Add(game);
        }

        public void AddToPlayed(Game game)
        {
            Remove(game);
            PlayedGames.Add(game);
        }
    }
}
