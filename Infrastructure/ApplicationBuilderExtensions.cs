﻿namespace MessiFinder.Infrastructure
{
    using System.Linq;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            var scopeService = app.ApplicationServices.CreateScope();

            var data = scopeService.ServiceProvider.GetService<MessiFinderDbContext>();

            data?.Database.Migrate();

            SeedPlaygrounds(data);

            return app;
        }

        private static void SeedPlaygrounds(MessiFinderDbContext data)
        {
            if (data.Playgrounds.Any())
            {
                return;
            }

            data.Playgrounds.AddRange(new[]
            {
                new Playground
                {
                    Name = "Avenue",
                    Country = "Bulgaria",
                    Town = "Haskovo",
                    Description = "In the summer this place is number 1 to play mini football.",
                    Address = "ул. Дунав 1 - в парка под супермаркет авеню",
                    ImageUrl = "https://imgrabo.com/pics/businesses/b18e8a5e845a9317f4e301b3ffd58c14.jpeg"
                },
                new Playground
                {
                    Name = "Kortove",
                    Country = "Bulgaria",
                    Town = "Haskovo",
                    Description = "In the winter this place is number 1 to play mini football, because the players play inside.",
                    Address = "След Хотел Европа - до тенис кортовете",
                    ImageUrl = "https://tennishaskovo.com/uploads/galerii/baza_kenana/44.jpg"
                },
                new Playground
                {
                    Name = "Yildizlar",
                    Country = "Turkey",
                    Town = "Edirne",
                    Description = "In the summer this place is number 1 to play mini football in Edirne.",
                    Address = "Ilk Okullun yaninda.",
                    ImageUrl = "https://hotel-evrika.com/wp-content/uploads/2019/12/VIK_6225-1024x683.jpg"
                },
                new Playground
                {
                    Name = "Optimum",
                    Country = "Bulgaria",
                    Town = "Plovdiv",
                    Description = "In summer and winter this place is number 1 to play mini football in Plovdiv.",
                    Address = "бул. „Асеновградско шосе",
                    ImageUrl = "https://imgrabo.com/pics/guide/900x600/20150901162641_20158.jpg"
                },
                new Playground
                {
                    Name = "Avangard Fitness",
                    Country = "Bulgaria",
                    Town = "Plovdiv",
                    Description = "You can workout in fitness and then play football with friends.",
                    Address = "жк. Тракия 96-Д, 4023 кв. Капитан Бураго",
                    ImageUrl = "https://fitness-avantgarde.com/sites/default/files/img_8189.jpg"
                },
            });

            data.SaveChanges();
        }
    }
}
