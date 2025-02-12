using System;
using Serilog.Events;
using Serilog.Parsing;
using Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;
using Soenneker.Serilog.Sinks.Browser.Blazor.Renderers.Base;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Renderers;

internal sealed class NewLineRenderer : BaseRenderer
{
    private readonly Alignment? _alignment;

    internal NewLineRenderer(Alignment? alignment)
    {
        _alignment = alignment;
    }

    internal override void Render(LogEvent logEvent, TokenEmitter emitToken)
    {
        if (_alignment is not null)
            emitToken(Environment.NewLine.Pad(_alignment.Value.Widen(Environment.NewLine.Length)));
        else
            emitToken(Environment.NewLine);
    }
}