using System;
using System.IO;
using Serilog.Events;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Tests;

internal sealed class NestedLogValue(Action writeNested) : LogEventPropertyValue
{
    public override void Render(TextWriter output, string? format = null, IFormatProvider? formatProvider = null)
    {
        output.Write("before");
        writeNested();
        output.Write("after");
    }
}
