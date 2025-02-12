using System.IO;
using System.Collections.Generic;
using System.Text;
using Serilog.Events;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;

internal static class LogEventPropertyValueExtension
{
    internal static object? ToInteropValue(this LogEventPropertyValue value, string? format = null)
    {
        switch (value)
        {
            case ScalarValue sv when format is null:
                return sv.Value;

            case ScalarValue sv:
                var sb = new StringBuilder();
                using (var sw = new StringWriter(sb))
                {
                    sv.Render(sw, format);
                }
                return sb.ToString();

            case SequenceValue sqv:
                var array = new object?[sqv.Elements.Count];

                for (var i = 0; i < sqv.Elements.Count; i++)
                {
                    array[i] = sqv.Elements[i].ToInteropValue();
                }
                return array;

            case StructureValue st:
                var dict = new Dictionary<string, object?>(st.Properties.Count);

                foreach (LogEventProperty kv in st.Properties)
                {
                    dict[kv.Name] = kv.Value.ToInteropValue();
                }
                return dict;

            case DictionaryValue dv:
                var resultDict = new Dictionary<object, object?>(dv.Elements.Count);

                foreach (KeyValuePair<ScalarValue, LogEventPropertyValue> kv in dv.Elements)
                {
                    resultDict[kv.Key.ToInteropValue()!] = kv.Value.ToInteropValue();
                }

                return resultDict;

            default:
                return value;
        }
    }
}