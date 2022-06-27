// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

// WithAlertResult adds temporary error/info/success
// messages to the result of a controller action.
// This data is read and displayed by the _AlertPartial view
public class WithAlertResult : IActionResult
{
    public IActionResult Result { get; }
    public string Type { get; }
    public string Message { get; }
    public string DebugInfo { get; }

    public WithAlertResult(IActionResult result,
                            string type,
                            string message,
                            string? debugInfo)
    {
        Result = result;
        Type = type;
        Message = message;
        DebugInfo = debugInfo ?? string.Empty;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var factory = context.HttpContext.RequestServices
            .GetService<ITempDataDictionaryFactory>();

        if (factory == null) return;

        var tempData = factory.GetTempData(context.HttpContext);

        tempData["_alertType"] = Type;
        tempData["_alertMessage"] = Message;
        tempData["_alertDebugInfo"] = DebugInfo;

        await Result.ExecuteResultAsync(context);
    }
}
