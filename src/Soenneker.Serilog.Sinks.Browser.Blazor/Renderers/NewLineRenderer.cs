using System;
using Serilog.Events;
using Serilog.Parsing;
using Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;
using Soenneker.Serilog.Sinks.Browser.Blazor.Renderers.Base;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Renderers;

internal sealed class NewLineRenderer : BaseRenderer
{
    private readonly string _text;

    internal NewLineRenderer(Alignment? alignment)
    {
        _text = alignment is null ? Environment.NewLine : Environment.NewLine.Pad(alignment.Value.Widen(Environment.NewLine.Length));
    }

    internal override void Render(LogEvent logEvent, TokenEmitter emitToken)
    {
        emitToken(_text);
    }
}