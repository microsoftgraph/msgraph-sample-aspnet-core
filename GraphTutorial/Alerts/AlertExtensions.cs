// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Adds alert methods to instances of the <see cref="IActionResult"/> interface.
/// </summary>
public static class AlertExtensions
{
    /// <summary>
    /// Adds an error alert to the result.
    /// </summary>
    /// <param name="result">The <see cref="IActionResult"/> to add the alert to.</param>
    /// <param name="message">The error message.</param>
    /// <param name="debugInfo">Additional debug info.</param>
    /// <returns>The <see cref="IActionResult"/>.</returns>
    public static IActionResult WithError(
        this IActionResult result,
        string message,
        string? debugInfo = null)
    {
        return Alert(result, "danger", message, debugInfo);
    }

    /// <summary>
    /// Adds a success alert to the result.
    /// </summary>
    /// <param name="result">The <see cref="IActionResult"/> to add the alert to.</param>
    /// <param name="message">The success message.</param>
    /// <param name="debugInfo">Additional debug info.</param>
    /// <returns>The <see cref="IActionResult"/>.</returns>
    public static IActionResult WithSuccess(
        this IActionResult result,
        string message,
        string? debugInfo = null)
{
        return Alert(result, "success", message, debugInfo);
    }

    /// <summary>
    /// Adds an informational alert to the result.
    /// </summary>
    /// <param name="result">The <see cref="IActionResult"/> to add the alert to.</param>
    /// <param name="message">The informational message.</param>
    /// <param name="debugInfo">Additional debug info.</param>
    /// <returns>The <see cref="IActionResult"/>.</returns>
    public static IActionResult WithInfo(
        this IActionResult result,
        string message,
        string? debugInfo = null)
    {
        return Alert(result, "info", message, debugInfo);
    }

    private static IActionResult Alert(
        IActionResult result,
        string type,
        string message,
        string? debugInfo)
    {
        return new WithAlertResult(result, type, message, debugInfo);
    }
}
