// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Graph.Models;

namespace GraphTutorial.Models
{
    /// <summary>
    /// Represents the view model for the calendar page.
    /// </summary>
    public class CalendarViewModel
    {
        private readonly DateTime startOfWeek;
        private readonly DateTime endOfWeek;
        private readonly List<CalendarViewEvent> events;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarViewModel"/> class.
        /// </summary>
        public CalendarViewModel()
        {
            startOfWeek = DateTime.MinValue;
            events = new List<CalendarViewEvent>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarViewModel"/> class.
        /// </summary>
        /// <param name="startOfWeek">The date of the start of this week.</param>
        /// <param name="events">The list of events for this week.</param>
        public CalendarViewModel(DateTime startOfWeek, IEnumerable<Event> events)
        {
            this.startOfWeek = startOfWeek;
            endOfWeek = startOfWeek.AddDays(7);
            this.events = new List<CalendarViewEvent>();

            if (events != null)
            {
              foreach (var item in events)
              {
                    this.events.Add(new CalendarViewEvent(item));
              }
            }
        }

        /// <summary>
        /// Gets the view model for Sunday.
        /// </summary>
        public DailyViewModel Sunday => new DailyViewModel(
                  startOfWeek,
                  GetEventsForDay(DayOfWeek.Sunday));

        /// <summary>
        /// Gets the view model for Monday.
        /// </summary>
        public DailyViewModel Monday => new DailyViewModel(
                  startOfWeek.AddDays(1),
                  GetEventsForDay(DayOfWeek.Monday));

        /// <summary>
        /// Gets the view model for Tuesday.
        /// </summary>
        public DailyViewModel Tuesday => new DailyViewModel(
                  startOfWeek.AddDays(2),
                  GetEventsForDay(DayOfWeek.Tuesday));

        /// <summary>
        /// Gets the view model for Wednesday.
        /// </summary>
        public DailyViewModel Wednesday => new DailyViewModel(
                  startOfWeek.AddDays(3),
                  GetEventsForDay(DayOfWeek.Wednesday));

        /// <summary>
        /// Gets the view model for Thursday.
        /// </summary>
        public DailyViewModel Thursday => new DailyViewModel(
                  startOfWeek.AddDays(4),
                  GetEventsForDay(DayOfWeek.Thursday));

        /// <summary>
        /// Gets the view model for Friday.
        /// </summary>
        public DailyViewModel Friday => new DailyViewModel(
                  startOfWeek.AddDays(5),
                  GetEventsForDay(DayOfWeek.Friday));

        /// <summary>
        /// Gets the view model for Saturday.
        /// </summary>
        public DailyViewModel Saturday => new DailyViewModel(
                  startOfWeek.AddDays(6),
                  GetEventsForDay(DayOfWeek.Saturday));

        /// <summary>
        /// Gets the string representation of the week.
        /// </summary>
        /// <returns>A string representing the start and end of the week.</returns>
        public string TimeSpan()
        {
            return $"{startOfWeek.ToString("MMMM d, yyyy")} - {startOfWeek.AddDays(6).ToString("MMMM d, yyyy")}";
        }

        private IEnumerable<CalendarViewEvent> GetEventsForDay(DayOfWeek day)
        {
            return events.Where(e => e.End > startOfWeek &&
                ((e.Start.DayOfWeek.Equals(day) && e.Start >= startOfWeek) ||
                 (e.End.DayOfWeek.Equals(day) && e.End < endOfWeek)));
        }
    }
}
