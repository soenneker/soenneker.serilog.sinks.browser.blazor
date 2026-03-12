using Serilog.Events;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Renderers.Base;

internal abstract class BaseRenderer
{
    internal delegate void TokenEmitter(object? token);

    internal abstract void Render(LogEvent logEvent, TokenEmitter emitToken);
}