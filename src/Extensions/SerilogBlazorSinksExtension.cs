using Serilog.Events;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;

internal static class SerilogBlazorSinksExtension
{
    internal static string ToConsoleMethod(this LogEventLevel logLevel) =>
        logLevel switch
        {
            >= LogEventLevel.Error => "console.error",
            LogEventLevel.Warning => "console.warn",
            LogEventLevel.Information => "console.info",
            LogEventLevel.Debug => "console.debug",
            LogEventLevel.Verbose => "console.trace",
            _ => "console.log"
        };
}