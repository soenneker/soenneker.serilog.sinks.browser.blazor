using System;
using Serilog.Events;
using Serilog.Parsing;
using Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;
using Soenneker.Serilog.Sinks.Browser.Blazor.Renderers.Base;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Renderers;

internal sealed class MessageTemplateRenderer : BaseRenderer
{
    internal override void Render(LogEvent logEvent, TokenEmitter emitToken)
    {
        foreach (MessageTemplateToken? token in logEvent.MessageTemplate.Tokens)
        {
            switch (token)
            {
                case TextToken tt:
                    emitToken(tt.Text);
                    break;
                case PropertyToken pt:
                    if (logEvent.Properties.TryGetValue(pt.PropertyName, out LogEventPropertyValue? propertyValue))
                        emitToken(propertyValue.ToInteropValue());
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}