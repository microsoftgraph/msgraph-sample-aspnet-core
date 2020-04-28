// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using System.Security.Claims;

namespace GraphTutorial
{
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
            return "Adele Vance";
            //return claimsPrincipal.FindFirstValue("graph_name");
        }

        public static string GetUserGraphEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return "adelev@contoso.com";
            // return claimsPrincipal.FindFirstValue("graph_email");
        }

        public static string GetUserGraphPhoto(this ClaimsPrincipal claimsPrincipal)
        {
            return "/img/no-profile-photo.png";
        }

        public static string GetUserGraphTimeZone(this ClaimsPrincipal claimsPrincipal)
        {
            return "";
        }

        public static string GetUserGraphTimeFormat(this ClaimsPrincipal claimsPrincipal)
        {
            return "";
        }
    }
}
