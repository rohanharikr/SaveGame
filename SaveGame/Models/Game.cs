using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveGame.Models
{
    public class Game : IGDB.Models.Game
    {
        public PlayStates PlayState = PlayStates.None;
    }

    public enum PlayStates
    {
        None,
        Play,
        Playing,
        Played
    }
}
