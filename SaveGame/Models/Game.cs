using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IGDB;

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
    }
}
