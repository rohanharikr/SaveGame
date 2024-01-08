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

        public object Clone()
        {
            Game clonedGame = new Game();
            PropertyInfo[] properties = typeof(Game).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    property.SetValue(clonedGame, property.GetValue(this));
                }
            }

            return clonedGame;
        }
    }
}
