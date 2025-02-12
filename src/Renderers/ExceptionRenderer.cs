using Serilog.Events;
using Soenneker.Serilog.Sinks.Browser.Blazor.Renderers.Base;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Renderers;

internal sealed class ExceptionRenderer : BaseRenderer
{
    internal override void Render(LogEvent logEvent, TokenEmitter emitToken)
    {
        if (logEvent.Exception is not null)
            emitToken(logEvent.Exception.ToString());
    }
}