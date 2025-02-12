using Serilog.Events;
using Soenneker.Extensions.Char;
using Soenneker.Extensions.String;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;

internal static class LogEventLevelExtension
{
    internal static string ToLevelPrefix(this LogEventLevel level, string? format = null)
    {
        if (format.IsNullOrEmpty() || format.Length < 2 || format.Length > 3)
            return level.ToString().ToUpperInvariantFast();

        // Extract width
        if (!format[0].IsDigit() || (format.Length is 3 && !format[1].IsDigit()))
            return level.ToString().ToUpperInvariantFast();

        int width = format.Length is 2 ? format[0] - '0' : (format[0] - '0') * 10 + (format[1] - '0');

        if (width < 1) 
            return "";

        if (width > 4) 
            return level.ToString().ToUpperInvariantFast().Substring(0, width);

        string baseName = level switch
        {
            LogEventLevel.Verbose => "VERB",
            LogEventLevel.Debug => "DBUG",
            LogEventLevel.Information => "INFO",
            LogEventLevel.Warning => "WARN",
            LogEventLevel.Error => "EROR", // Prevents "ERROR" from exceeding 4 chars
            LogEventLevel.Fatal => "FATL",
            _ => level.ToString().ToUpperInvariantFast()
        };

        return baseName[..width];
    }
}