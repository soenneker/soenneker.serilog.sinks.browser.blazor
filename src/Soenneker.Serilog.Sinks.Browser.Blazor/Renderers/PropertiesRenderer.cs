using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Serilog.Events;
using Serilog.Parsing;
using Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;
using Soenneker.Serilog.Sinks.Browser.Blazor.Renderers.Base;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Renderers;

internal sealed class PropertiesRenderer : BaseRenderer
{
    private static readonly ConditionalWeakTable<MessageTemplate, HashSet<string>> _messageProperties = new();
    private readonly PropertyToken _token;
    private readonly HashSet<string> _templatePropertyNames;

    internal PropertiesRenderer(PropertyToken token, MessageTemplate outputTemplate)
    {
        _token = token;

        // Precompute property names in the template for fast lookup
        _templatePropertyNames = GetTemplatePropertyNames(outputTemplate);
    }

    internal override void Render(LogEvent logEvent, TokenEmitter emitToken)
    {
        // Precompute property names from the message template
        if (logEvent.Properties.Count == 0)
            return;
        HashSet<string> messageTemplateProperties = _messageProperties.GetValue(logEvent.MessageTemplate, GetTemplatePropertyNames);

        foreach (KeyValuePair<string, LogEventPropertyValue> property in logEvent.Properties)
        {
            // Skip properties already in either template
            if (messageTemplateProperties.Contains(property.Key) || _templatePropertyNames.Contains(property.Key))
                continue;

            emitToken(property.Value.ToInteropValue(_token.Format));
        }

    }

    private static HashSet<string> GetTemplatePropertyNames(MessageTemplate template)
    {
        var propertyNames = new HashSet<string>();

        foreach (MessageTemplateToken token in template.Tokens)
        {
            if (token is PropertyToken namedProperty)
            {
                propertyNames.Add(namedProperty.PropertyName);
            }
        }

        return propertyNames;
    }
}
