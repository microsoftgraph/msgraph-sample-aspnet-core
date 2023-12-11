// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

/// <summary>
/// Contains constants used by Microsoft Graph-related code.
/// </summary>
public static class GraphConstants
{
    /// <summary>
    /// Defines the permission scopes used by the app.
    /// </summary>
    public static readonly string[] Scopes =
    {
        "User.Read",
        "MailboxSettings.Read",
        "Calendars.ReadWrite",
    };
}
