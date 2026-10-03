using Serilog.Events;
using Serilog.Parsing;
using Soenneker.Serilog.Sinks.Browser.Blazor.Extensions;
using Soenneker.Serilog.Sinks.Browser.Blazor.Renderers.Base;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Renderers;

internal sealed class LevelRenderer : BaseRenderer
{
    private readonly PropertyToken _levelToken;
    private readonly string[] _prefixes;

    public LevelRenderer(PropertyToken levelToken)
    {
        _levelToken = levelToken;
        _prefixes = new string[6];
        for (var i = 0; i < _prefixes.Length; i++)
            _prefixes[i] = ((LogEventLevel)i).ToLevelPrefix().Pad(levelToken.Alignment);
    }

    internal override void Render(LogEvent logEvent, TokenEmitter emitToken)
    {
        int level = (int)logEvent.Level;
        string alignedOutput = (uint)level < (uint)_prefixes.Length
            ? _prefixes[level]
            : logEvent.Level.ToLevelPrefix().Pad(_levelToken.Alignment);
        emitToken(alignedOutput);
    }
}
