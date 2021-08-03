﻿namespace MiniFootball.Services.Statistics
{
    using Data;
    using System.Linq;

    public class StatisticsService : IStatisticsService
    {
        private readonly MiniFootballDbContext data;

        public StatisticsService(MiniFootballDbContext data) 
            => this.data = data;

        public StatisticsServiceModel Total()
        {
            var totalGames = this.data.Games.Count();
            var totalPlaygrounds = this.data.Playgrounds.Count();
            var totalUsers = this.data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalGames = totalGames,
                TotalPlaygrounds = totalPlaygrounds,
                TotalUsers = totalUsers,
            };
        }
    }
}