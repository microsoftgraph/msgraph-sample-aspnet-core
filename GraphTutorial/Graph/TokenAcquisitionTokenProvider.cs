// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Security.Claims;
using Microsoft.Identity.Web;
using Microsoft.Kiota.Abstractions.Authentication;

public class TokenAcquisitionTokenProvider : IAccessTokenProvider
{
    private string[] validHosts =
    {
        "graph.microsoft.com",
        "graph.microsoft.us",
        "dod-graph.microsoft.us",
        "graph.microsoft.de",
        "microsoftgraph.chinacloudapi.cn"
    };

    private ITokenAcquisition _tokenAcquisition;
    private string[] _scopes;
    private ClaimsPrincipal _user;

    public TokenAcquisitionTokenProvider(
        ITokenAcquisition tokenAcquisition,
        string[] scopes,
        ClaimsPrincipal? user)
    {
        _tokenAcquisition = tokenAcquisition;
        _scopes = scopes;
        _user = user ?? throw new Exception("User claims principal is required.");
    }

    public AllowedHostsValidator AllowedHostsValidator => new AllowedHostsValidator(validHosts);

    public async Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
    {
        if (!this.AllowedHostsValidator.IsUrlHostValid(uri))
        {
            return string.Empty;
        }

        if (uri.Scheme != "https")
        {
            throw new Exception("URL must use https.");
        }

        return await this._tokenAcquisition.GetAccessTokenForUserAsync(
            this._scopes, user: this._user);
    }
}
