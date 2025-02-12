using Serilog.Events;
using Serilog.Parsing;
using Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;
using Soenneker.Serilog.Sinks.Browser.Blazor.Renderers.Base;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Renderers;

internal sealed class LevelRenderer : BaseRenderer
{
    private readonly PropertyToken _levelToken;

    public LevelRenderer(PropertyToken levelToken)
    {
        _levelToken = levelToken;
    }

    internal override void Render(LogEvent logEvent, TokenEmitter emitToken)
    {
        string prefix = logEvent.Level.ToLevelPrefix(_levelToken.Format);
        string alignedOutput = prefix.Pad(_levelToken.Alignment);
        emitToken(alignedOutput);
    }
}