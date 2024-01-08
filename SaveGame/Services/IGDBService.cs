using IGDB;
using SaveGame.Models;
using Newtonsoft.Json.Linq;
using SaveGame.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveGame.Services
{
    public class IGDBService
    {
        readonly IGDBClient igdb;

        public IGDBService()
        {
            igdb = new IGDBClient(
                Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
                Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")
            );
        }

        readonly string fields = $"fields name, first_release_date, involved_companies.developer, involved_companies.company.name," +
            $"screenshots.image_id, screenshots.url, platforms.name, platforms.slug, platforms.platform_family.*, aggregated_rating, cover.url, cover.image_id," +
            $"language_supports.language.name, summary, genres.name, genres.slug, release_dates.y;";

        public async Task<IEnumerable<Game>> GetUpcomingReleases()
        {
            Int32 unixTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query:
                    fields +

                    $"where screenshots >= 3 & genres > 0 & summary != null & name ~ *\"\"* & version_parent = null &" +
                        $"(follows > 25 | hypes > 25) & first_release_date > {unixTime} & involved_companies.developer = true;" +

                    $"sort first_release_date asc;" +

                    $"limit 5;");
            
            return games;
        }

        public async Task<IEnumerable<Game>> GetRecentReleases()
        {
            int unixTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            int yearInUnixTime = 31536000;
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query:
                    fields +

                    $"where screenshots >= 3 & genres > 0 & summary != null & name ~ *\"\"* & version_parent = null &" +
                        $"(follows > 25 | hypes > 25) & first_release_date > {unixTime - yearInUnixTime} & first_release_date < {unixTime} & involved_companies.developer = true;" +

                    $"sort first_release_date asc;" +

                    $"limit 5;");
            
            return games;
        }

        private static int GetOffset()
        {
            Random random = new();
            return random.Next(0, 21) * 5;
        }

        public async Task<IEnumerable<Game>> GetHighRatedGames()
        {
            int offset = GetOffset();
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query:
                fields +

                $"where screenshots >= 3 & genres > 0 & summary != null & name ~ *\"\"* &" +
                    $"(follows != null | hypes != null) & version_parent = null &" +
                    $"involved_companies.developer = true & release_dates > 0;" +

                $"sort aggregated_rating desc;" +

                $"offset: {offset};" +

                $"limit 5;");
            
            return games;
        }

        public async Task<IEnumerable<Game>> SearchGame(string query)
        {
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query:
                fields +

                $"search \"{query}\";" +

                $"where screenshots >= 3 & genres > 0 & summary != null & name ~ *\"\"* &" +
                    $"(follows != null | hypes != null) & version_parent = null &" +
                    $"involved_companies.developer = true & release_dates > 0;" +

                $"limit 5;");
            
            return games;
        }
    }
}
