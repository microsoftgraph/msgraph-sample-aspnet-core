// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace GraphTutorial.Models
{
    /// <summary>
    /// Represents a day in the view.
    /// </summary>
    public class DailyViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyViewModel"/> class.
        /// </summary>
        /// <param name="day">The day this view represents.</param>
        /// <param name="events">The list of events for this day.</param>
        public DailyViewModel(DateTime day, IEnumerable<CalendarViewEvent> events)
        {
            Day = day;
            Events = events;
        }

        /// <summary>
        /// Gets the day this view represents.
        /// </summary>
        public DateTime Day { get; private set; }

        /// <summary>
        /// Gets the events for this day.
        /// </summary>
        public IEnumerable<CalendarViewEvent> Events { get; private set; }
    }
}
