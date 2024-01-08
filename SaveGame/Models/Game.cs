using IGDB;
using System.Reflection;

namespace SaveGame.Models
{
    public enum PlayStates
    {
        None,
        Play,
        Playing,
        Played
    }

    public class Game : IGDB.Models.Game
    {
        public PlayStates PlayState = PlayStates.None;
        public new IdentitiesOrValues<Game>? SimilarGames { get; set; }
    }
}
