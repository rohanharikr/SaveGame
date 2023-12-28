using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IGDB;
using IGDB.Models;

namespace SaveGame.ViewModels
{
    partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        string searchQuery = "";

        [ObservableProperty]
        IEnumerable<Game> searchResults;

        IGDBClient igdb;

        public MainViewModel()
        {
            igdb = new IGDBClient(
                "x",
                "x"
            );
        }

        async partial void OnSearchQueryChanged(string value)
        {
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: $"fields *, artworks.*, cover.*; search \"{value}\"; limit 4;");
            SearchResults = games;
        }
    }
}
