// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace GraphTutorial.Models;

/// <summary>
/// Represents the view model for an error.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Gets or sets the request ID for the request that generated the error.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether the request ID should be shown.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
