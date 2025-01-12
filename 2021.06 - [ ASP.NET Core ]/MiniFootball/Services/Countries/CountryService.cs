﻿namespace MiniFootball.Services.Countries
{
    using Data;
    using Data.Models;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using static GlobalConstants;

    public class CountryService : ICountryService
    {
        private List<string> allCountries;

        private readonly MiniFootballDbContext data;
        private readonly IMemoryCache cache;

        public CountryService(MiniFootballDbContext data,
                              IMemoryCache cache)
        {
            this.data = data;
            this.cache = cache;
            allCountries = new List<string>();
        }

        public List<string> All()
        {
            allCountries = cache.Get<List<string>>(LatestGamesCacheKey);

            if (allCountries == null)
            {
                allCountries = data
                    .Countries
                    .Select(country => country.Name)
                    .ToList();
                
                allCountries.Sort();

                var cacheOptions = 
                    new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromDays(365));

                cache.Set(LatestGamesCacheKey, allCountries, cacheOptions);
            }

            return allCountries;
        }

        public void SaveAll()
        {
            var cultureList = new List<string>();

            var getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (var getCulture in getCultureInfo)
            {
                var getRegionInfo = new RegionInfo(getCulture.LCID);

                if (cultureList.Contains(getRegionInfo.EnglishName) == false)
                {
                    cultureList.Add(getRegionInfo.EnglishName);
                    
                    var country = new Country
                    {
                        Name = getRegionInfo.EnglishName
                    };

                    data.Countries.Add(country);
                    data.SaveChanges();
                }
            }
        }

        public string Name(int id)
            => data
                .Countries
                .Where(c => c.Id == id)
                .Select(c => c.Name)
                .FirstOrDefault();

        public int CountryIdByName(string name)
            => data
                .Countries
                .Where(c => c.Name == name)
                .Select(c => c.Id)
                .FirstOrDefault();
    }
}