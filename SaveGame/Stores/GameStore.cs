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

        public void Remove(Game game)
        {
            PlayGames.Remove(game);
            PlayingGames.Remove(game);
            PlayedGames.Remove(game);
        }

        public void AddToPlay(Game game)
        {
            Remove(game);
            game.PlayState = PlayStates.Play;
            PlayGames.Add(game);
        }

        public void AddToPlaying(Game game)
        {
            Remove(game);
            game.PlayState = PlayStates.Playing;
            PlayingGames.Add(game);
        }

        public void AddToPlayed(Game game)
        {
            Remove(game);
            game.PlayState = PlayStates.Played;
            PlayedGames.Add(game);
        }
    }
}
