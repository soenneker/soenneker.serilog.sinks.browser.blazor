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

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Demo;

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

            AddBlazorConsoleLogger(host);

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

    private static IServiceCollection ConfigureLogging(IServiceCollection services)
    {
        SelfLog.Enable(m => Console.Error.WriteLine(m));

        services.AddLogging(builder =>
        {
            builder.ClearProviders();

            builder.AddSerilog(dispose: true);
        });

        return services;
    }

    private static WebAssemblyHost AddBlazorConsoleLogger(WebAssemblyHost host)
    {
        var jsRuntime = host.Services.GetRequiredService<IJSRuntime>();

        const LogEventLevel logEventLevel = LogEventLevel.Verbose;

        var loggerConfig = new LoggerConfiguration();

        loggerConfig.WriteTo.BlazorConsole(jsRuntime: jsRuntime, restrictedToMinimumLevel: logEventLevel);

        Log.Logger = loggerConfig.CreateLogger();

        return host;
    }
}