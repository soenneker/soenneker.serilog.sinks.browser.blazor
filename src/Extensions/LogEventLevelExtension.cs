using Serilog.Events;
using Soenneker.Extensions.String;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;

internal static class LogEventLevelExtension
{
    internal static string ToLevelPrefix(this LogEventLevel level)
    {
        return level switch
        {
            LogEventLevel.Verbose => "VERB",
            LogEventLevel.Debug => "DBUG",
            LogEventLevel.Information => "INFO",
            LogEventLevel.Warning => "WARN",
            LogEventLevel.Error => "EROR", // Keeps it at 4 chars
            LogEventLevel.Fatal => "FATL",
            _ => level.ToString().ToUpperInvariantFast()
        };
    }
}