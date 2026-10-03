using System;
using System.Globalization;
using Serilog.Events;
using Serilog.Parsing;
using Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;
using Soenneker.Serilog.Sinks.Browser.Blazor.Renderers.Base;
using Soenneker.Utils.ReusableStringWriter;
using Soenneker.Serilog.Sinks.Browser.Blazor.Internal;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Renderers;

internal sealed class TimestampRenderer : BaseRenderer
{
    private readonly PropertyToken _token;
    private readonly IFormatProvider? _formatProvider;

    internal TimestampRenderer(PropertyToken token, IFormatProvider? formatProvider)
    {
        _token = token;
        _formatProvider = formatProvider;
    }

    internal override void Render(LogEvent logEvent, TokenEmitter emitToken)
    {
        if (_formatProvider is null)
        {
            emitToken(logEvent.Timestamp.ToString(_token.Format, CultureInfo.InvariantCulture).Pad(_token.Alignment));
            return;
        }

        var scalarValue = new ScalarValue(logEvent.Timestamp);
        ReusableStringWriter writer = ReusableStringWriterCache.Get();
        string result;
        try
        {
            scalarValue.Render(writer, _token.Format, _formatProvider);
            result = writer.Finish();
        }
        finally
        {
            ReusableStringWriterCache.Return(writer);
        }

        if (_token.Alignment is not null)
            emitToken(result.Pad(_token.Alignment));
        else
            emitToken(result);
    }
}
