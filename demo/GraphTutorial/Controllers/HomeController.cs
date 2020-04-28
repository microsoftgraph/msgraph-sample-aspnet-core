// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using GraphTutorial.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GraphTutorial.Controllers
{
    public class HomeController : Controller
    {
        ITokenAcquisition _tokenAcquisition;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ITokenAcquisition tokenAcquisition,
            ILogger<HomeController> logger)
        {
            _tokenAcquisition = tokenAcquisition;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // TEMPORARY
            // Get the token and display it
            string token = await _tokenAcquisition
                .GetAccessTokenForUserAsync(GraphConstants.Scopes);
            return View().WithInfo("Token acquired", token);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult ErrorWithMessage(string message, string debug)
        {
            return View("Index").WithError(message, debug);
        }
    }
}
