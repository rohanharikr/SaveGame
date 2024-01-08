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
