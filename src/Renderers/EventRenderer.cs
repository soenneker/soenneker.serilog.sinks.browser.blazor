using System;
using System.IO;
using Serilog.Events;
using Serilog.Parsing;
using Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;
using Soenneker.Serilog.Sinks.Browser.Blazor.Renderers.Base;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Renderers;

internal sealed class EventRenderer : BaseRenderer
{
    private readonly PropertyToken _token;
    private readonly IFormatProvider? _formatProvider;

    public EventRenderer(PropertyToken token, IFormatProvider? formatProvider)
    {
        _token = token;
        _formatProvider = formatProvider;
    }

    internal override void Render(LogEvent logEvent, TokenEmitter emitToken)
    {
        // If the property is missing, render nothing (but respect alignment if present).
        if (!logEvent.Properties.TryGetValue(_token.PropertyName, out LogEventPropertyValue? propertyValue))
        {
            if (_token.Alignment is not null)
                emitToken(new string(' ', _token.Alignment.Value.Width));
            return;
        }

        string result;

        // Handle string values with custom casing
        if (propertyValue is ScalarValue {Value: string literalString})
        {
            result = literalString.Format(_token.Format);
        }
        else
        {
            using var writer = new StringWriter();
            propertyValue.Render(writer, _token.Format, _formatProvider);
            result = writer.ToString();
        }

        // Apply alignment efficiently
        if (_token.Alignment is not null && result.Length < _token.Alignment.Value.Width)
            result = result.PadLeft(_token.Alignment.Value.Width);

        emitToken(result);
    }
}