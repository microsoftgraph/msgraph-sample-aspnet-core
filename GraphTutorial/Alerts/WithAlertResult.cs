// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

// WithAlertResult adds temporary error/info/success
// messages to the result of a controller action.
// This data is read and displayed by the _AlertPartial view

/// <summary>
/// Adds temporary error/info/success messages to the result of a controller action.
/// This data is read and displayed by the _AlertPartial view.
/// </summary>
public class WithAlertResult : IActionResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WithAlertResult"/> class.
    /// </summary>
    /// <param name="result">The <see cref="IActionResult"/> to add an alert to.</param>
    /// <param name="type">The type of alert.</param>
    /// <param name="message">The message to display in the alert.</param>
    /// <param name="debugInfo">Additional debug information to display in the alert.</param>
    public WithAlertResult(
        IActionResult result,
        string type,
        string message,
        string? debugInfo)
    {
        Result = result;
        Type = type;
        Message = message;
        DebugInfo = debugInfo ?? string.Empty;
    }

    /// <summary>
    /// Gets the <see cref="IActionResult"/>.
    /// </summary>
    public IActionResult Result { get; }

    /// <summary>
    /// Gets the type of alert.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the message for the alert.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the debug information for the alert.
    /// </summary>
    public string DebugInfo { get; }

    /// <inheritdoc/>
    public async Task ExecuteResultAsync(ActionContext context)
    {
        var factory = context.HttpContext.RequestServices
            .GetService<ITempDataDictionaryFactory>();

        if (factory == null)
        {
            return;
        }

        var tempData = factory.GetTempData(context.HttpContext);

        tempData["_alertType"] = Type;
        tempData["_alertMessage"] = Message;
        tempData["_alertDebugInfo"] = DebugInfo;

        await Result.ExecuteResultAsync(context);
    }
}
