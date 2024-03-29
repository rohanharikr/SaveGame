﻿using IGDB;
using SaveGame.Models;
using SaveGame.Stores;
using System.Text;

namespace SaveGame.Services
{
    public class IGDBService(GameStore gameStore)
    {
        readonly IGDBClient igdb = new(
                Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
                Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")
            );

        readonly string queryField = MakeQueryField();
        readonly GameStore _gameStore = gameStore;
        readonly int unixTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        const int yearInUnixTime = 31536000;

        public async Task<IEnumerable<Game>> GetUpcomingReleases()
        {
            //TBD Handle exception
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query:
                queryField +

                $"where screenshots >= 3 & genres > 0 & summary != null & name ~ *\"\"* & version_parent = null &" +
                    $"(follows > 25 | hypes > 25) & first_release_date > {unixTime} & involved_companies.developer = true;" +

                $"sort first_release_date asc;" +

                $"limit 5;");

            return GamesWithUpdatedState(games);
        }

        public async Task<IEnumerable<Game>> GetRecentReleases()
        {
            //TBD Handle exception
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query:
                queryField +

                $"where screenshots >= 3 & genres > 0 & summary != null & name ~ *\"\"* & version_parent = null &" +
                    $"(follows > 25 | hypes > 25) & first_release_date > {unixTime - yearInUnixTime} & first_release_date < {unixTime} & involved_companies.developer = true;" +

                $"sort first_release_date asc;" +

                $"limit 5;");

            return GamesWithUpdatedState(games);
        }

        public async Task<IEnumerable<Game>> GetHighRatedGames()
        {
            int offset = Utility.GetOffset();

            //TBD Handle exception
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query:
                queryField +

                $"where screenshots >= 3 & genres > 0 & summary != null & name ~ *\"\"* &" +
                    $"(follows != null | hypes != null) & version_parent = null &" +
                    $"involved_companies.developer = true & release_dates > 0;" +

                $"sort aggregated_rating desc;" +

                $"offset: {offset};" +

                $"limit 5;");

            return GamesWithUpdatedState(games);
        }

        public async Task<IEnumerable<Game>> SearchGame(string query)
        {
            //TBD Handle exception
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query:
                queryField +

                $"search \"{query}\";" +

                $"where screenshots >= 3 & genres > 0 & summary != null & name ~ *\"\"* &" +
                    $"(follows != null | hypes != null) & version_parent = null &" +
                    $"involved_companies.developer = true & release_dates > 0;" +

                $"limit 5;");

            return GamesWithUpdatedState(games);
        }

        private Game[] GamesWithUpdatedState(Game[] games)
        {
            foreach(var game in games)
            {
                if (_gameStore.PlayGames.FirstOrDefault(i => i.Id == game.Id) != null)
                    game.PlayState = PlayStates.Play;
                else if (_gameStore.PlayingGames.FirstOrDefault(i => i.Id == game.Id) != null)
                    game.PlayState = PlayStates.Playing;
                else if (_gameStore.PlayedGames.FirstOrDefault(i => i.Id == game.Id) != null)
                    game.PlayState = PlayStates.Played;
                else
                    game.PlayState = PlayStates.None;
            }
            return games;
        }

        static string MakeQueryField()
        {
            StringBuilder q = new();
            List<string> baseFields =
            [
                "name", "first_release_date", "involved_companies.developer", "involved_companies.company.name",
                "screenshots.image_id", "screenshots.url", "platforms.name", "platforms.slug", "platforms.platform_family.*",
                "aggregated_rating", "cover.url", "cover.image_id", "language_supports.language.name", "summary", "genres.name",
                "genres.slug", "release_dates.y"
            ];

            q.Append(string.Join(",", baseFields));

            foreach (string field in baseFields)
                q.Append(",similar_games." + field);

            return "fields " + q.ToString() + ";";
        }
    }
}
