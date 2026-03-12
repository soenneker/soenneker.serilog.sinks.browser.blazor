using Serilog.Parsing;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;

internal static class PaddingExtension
{
    internal static string Pad(this string value, Alignment? alignment)
    {
        if (alignment is null || value.Length >= alignment.Value.Width)
        {
            return value;
        }

        return alignment.Value.Direction is AlignmentDirection.Left
            ? value.PadRight(alignment.Value.Width)
            : value.PadLeft(alignment.Value.Width);
    }
}