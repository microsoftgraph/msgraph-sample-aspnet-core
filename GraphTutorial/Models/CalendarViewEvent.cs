// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Graph.Models;

namespace GraphTutorial.Models
{
    public class CalendarViewEvent
    {
        public string Subject { get; private set; }
        public string Organizer { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public CalendarViewEvent(Event graphEvent)
        {
            Subject = graphEvent.Subject ?? string.Empty;
            Organizer = graphEvent.Organizer?.EmailAddress?.Name ?? string.Empty;
            Start = DateTime.Parse(graphEvent.Start?.DateTime ??
                throw new Exception("Missing start for event."));
            End = DateTime.Parse(graphEvent.End?.DateTime ??
                throw new Exception("Missing end for event."));
        }
    }
}
