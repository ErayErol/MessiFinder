﻿namespace MiniFootball.Services.Fields
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Games.Models;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class FieldService : IFieldService
    {
        private readonly MiniFootballDbContext data;
        private readonly IConfigurationProvider mapper;

        public FieldService(
            MiniFootballDbContext data,
            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public FieldQueryServiceModel All(
            string cityName,
            string searchTerm,
            Sorting sorting,
            int currentPage,
            int fieldsPerPage)
        {
            var fieldsQuery = data.Fields.AsQueryable();

            var city = data
                .Cities
                .FirstOrDefault(c => c.Name == cityName);

            if (string.IsNullOrWhiteSpace(city?.Name) == false)
            {
                fieldsQuery = fieldsQuery
                    .Where(g => g.City.Name == city.Name);
            }

            if (string.IsNullOrWhiteSpace(searchTerm) == false)
            {
                fieldsQuery = fieldsQuery
                    .Where(g => g
                        .Name
                        .ToLower()
                        .Contains(searchTerm.ToLower()));
            }

            fieldsQuery = sorting switch
            {
                Sorting.City
                    => fieldsQuery
                        .OrderBy(g => g.City.Name),
                Sorting.FieldName
                    => fieldsQuery
                        .OrderBy(g => g.Name),
                Sorting.DateCreated or _
                    => fieldsQuery
                        .OrderBy(g => g.Id)
            };

            var totalPlaygrounds = fieldsQuery.Count();

            var fields = GetFields(fieldsQuery
                .Skip((currentPage - 1) * fieldsPerPage)
                .Take(fieldsPerPage), mapper)
                .ToList();

            return new FieldQueryServiceModel
            {
                Fields = fields,
                TotalFields = totalPlaygrounds
            };
        }

        public bool IsCorrectCountryAndCity(int fieldId, string name, string countryName, string cityName)
        {
            var field = data.Fields.FirstOrDefault(f => f.Id == fieldId);

            if (field == null)
            {
                return false;
            }

            field.Country = data.Countries.Find(field.CountryId);
            field.City = data.Cities.Find(field.CityId);

            return
                field != null &&
                field.Name.ToLower() == name.ToLower() &&
                field.Country.Name.ToLower() == countryName.ToLower() &&
                field.City.Name.ToLower() == cityName.ToLower();
        }

        public int Create(string name,
            int countryId,
            int cityId,
            string address,
            string imageUrl,
            string phoneNumber,
            bool parking,
            bool cafe,
            bool shower,
            bool changingRoom,
            string description, 
            int adminId)
        {
            var country = data
                .Countries
                .Where(c => c.Id == countryId)
                .Select(c => c.Id)
                .FirstOrDefault();

            var city = data
                .Cities
                .Where(c => c.Id == cityId)
                .Select(c => c.Id)
                .FirstOrDefault();

            var field = new Field
            {
                Name = name,
                CountryId = country,
                CityId = city,
                Address = address,
                ImageUrl = imageUrl,
                PhoneNumber = phoneNumber,
                Parking = parking,
                Cafe = cafe,
                Shower = shower,
                ChangingRoom = changingRoom,
                Description = description,
                AdminId = adminId
            };

            data.Fields.Add(field);
            data.SaveChanges();

            return field.Id;
        }

        public bool IsExist(string name, int countryId, int cityId)
            => data
                .Fields
                .Any(p => p.Name == name && p.Country.Id == countryId && p.City.Id == cityId);

        public IEnumerable<string> Cities()
            => data
                .Fields
                .Select(p => p.City.Name)
                .Distinct()
                .OrderBy(t => t)
                .AsEnumerable();

        public IEnumerable<FieldListingServiceModel> FieldsListing(string cityName, string countryName)
            => data
                .Fields
                .Where(x => x.City.Name == cityName && x.Country.Name == countryName)
                .ProjectTo<FieldListingServiceModel>(mapper)
                .ToList();

        public bool FieldExist(int fieldId)
            => data
                .Fields
                .Any(p => p.Id == fieldId);

        public string FieldName(int fieldId)
            => data
                .Fields
                .Where(f => f.Id == fieldId)
                .Select(f => f.Name)
                .FirstOrDefault();

        public FieldDetailServiceModel GetDetails(int id)
        {
            return data
                .Fields
                .Where(f => f.Id == id)
                .ProjectTo<FieldDetailServiceModel>(mapper)
                .FirstOrDefault();
        }

        public bool Edit(
            int id,
            string name,
            string address,
            string imageUrl,
            bool parking,
            bool shower,
            bool changingRoom,
            bool cafe,
            string description)
        {
            var field = data.Fields.Find(id);

            if (field == null)
            {
                return false;
            }

            field.Name = name;
            field.Address = address;
            field.ImageUrl = imageUrl;
            field.Parking = parking;
            field.Shower = shower;
            field.ChangingRoom = changingRoom;
            field.Cafe = cafe;
            field.Description = description;

            data.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var field = data.Fields.Find(id);

            if (field == null)
            {
                return false;
            }

            var allGames = data
                .Games
                .Where(g => g.FieldId == field.Id);

            data.Games.RemoveRange(allGames);
            data.SaveChanges();

            data.Fields.Remove(field);
            data.SaveChanges();

            return true;
        }

        public IEnumerable<FieldServiceModel> ByUser(string userId)
        {
            var games = GetFields(
                data
                    .Fields
                    .Where(g => g.Admin.UserId == userId),
                mapper);

            return games;
        }

        public bool IsByAdmin(int id, int adminId)
        {
            var fields = data
                .Fields
                .FirstOrDefault(c => c.Id == id && c.AdminId == adminId);

            if (fields == null)
            {
                return false;
            }

            return true;
        }

        private static IEnumerable<FieldServiceModel> GetFields(
            IQueryable<Field> fieldQuery,
            IConfigurationProvider mapper)
            => fieldQuery
                .ProjectTo<FieldServiceModel>(mapper)
                .ToList();
    }
}
