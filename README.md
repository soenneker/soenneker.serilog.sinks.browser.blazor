﻿[![](https://img.shields.io/nuget/v/soenneker.serilog.sinks.browser.blazor.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.serilog.sinks.browser.blazor/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.serilog.sinks.browser.blazor/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.serilog.sinks.browser.blazor/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.serilog.sinks.browser.blazor.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.serilog.sinks.browser.blazor/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Serilog.Sinks.Browser.Blazor
### A Serilog sink for logging within the Blazor client-side environment

An example demo app has been added to the solution.

## 🛠 Installation

1. **Install Required NuGet Packages**

   ```sh
   dotnet add package Serilog.Extensions.Logging
   dotnet add package Soenneker.Serilog.Sinks.Browser.Blazor
   ```

2. **Configure Logging in `Program.cs`**

An example:

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Soenneker.Serilog.Sinks.Browser.Blazor.Registrars;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            ConfigureLogging(builder.Services);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            WebAssemblyHost host = builder.Build();

            SetGlobalLogger(host);

            await host.RunAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, "Stopped program because of exception");
            throw;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static void ConfigureLogging(IServiceCollection services)
    {
        SelfLog.Enable(m => Console.Error.WriteLine(m));

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(dispose: true);
        });
    }

    private static void SetGlobalLogger(WebAssemblyHost host)
    {
        var jsRuntime = (IJSRuntime)host.Services.GetService(typeof(IJSRuntime))!;

        var loggerConfig = new LoggerConfiguration();

        loggerConfig.WriteTo.BlazorConsole(jsRuntime: jsRuntime);

        Log.Logger = loggerConfig.CreateLogger();
    }
}
```

**How It Works**
   - **`ConfigureLogging(IServiceCollection services)`**  
     - Enables Serilog's self-logging to capture any internal errors.
     - Registers Serilog as the primary logging provider.
   - **`SetGlobalLogger(WebAssemblyHost host)`**  
     - Initializes the **BlazorConsole sink** to log messages directly in the browser's developer console.

---

## 🛠 Usage

Once you have installed and configured Serilog with the **BlazorConsole sink**, you can start logging messages in your Blazor components.

### Injecting the Logger

In your **Blazor component (`.razor` file)**, inject the `ILogger<T>` service:

```razor
@page "/"
@using Microsoft.Extensions.Logging

@inject ILogger<Index> Logger

<button @onclick="Click">Click</button>

@code {
    public void Click()
    {
        Logger.LogInformation("Testing information log");
    }
}
```
