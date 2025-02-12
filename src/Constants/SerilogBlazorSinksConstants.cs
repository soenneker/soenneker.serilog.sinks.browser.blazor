namespace Soenneker.Serilog.Sinks.Browser.Blazor.Constants;

internal static class SerilogBlazorSinksConstants
{
    private const string _serilogToken =
        "%cserilog{_}color:white;background:#007acc;border-radius:3px;padding:1px 2px;font-weight:600;";

    internal const string DefaultConsoleOutputTemplate = _serilogToken + "{Message}{NewLine}{Exception}";
}