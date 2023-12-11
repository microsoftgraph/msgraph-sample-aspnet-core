// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Security.Claims;
using Microsoft.Identity.Web;
using Microsoft.Kiota.Abstractions.Authentication;

/// <summary>
/// Access token provider used during login.
/// </summary>
public class TokenAcquisitionTokenProvider : IAccessTokenProvider
{
    private readonly string[] validHosts =
    {
        "graph.microsoft.com",
        "graph.microsoft.us",
        "dod-graph.microsoft.us",
        "graph.microsoft.de",
        "microsoftgraph.chinacloudapi.cn",
    };

    private readonly ITokenAcquisition tokenAcquisition;
    private readonly string[] scopes;
    private readonly ClaimsPrincipal user;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenAcquisitionTokenProvider"/> class.
    /// </summary>
    /// <param name="tokenAcquisition">The <see cref="ITokenAcquisition"/> service to use to acquire tokens.</param>
    /// <param name="scopes">The permission scopes to use for the token request.</param>
    /// <param name="user">The user's <see cref="ClaimsPrincipal"/>.</param>
    /// <exception cref="Exception">Thrown if the <see cref="ClaimsPrincipal"/> is null.</exception>
    public TokenAcquisitionTokenProvider(
        ITokenAcquisition tokenAcquisition,
        string[] scopes,
        ClaimsPrincipal? user)
    {
        this.tokenAcquisition = tokenAcquisition;
        this.scopes = scopes;
        this.user = user ?? throw new Exception("User claims principal is required.");
    }

    /// <summary>
    /// Gets the allowed host validator.
    /// </summary>
    public AllowedHostsValidator AllowedHostsValidator => new AllowedHostsValidator(validHosts);

    /// <summary>
    /// Gets an access token for the user.
    /// </summary>
    /// <param name="uri">The API URI of the request that the token will be added to.</param>
    /// <param name="additionalAuthenticationContext">Additional name value pairs to add to the token request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The access token.</returns>
    /// <exception cref="Exception">Thrown if the URI is not HTTPS.</exception>
    public async Task<string> GetAuthorizationTokenAsync(
        Uri uri,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        if (!AllowedHostsValidator.IsUrlHostValid(uri))
        {
            return string.Empty;
        }

        if (uri.Scheme != "https")
        {
            throw new Exception("URL must use https.");
        }

        return await tokenAcquisition.GetAccessTokenForUserAsync(
            scopes, user: user);
    }
}
