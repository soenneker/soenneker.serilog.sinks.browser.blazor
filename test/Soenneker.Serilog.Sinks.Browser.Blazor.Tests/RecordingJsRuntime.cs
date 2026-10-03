using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Tests;

internal sealed class RecordingJsRuntime : IJSRuntime
{
    internal List<object?[]> Calls { get; } = [];
    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => InvokeAsync<TValue>(identifier, default, args);
    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
    {
        Calls.Add(args!);
        return ValueTask.FromResult(default(TValue)!);
    }
}
