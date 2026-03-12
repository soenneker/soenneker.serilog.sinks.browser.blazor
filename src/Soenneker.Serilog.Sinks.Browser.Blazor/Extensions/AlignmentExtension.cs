using Serilog.Parsing;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;

internal static class AlignmentExtension
{
    internal static Alignment Widen(this Alignment alignment, int amount) => new(alignment.Direction, alignment.Width + amount);
}