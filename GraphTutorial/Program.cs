// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.Net;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Kiota.Abstractions.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Configure authentication
builder.Services

    // Use OpenId authentication
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)

    // Specify this is a web app and needs auth code flow
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureAd", options);

        // This causes the signin to prompt the user for which
        // account to use - useful when there are multiple accounts signed
        // into the browser
        options.Prompt = "select_account";

        options.Events.OnTokenValidated = async context =>
        {
            var tokenAcquisition = context.HttpContext.RequestServices
                .GetRequiredService<ITokenAcquisition>();

            var graphClient = new GraphServiceClient(
                new BaseBearerTokenAuthenticationProvider(
                    new TokenAcquisitionTokenProvider(
                        tokenAcquisition,
                        GraphConstants.Scopes,
                        context.Principal)));

            // Get user information from Graph
            var user = await graphClient.Me
                .GetAsync(config =>
                {
                    config.QueryParameters.Select =
                        ["displayName", "mail", "mailboxSettings", "userPrincipalName"];
                });

            context.Principal?.AddUserGraphInfo(user);

            // Get the user's photo
            // If the user doesn't have a photo, this throws
            try
            {
                var photo = await graphClient.Me
                    .Photos["48x48"]
                    .Content
                    .GetAsync();

                context.Principal?.AddUserGraphPhoto(photo);
            }
            catch (ServiceException ex)
            {
                if (ex.IsMatch("ErrorItemNotFound") ||
                    ex.IsMatch("ImageNotFound") ||
                    ex.IsMatch("ConsumerPhotoIsNotSupported"))
                {
                    context.Principal?.AddUserGraphPhoto(null);
                }
                else
                {
                    throw;
                }
            }
        };

        options.Events.OnAuthenticationFailed = context =>
        {
            var error = WebUtility.UrlEncode(context.Exception.Message);
            context.Response
                .Redirect($"/Home/ErrorWithMessage?message=Authentication+error&debug={error}");
            context.HandleResponse();

            return Task.FromResult(0);
        };

        options.Events.OnRemoteFailure = context =>
        {
            if (context.Failure is OpenIdConnectProtocolException)
            {
                var error = WebUtility.UrlEncode(context.Failure.Message);
                context.Response
                    .Redirect($"/Home/ErrorWithMessage?message=Sign+in+error&debug={error}");
                context.HandleResponse();
            }

            return Task.FromResult(0);
        };
    })

    // Add ability to call web API (Graph)
    // and get access tokens
    .EnableTokenAcquisitionToCallDownstreamApi(
        options =>
        {
            builder.Configuration.Bind("AzureAd", options);
        },
        GraphConstants.Scopes)

    // Add a GraphServiceClient via dependency injection
    .AddMicrosoftGraph(options =>
    {
        options.Scopes = GraphConstants.Scopes;
    })

    // Use in-memory token cache
    // See https://github.com/AzureAD/microsoft-identity-web/wiki/token-cache-serialization
    .AddInMemoryTokenCaches();

// Require authentication
builder.Services
    .AddControllersWithViews(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    })

    // Add the Microsoft Identity UI pages for sign in/out
    .AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
