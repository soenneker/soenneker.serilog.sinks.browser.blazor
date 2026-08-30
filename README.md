[![](https://img.shields.io/nuget/v/soenneker.serilog.sinks.browser.blazor.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.serilog.sinks.browser.blazor/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.serilog.sinks.browser.blazor/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.serilog.sinks.browser.blazor/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.serilog.sinks.browser.blazor.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker/soenneker.serilog.sinks.browser.blazor/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.serilog.sinks.browser.blazor/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.serilog.sinks.browser.blazor/actions/workflows/codeql.yml)

# Soenneker.Serilog.Sinks.Browser.Blazor

A Serilog sink that writes structured log events to the browser developer console through Blazor's `IJSRuntime`.

![Browser console output](https://github.com/user-attachments/assets/f9fa6f2d-cf9e-45f5-9f3a-966d3e9c5e6a)

## Installation

```bash
dotnet add package Soenneker.Serilog.Sinks.Browser.Blazor
```

## Configure a Blazor WebAssembly app

`IJSRuntime` is available after the host is built. Resolve it, create the Serilog logger, and then connect Serilog to Microsoft logging:

```csharp
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Serilog;
using Serilog.Events;
using Soenneker.Serilog.Sinks.Browser.Blazor.Registrars;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddSerilog(dispose: false);
});

WebAssemblyHost host = builder.Build();
IJSRuntime jsRuntime = host.Services.GetRequiredService<IJSRuntime>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.BlazorConsole(
        jsRuntime,
        restrictedToMinimumLevel: LogEventLevel.Information)
    .CreateLogger();

try
{
    await host.RunAsync();
}
finally
{
    await Log.CloseAndFlushAsync();
}
```

With `dispose: false`, the global Serilog logger owns its sink and `CloseAndFlushAsync` performs teardown.

## Log from a component

Microsoft `ILogger<T>` messages flow through the configured Serilog provider:

```razor
@inject ILogger<Index> Logger

<button @onclick="LogClick">Log</button>

@code {
    private void LogClick()
    {
        Logger.LogInformation("Clicked at {Timestamp}", DateTimeOffset.UtcNow);
    }
}
```

Serilog levels map to the corresponding browser methods: fatal and error use `console.error`, warning uses `console.warn`, information uses `console.info`, debug uses `console.debug`, and verbose uses `console.trace`.

## Formatting and failures

Customize the output template and culture when configuring the sink:

```csharp
.WriteTo.BlazorConsole(
    jsRuntime,
    outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message}{NewLine}{Exception}",
    formatProvider: CultureInfo.InvariantCulture)
```

The default template adds a styled `serilog` label followed by the message, newline, and exception. Structured values are passed to the browser console as arguments rather than flattened into one string.

Writes are asynchronous and fire-and-forget because Serilog's sink contract is synchronous. JS interop failures are reported through `Serilog.Debugging.SelfLog`; enable it while diagnosing logging setup:

```csharp
SelfLog.Enable(message => Console.Error.WriteLine(message));
```

Everything sent to this sink is visible to the browser user. Do not log access tokens, credentials, personal data, or server-only details.
