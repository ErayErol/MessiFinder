﻿namespace MiniFootball.Models.Games
{
    using Services.Games.Models;
    using System.Collections.Generic;

    public class GameAllQueryModel
    {
        public int GamesPerPage = 3;

        public int CurrentPage { get; set; } = 1;

        public string City { get; set; }

        public string SearchTerm { get; set; }

        public int TotalGames { get; set; }

        public Sorting Sorting { get; set; }

        public IEnumerable<string> Cities { get; set; }

        public IEnumerable<GameListingServiceModel> Games { get; set; }
    }
}
