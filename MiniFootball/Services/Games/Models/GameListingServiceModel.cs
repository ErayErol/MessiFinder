﻿namespace MiniFootball.Services.Games.Models
{
    using Data.Models;
    using System;

    public class GameListingServiceModel
    {
        public int Id { get; set; }

        public Playground Playground { get; set; }

        public DateTime Date { get; set; }
    }
}