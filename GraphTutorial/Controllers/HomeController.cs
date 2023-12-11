// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Diagnostics;
using GraphTutorial.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraphTutorial.Controllers;

/// <summary>
/// The controller for the home page.
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeController"/> class.
    /// </summary>
    /// <param name="logger">An <see cref="ILogger"/> to use for logging.</param>
    public HomeController(ILogger<HomeController> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Loads the home page.
    /// </summary>
    /// <returns>The home page.</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Loads the privacy page.
    /// </summary>
    /// <returns>The privacy page.</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Loads the error page.
    /// </summary>
    /// <returns>The error page.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /// <summary>
    /// Loads the error page with custom message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="debug">Additional debug information.</param>
    /// <returns>The error page.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public IActionResult ErrorWithMessage(string message, string debug)
    {
        return View("Index").WithError(message, debug);
    }
}
