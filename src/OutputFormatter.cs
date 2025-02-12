using System;
using System.Collections.Generic;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Parsing;
using Soenneker.Serilog.Sinks.Browser.Blazor.Renderers;
using Soenneker.Serilog.Sinks.Browser.Blazor.Renderers.Base;

namespace Soenneker.Serilog.Sinks.Browser.Blazor;

internal sealed class OutputFormatter
{
    private readonly List<BaseRenderer> _renderers;

    internal OutputFormatter(string outputTemplate, IFormatProvider? formatProvider)
    {
        MessageTemplate template = new MessageTemplateParser().Parse(outputTemplate);
        _renderers = [];

        foreach (MessageTemplateToken token in template.Tokens)
        {
            switch (token)
            {
                case TextToken textToken:
                    _renderers.Add(new TextRenderer(textToken.Text));
                    break;
                case PropertyToken propertyToken:
                    _renderers.Add(CreateRenderer(propertyToken, formatProvider, template));
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported token type: {token.GetType().Name}");
            }
        }
    }

    private static BaseRenderer CreateRenderer(PropertyToken token, IFormatProvider? formatProvider, MessageTemplate template)
    {
        return token.PropertyName switch
        {
            OutputProperties.LevelPropertyName => new LevelRenderer(token),
            OutputProperties.NewLinePropertyName => new NewLineRenderer(token.Alignment),
            OutputProperties.ExceptionPropertyName => new ExceptionRenderer(),
            OutputProperties.MessagePropertyName => new MessageTemplateRenderer(),
            OutputProperties.TimestampPropertyName => new TimestampRenderer(token, formatProvider),
            OutputProperties.PropertiesPropertyName => new PropertiesRenderer(token, template),
            _ => new EventRenderer(token, formatProvider)
        };
    }

    internal object?[] Format(LogEvent logEvent)
    {
        var output = new List<object?>(_renderers.Count * 2);

        foreach (BaseRenderer renderer in _renderers)
        {
            renderer.Render(logEvent, output.Add);
        }

        return output.ToArray();
    }
}