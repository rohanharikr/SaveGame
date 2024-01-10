using IGDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveGame.Models
{
    public class Game : IGDB.Models.Game
    {
        public PlayStates PlayState { get; set; }
        //override SimilarGames to use this.Game
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
