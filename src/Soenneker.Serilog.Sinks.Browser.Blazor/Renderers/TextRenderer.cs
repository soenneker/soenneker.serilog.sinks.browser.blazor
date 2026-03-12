using Serilog.Events;
using Soenneker.Serilog.Sinks.Browser.Blazor.Renderers.Base;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Renderers;

internal sealed class TextRenderer : BaseRenderer
{
    private readonly string _text;

    internal TextRenderer(string text)
    {
        _text = text;
    }

    internal override void Render(LogEvent logEvent, TokenEmitter emitToken)
    {
        emitToken(_text);
    }
}