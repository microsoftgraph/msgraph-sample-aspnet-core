// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

// <DailyViewModelSnippet>
using System;
using System.Collections.Generic;

namespace GraphTutorial.Models
{
    public class DailyViewModel
    {
        public DateTime Day { get; private set; }
        public IEnumerable<CalendarViewEvent> Events { get; private set; }

        public DailyViewModel(DateTime day, IEnumerable<CalendarViewEvent> events)
        {
            Day = day;
            Events = events;
        }
    }
}
// </DailyViewModelSnippet>
