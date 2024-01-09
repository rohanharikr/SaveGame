﻿using IGDB.Models;
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
        }

        private async void RemoveFromDb(Game game)
        {
        }

        public async Task RetreiveGamesFromDb()
        {
        }
    }
}
