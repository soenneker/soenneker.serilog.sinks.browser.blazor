using System;
using Microsoft.JSInterop;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Soenneker.Serilog.Sinks.Browser.Blazor.Constants;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Registrars;

/// <summary>
/// A Serilog sink for logging within the Blazor client-side environment
/// </summary>
public static class SerilogBlazorSinkRegistrar
{
    /// <summary>
    /// Configures a logging sink that writes log events to the browser console in a Blazor application.
    /// </summary>
    /// <param name="sinkConfiguration">The logger sink configuration.</param>
    /// <param name="jsRuntime">An instance of <see cref="IJSRuntime"/> used to interact with the browser console.</param>
    /// <param name="restrictedToMinimumLevel">
    /// The minimum log level for events to be recorded by the sink.
    /// If <paramref name="levelSwitch"/> is specified, this value is ignored.
    /// </param>
    /// <param name="outputTemplate">
    /// A message template that defines the format of log messages.
    /// </param>
    /// <param name="formatProvider">
    /// An optional provider for culture-specific formatting.
    /// If <c>null</c>, the default formatting is used.
    /// </param>
    /// <param name="levelSwitch">
    /// An optional <see cref="LoggingLevelSwitch"/> that allows the minimum log level
    /// to be dynamically adjusted at runtime.
    /// </param>
    /// <returns>A <see cref="LoggerConfiguration"/> object that supports method chaining.</returns>
    public static LoggerConfiguration BlazorConsole(
        this LoggerSinkConfiguration sinkConfiguration,
        IJSRuntime jsRuntime,
        LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
        string outputTemplate = SerilogBlazorSinksConstants.DefaultConsoleOutputTemplate,
        IFormatProvider? formatProvider = null,
        LoggingLevelSwitch? levelSwitch = null)
    {
        var formatter = new OutputFormatter(outputTemplate, formatProvider);

        return sinkConfiguration.Sink(new BlazorConsoleSink(jsRuntime, formatter), restrictedToMinimumLevel, levelSwitch);
    }
}