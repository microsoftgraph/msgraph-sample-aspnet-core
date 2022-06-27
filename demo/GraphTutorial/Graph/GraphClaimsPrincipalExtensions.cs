// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Graph;
using System.Security.Claims;

public static class GraphClaimTypes {
    public const string DisplayName ="graph_name";
    public const string Email = "graph_email";
    public const string Photo = "graph_photo";
    public const string TimeZone = "graph_timezone";
    public const string TimeFormat = "graph_timeformat";
}

// Helper methods to access Graph user data stored in
// the claims principal
public static class GraphClaimsPrincipalExtensions
{
    public static string GetUserGraphDisplayName(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(GraphClaimTypes.DisplayName);
    }

    public static string GetUserGraphEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(GraphClaimTypes.Email);
    }

    public static string GetUserGraphPhoto(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(GraphClaimTypes.Photo);
    }

    public static string GetUserGraphTimeZone(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(GraphClaimTypes.TimeZone);
    }

    public static string GetUserGraphTimeFormat(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(GraphClaimTypes.TimeFormat);
    }

    public static void AddUserGraphInfo(this ClaimsPrincipal claimsPrincipal, User user)
    {
        var identity = claimsPrincipal.Identity as ClaimsIdentity;
        if (identity == null) throw new Exception("bah");

        identity.AddClaim(
            new Claim(GraphClaimTypes.DisplayName, user.DisplayName));
        identity.AddClaim(
            new Claim(GraphClaimTypes.Email,
                user.Mail ?? user.UserPrincipalName));
        // If not set, default to UTC
        var userTimeZone = string.IsNullOrEmpty(user.MailboxSettings.TimeZone) ? "UTC" :
            user.MailboxSettings.TimeZone;
        identity.AddClaim(
            new Claim(GraphClaimTypes.TimeZone,
                userTimeZone));
        // If not set, default to HH:mm
        var userTimeFormat = string.IsNullOrEmpty(user.MailboxSettings.TimeFormat) ? "HH:mm" :
            user.MailboxSettings.TimeFormat;
        identity.AddClaim(
            new Claim(GraphClaimTypes.TimeFormat, userTimeFormat));
    }

    public static void AddUserGraphPhoto(this ClaimsPrincipal claimsPrincipal, Stream? photoStream)
    {
        var identity = claimsPrincipal.Identity as ClaimsIdentity;
        if (identity == null) throw new Exception("bah");

        if (photoStream == null)
        {
            // Add the default profile photo
            identity.AddClaim(
                new Claim(GraphClaimTypes.Photo, "/img/no-profile-photo.png"));
            return;
        }

        // Copy the photo stream to a memory stream
        // to get the bytes out of it
        var memoryStream = new MemoryStream();
        photoStream.CopyTo(memoryStream);
        var photoBytes = memoryStream.ToArray();

        // Generate a date URI for the photo
        var photoUrl = $"data:image/png;base64,{Convert.ToBase64String(photoBytes)}";

        identity.AddClaim(
            new Claim(GraphClaimTypes.Photo, photoUrl));
    }
}
