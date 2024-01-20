using IGDB;

namespace SaveGame.Models
{
    public class Game : IGDB.Models.Game
    {
        public PlayStates PlayState { get; set; }
        //override inherited Game.SimilarGames to use this.Game
        public new IdentitiesOrValues<Game>? SimilarGames { get; set; }
    }

    public enum PlayStates
    {
        None,
        Play,
        Playing,
        Played
    }
}
