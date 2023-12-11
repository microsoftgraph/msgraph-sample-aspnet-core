// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Security.Claims;
using Microsoft.Graph.Models;

/// <summary>
/// Contains helper method to access Graph user data stored in the claims principal.
/// </summary>
public static class GraphClaimsPrincipalExtensions
{
    /// <summary>
    /// Gets the user's display name.
    /// </summary>
    /// <param name="claimsPrincipal">The <see cref="ClaimsPrincipal"/> that contains the information.</param>
    /// <returns>The user's display name.</returns>
    public static string? GetUserGraphDisplayName(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(GraphClaimTypes.DisplayName);
    }

    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    /// <param name="claimsPrincipal">The <see cref="ClaimsPrincipal"/> that contains the information.</param>
    /// <returns>The user's email address.</returns>
    public static string? GetUserGraphEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(GraphClaimTypes.Email);
    }

    /// <summary>
    /// Gets the user's profile photo.
    /// </summary>
    /// <param name="claimsPrincipal">The <see cref="ClaimsPrincipal"/> that contains the information.</param>
    /// <returns>The user's profile photo.</returns>
    public static string? GetUserGraphPhoto(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(GraphClaimTypes.Photo);
    }

    /// <summary>
    /// Gets the user's time zone.
    /// </summary>
    /// <param name="claimsPrincipal">The <see cref="ClaimsPrincipal"/> that contains the information.</param>
    /// <returns>The user's time zone.</returns>
    public static string? GetUserGraphTimeZone(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(GraphClaimTypes.TimeZone);
    }

    /// <summary>
    /// Gets the user's time format.
    /// </summary>
    /// <param name="claimsPrincipal">The <see cref="ClaimsPrincipal"/> that contains the information.</param>
    /// <returns>The user's time format.</returns>
    public static string? GetUserGraphTimeFormat(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(GraphClaimTypes.TimeFormat);
    }

    /// <summary>
    /// Adds user information from Microsoft Graph to the claims principal.
    /// </summary>
    /// <param name="claimsPrincipal">The <see cref="ClaimsPrincipal"/> to add information to.</param>
    /// <param name="user">The <see cref="User"/> returned from Microsoft Graph.</param>
    /// <exception cref="Exception">Thrown if the claims principal does not contain an identity.</exception>
    public static void AddUserGraphInfo(this ClaimsPrincipal claimsPrincipal, User? user)
    {
        if (user == null)
        {
            return;
        }

        if (claimsPrincipal.Identity is not ClaimsIdentity identity)
        {
            throw new Exception("Identity cannot be null");
        }

        identity.AddClaim(
            new Claim(GraphClaimTypes.DisplayName, user.DisplayName ?? string.Empty));
        identity.AddClaim(
            new Claim(
                GraphClaimTypes.Email,
                user.Mail ?? user.UserPrincipalName ?? string.Empty));

        // If not set, default to UTC
        identity.AddClaim(
            new Claim(
                GraphClaimTypes.TimeZone,
                user.MailboxSettings?.TimeZone ?? "UTC"));

        // If not set, default to HH:mm
        identity.AddClaim(
            new Claim(
                GraphClaimTypes.TimeFormat,
                user.MailboxSettings?.TimeFormat ?? "HH:mm"));
    }

    /// <summary>
    /// Adds the user's profile photo to the claims principal.
    /// </summary>
    /// <param name="claimsPrincipal">The <see cref="ClaimsPrincipal"/> to add information to.</param>
    /// <param name="photoStream">A <see cref="Stream"/> containing the user's photo.</param>
    /// <exception cref="Exception">Thrown if the claims principal does not contain an identity.</exception>
    public static void AddUserGraphPhoto(this ClaimsPrincipal claimsPrincipal, Stream? photoStream)
    {
        if (claimsPrincipal.Identity is not ClaimsIdentity identity)
        {
            throw new Exception("Identity cannot be null");
        }

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
