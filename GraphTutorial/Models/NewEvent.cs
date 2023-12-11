// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace GraphTutorial.Models
{
    /// <summary>
    /// Represents the data collected from the new event form.
    /// </summary>
    public class NewEvent
    {
        /// <summary>
        /// Gets or sets the subject of the event.
        /// </summary>
        [Required]
        public string? Subject { get; set; }

        /// <summary>
        /// Gets or sets the start date and time of the event.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the end date and time of the event.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Gets or sets the body of the event.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string? Body { get; set; }

        /// <summary>
        /// Gets or sets the attendees of the event.
        /// </summary>
        [RegularExpression(
            @"((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([;])*)*",
            ErrorMessage="Please enter one or more email addresses separated by a semi-colon (;)")]
        public string? Attendees { get; set; }
    }
}
