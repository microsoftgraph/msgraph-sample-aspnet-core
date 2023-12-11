// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.Graph.Models;

namespace GraphTutorial.Models
{
    /// <summary>
    /// Represents an event in the view.
    /// </summary>
    public class CalendarViewEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarViewEvent"/> class.
        /// </summary>
        /// <param name="graphEvent">The <see cref="Event"/> object from Microsoft Graph.</param>
        /// <exception cref="Exception">Thrown if start or end are invalid.</exception>
        public CalendarViewEvent(Event graphEvent)
        {
            Subject = graphEvent.Subject ?? string.Empty;
            Organizer = graphEvent.Organizer?.EmailAddress?.Name ?? string.Empty;
            Start = DateTime.Parse(graphEvent.Start?.DateTime ??
                throw new Exception("Missing start for event."));
            End = DateTime.Parse(graphEvent.End?.DateTime ??
                throw new Exception("Missing end for event."));
        }

        /// <summary>
        /// Gets the event's subject.
        /// </summary>
        public string Subject { get; private set; }

        /// <summary>
        /// Gets the event's organizer.
        /// </summary>
        public string Organizer { get; private set; }

        /// <summary>
        /// Gets the event's start date and time.
        /// </summary>
        public DateTime Start { get; private set; }

        /// <summary>
        /// Gets the event's end date and time.
        /// </summary>
        public DateTime End { get; private set; }
    }
}
