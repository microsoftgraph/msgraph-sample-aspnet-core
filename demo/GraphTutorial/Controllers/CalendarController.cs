// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

// <UsingSnippet>
using GraphTutorial.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphTutorial.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly ILogger<HomeController> _logger;

        public CalendarController(
            ITokenAcquisition tokenAcquisition,
            ILogger<HomeController> logger)
        {
            _tokenAcquisition = tokenAcquisition;
            _logger = logger;
        }

// </UsingSnippet>

        // <IndexSnippet>
        // Minimum permission scope needed for this view
        [AuthorizeForScopes(Scopes = new[] { "Calendars.Read" })]
        public async Task<IActionResult> Index()
        {
            try
            {
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
                    User.GetUserGraphTimeZone());
                var startOfWeek = CalendarController.GetUtcStartOfWeekInTimeZone(
                    DateTime.Today, userTimeZone);

                var events = await GetUserWeekCalendar(startOfWeek);

                var model = new CalendarViewModel(startOfWeek, events);

                return View(model);
            }
            catch (ServiceException ex)
            {
                if (ex.InnerException is MsalUiRequiredException)
                {
                    throw ex;
                }

                return View(new CalendarViewModel())
                    .WithError("Error getting calendar view", ex.Message);
            }
        }
        // </IndexSnippet>

// <SecondSnippet>
        private async Task<IList<Event>> GetUserWeekCalendar(DateTime startOfWeek)
        {
            var graphClient = GraphServiceClientFactory
                .GetAuthenticatedGraphClient(async () =>
                {
                    return await _tokenAcquisition
                        .GetAccessTokenForUserAsync(GraphConstants.Scopes);
                }
            );

            // Configure a calendar view for the current week
            var endOfWeek = startOfWeek.AddDays(7);

            var viewOptions = new List<QueryOption>
            {
                new QueryOption("startDateTime", startOfWeek.ToString("o")),
                new QueryOption("endDateTime", endOfWeek.ToString("o"))
            };

            var events = await graphClient.Me
                .CalendarView
                .Request(viewOptions)
                // Send user time zone in request so date/time in
                // response will be in preferred time zone
                .Header("Prefer", $"outlook.timezone=\"{User.GetUserGraphTimeZone()}\"")
                // Get max 50 per request
                .Top(50)
                // Only return fields app will use
                .Select(e => new
                {
                    e.Subject,
                    e.Organizer,
                    e.Start,
                    e.End
                })
                // Order results chronologically
                .OrderBy("start/dateTime")
                .GetAsync();

            IList<Event> allEvents;
            // Handle case where there are more than 50
            if (events.NextPageRequest != null)
            {
                allEvents = new List<Event>();
                // Create a page iterator to iterate over subsequent pages
                // of results. Build a list from the results
                var pageIterator = PageIterator<Event>.CreatePageIterator(
                    graphClient, events,
                    (e) => {
                        allEvents.Add(e);
                        return true;
                    }
                );
                await pageIterator.IterateAsync();
            }
            else
            {
                // If only one page, just use the result
                allEvents = events.CurrentPage;
            }

            return allEvents;
        }

        private static DateTime GetUtcStartOfWeekInTimeZone(DateTime today, TimeZoneInfo timeZone)
        {
            // Assumes Sunday as first day of week
            int diff = System.DayOfWeek.Sunday - today.DayOfWeek;

            // create date as unspecified kind
            var unspecifiedStart = DateTime.SpecifyKind(today.AddDays(diff), DateTimeKind.Unspecified);

            // convert to UTC
            return TimeZoneInfo.ConvertTimeToUtc(unspecifiedStart, timeZone);
        }
    }
}
// </SecondSnippet>
