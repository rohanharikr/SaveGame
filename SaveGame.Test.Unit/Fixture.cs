using SaveGame.Models;

namespace SaveGame.Test.Unit
{
    public class Fixture
    {
        public static Game Mario()
        {
            return new Game()
            {
                Id = 0,
                Name = "Mario",
                Summary = "Its-a-me Mario!",
                AggregatedRating = 100,
            };
        }
    }
}
