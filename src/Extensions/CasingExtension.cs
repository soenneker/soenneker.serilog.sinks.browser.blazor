using Soenneker.Extensions.String;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;

internal static class CasingExtension
{
    /// <summary>
    /// Apply upper or lower casing to <paramref name="value"/> when <paramref name="format"/> is provided.
    /// Returns <paramref name="value"/> when no or invalid format provided.
    /// </summary>
    /// <param name="value">Provided string for formatting.</param>
    /// <param name="format">Format string.</param>
    /// <returns>The provided <paramref name="value"/> with formatting applied.</returns>
    internal static string Format(this string value, string? format = null) =>
        format switch
        {
            "u" => value.ToUpperInvariantFast(),
            "w" => value.ToLowerInvariantFast(),
            _ => value
        };
}