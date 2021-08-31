﻿namespace MiniFootball.Controllers
{
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    using static GlobalConstant.Error;

    public class ErrorController : Controller
    {
        [Route(ErrorRoute)]
        public IActionResult Error(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            var errorViewModel = new ErrorViewModel();

            if (statusCode != 0)
            {
                errorViewModel = new ErrorViewModel
                {
                    StatusCode = statusCode,
                    Path = statusCodeResult.OriginalPath,
                    QueryString = statusCodeResult.OriginalQueryString,
                };

                if (statusCode == 404)
                {
                    errorViewModel.Message = ErrorMessage404;
                }
            }

            return View(errorViewModel);
        }
    }
}
