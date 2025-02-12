using Microsoft.JSInterop;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Soenneker.Extensions.ValueTask;
using Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;
using System;
using System.Threading.Tasks;

namespace Soenneker.Serilog.Sinks.Browser.Blazor;

/// <summary>
/// A Serilog sink for logging within the Blazor client-side environment
/// </summary>
internal class BlazorConsoleSink : ILogEventSink
{
    private readonly IJSRuntime _jSRuntime;
    private readonly OutputFormatter _formatter;

    internal BlazorConsoleSink(IJSRuntime runtime, OutputFormatter formatter)
    {
        _jSRuntime = runtime;
        _formatter = formatter;
    }

    public void Emit(LogEvent logEvent)
    {
        string outputStream = logEvent.Level.ToConsoleMethod();

        object?[] args = _formatter.Format(logEvent);

        FireAndForget(_jSRuntime.InvokeAsync<string>(outputStream, args));
    }

    private static async void FireAndForget(ValueTask<string> valueTask)
    {
        try
        {
            await valueTask.NoSync();
        }
        catch (Exception ex)
        {
            SelfLog.WriteLine("Failed to emit log event: {0}", ex);
        }
    }
}