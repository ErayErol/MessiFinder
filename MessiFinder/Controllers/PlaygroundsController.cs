﻿namespace MessiFinder.Controllers
{
    using Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.Playgrounds;
    using Services.Admins;
    using Services.Countries;
    using Services.Playgrounds;

    public class PlaygroundsController : Controller
    {
        private readonly ICountryService country;
        private readonly IAdminService admin;
        private readonly IPlaygroundService playground;


        public PlaygroundsController(
            ICountryService country,
            IAdminService admin,
            IPlaygroundService playground)
        {
            this.country = country;
            this.admin = admin;
            this.playground = playground;
        }

        [Authorize]
        public IActionResult Create()
        {
            if (this.User.IsManager() == false)
            {
                return View();
            }

            return View(new PlaygroundCreateFormModel
            {
                Countries = this.country.All(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(PlaygroundCreateFormModel playgroundModel)
        {
            if (this.User.IsManager() == false)
            {
                return RedirectToAction(nameof(AdminsController.Become), "Admins");
            }

            if (ModelState.IsValid == false)
            {
                playgroundModel.Countries = this.country.All();
                return View(playgroundModel);
            }

            if (this.playground.IsSame(
                    playgroundModel.Name,
                    playgroundModel.Country,
                    playgroundModel.Town,
                    playgroundModel.Address))
            {
                // TODO: There are already exist playground with this name, country, town, address (render in page)
                return View(playgroundModel);
            }

            this.playground.Create(
                playgroundModel.Name,
                playgroundModel.Country,
                playgroundModel.Town,
                playgroundModel.Address,
                playgroundModel.ImageUrl,
                playgroundModel.PhoneNumber,
                playgroundModel.Parking,
                playgroundModel.Cafe,
                playgroundModel.Shower,
                playgroundModel.ChangingRoom,
                playgroundModel.Description);

            return RedirectToAction(nameof(All));
        }

        public IActionResult All([FromQuery] PlaygroundAllQueryModel query)
        {
            var queryResult = this.playground.All(
                query.Town,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.PlaygroundsPerPage);

            var towns = this.playground.Towns();

            query.TotalPlaygrounds = queryResult.TotalPlaygrounds;
            query.Playgrounds = queryResult.Playgrounds;
            query.Towns = towns;

            return View(query);
        }
    }
}