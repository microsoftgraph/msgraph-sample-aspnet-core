// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using GraphTutorial.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using Microsoft.Identity.Web;
using TimeZoneConverter;

namespace GraphTutorial.Controllers
{
    /// <summary>
    /// The controller for the calendar views.
    /// </summary>
    public class CalendarController : Controller
    {
        private readonly GraphServiceClient graphClient;
        private readonly ILogger<HomeController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarController"/> class.
        /// </summary>
        /// <param name="graphClient">An authenticated <see cref="GraphServiceClient"/>.</param>
        /// <param name="logger">An <see cref="ILogger"/> to use for logging.</param>
        public CalendarController(
            GraphServiceClient graphClient,
            ILogger<HomeController> logger)
        {
            this.graphClient = graphClient;
            this.logger = logger;
        }

        /// <summary>
        /// Loads the default view.
        /// </summary>
        /// <returns>A view of the current week's calendar.</returns>
        [AuthorizeForScopes(Scopes =
            ["Calendars.Read"])]
        public async Task<IActionResult> Index()
        {
            try
            {
                var userTimeZone = TZConvert.GetTimeZoneInfo(
                    User.GetUserGraphTimeZone() ?? "UTC");
                var startOfWeekUtc = GetUtcStartOfWeekInTimeZone(
                    DateTime.Today, userTimeZone);

                var events = await GetUserWeekCalendar(startOfWeekUtc);

                // Convert UTC start of week to user's time zone for
                // proper display
                var startOfWeekInTz = TimeZoneInfo.ConvertTimeFromUtc(startOfWeekUtc, userTimeZone);
                var model = new CalendarViewModel(startOfWeekInTz, events);

                return View(model);
            }
            catch (ServiceException ex)
            {
                if (ex.InnerException is MicrosoftIdentityWebChallengeUserException)
                {
                    throw;
                }

                return View(new CalendarViewModel())
                    .WithError("Error getting calendar view", ex.Message);
            }
        }

        /// <summary>
        /// Loads the new event form.
        /// </summary>
        /// <returns>The form.</returns>
        [AuthorizeForScopes(Scopes =
            ["Calendars.ReadWrite"])]
        public IActionResult New()
        {
            return View();
        }

        /// <summary>
        /// Accepts the data from the new event form when submitted and creates the new event.
        /// </summary>
        /// <param name="newEvent">The <see cref="NewEvent"/> instance with values from the form.</param>
        /// <returns>A redirect to the calendar view with the results.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeForScopes(Scopes =
            ["Calendars.ReadWrite"])]
        public async Task<IActionResult> New([Bind("Subject,Attendees,Start,End,Body")] NewEvent newEvent)
        {
            var timeZone = User.GetUserGraphTimeZone();

            // Create a Graph event with the required fields
            var graphEvent = new Event
            {
                Subject = newEvent.Subject,
                Start = new DateTimeTimeZone
                {
                    DateTime = newEvent.Start.ToString("o"),

                    // Use the user's time zone
                    TimeZone = timeZone,
                },
                End = new DateTimeTimeZone
                {
                    DateTime = newEvent.End.ToString("o"),
                    TimeZone = timeZone,
                },
            };

            // Add body if present
            if (!string.IsNullOrEmpty(newEvent.Body))
            {
                graphEvent.Body = new ItemBody
                {
                    ContentType = BodyType.Text,
                    Content = newEvent.Body,
                };
            }

            // Add attendees if present
            if (!string.IsNullOrEmpty(newEvent.Attendees))
            {
                var attendees =
                    newEvent.Attendees.Split(';', StringSplitOptions.RemoveEmptyEntries);

                if (attendees.Length > 0)
                {
                    var attendeeList = new List<Attendee>();
                    foreach (var attendee in attendees)
                    {
                        attendeeList.Add(new Attendee
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = attendee,
                            },
                            Type = AttendeeType.Required,
                        });
                    }

                    graphEvent.Attendees = attendeeList;
                }
            }

            try
            {
                // Add the event
                await graphClient.Me.Events
                    .PostAsync(graphEvent);

                // Redirect to the calendar view with a success message
                return RedirectToAction("Index").WithSuccess("Event created");
            }
            catch (ODataError ex)
            {
                // Redirect to the calendar view with an error message
                return RedirectToAction("Index")
                    .WithError("Error creating event", ex.Error?.Message);
            }
        }

        private static DateTime GetUtcStartOfWeekInTimeZone(DateTime today, TimeZoneInfo timeZone)
        {
            // Assumes Sunday as first day of week
            int diff = DayOfWeek.Sunday - today.DayOfWeek;

            // create date as unspecified kind
            var unspecifiedStart = DateTime.SpecifyKind(today.AddDays(diff), DateTimeKind.Unspecified);

            // convert to UTC
            return TimeZoneInfo.ConvertTimeToUtc(unspecifiedStart, timeZone);
        }

        private async Task<IList<Event>> GetUserWeekCalendar(DateTime startOfWeekUtc)
        {
            // Configure a calendar view for the current week
            var endOfWeekUtc = startOfWeekUtc.AddDays(7);

            var events = await graphClient.Me
                .CalendarView
                .GetAsync(config =>
                {
                    // Send user time zone in request so date/time in
                    // response will be in preferred time zone
                    config.Headers.Add("Prefer", $"outlook.timezone=\"{User.GetUserGraphTimeZone()}\"");

                    // Configure a calendar view for the current week
                    config.QueryParameters.StartDateTime = startOfWeekUtc.ToString("o");
                    config.QueryParameters.EndDateTime = endOfWeekUtc.ToString("o");

                    // Get max 50 per request
                    config.QueryParameters.Top = 50;

                    // Only return fields app will use
                    config.QueryParameters.Select =
                        ["subject", "organizer", "start", "end"];

                    // Order results chronologically
                    config.QueryParameters.Orderby =
                        ["start/dateTime"];
                });

            IList<Event> allEvents;

            // Handle case where there are more than 50
            if (!string.IsNullOrEmpty(events?.OdataNextLink))
            {
                allEvents = new List<Event>();

                // Create a page iterator to iterate over subsequent pages
                // of results. Build a list from the results
                var pageIterator = PageIterator<Event, EventCollectionResponse>.CreatePageIterator(
                    graphClient,
                    events,
                    (e) =>
                    {
                        allEvents.Add(e);
                        return true;
                    },
                    (req) =>
                    {
                        req.Headers.Add("Prefer", $"outlook.timezone=\"{User.GetUserGraphTimeZone()}\"");
                        return req;
                    });
                await pageIterator.IterateAsync();
            }
            else
            {
                // If only one page, just use the result
                allEvents = events?.Value ?? new List<Event>();
            }

            return allEvents;
        }
    }
}
