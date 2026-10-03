using System;
using System.Globalization;
using System.IO;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;
using Soenneker.Serilog.Sinks.Browser.Blazor.Registrars;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Tests;

public sealed class RenderingTests
{
    [Test]
    public void Timestamp_formats_match_Serilog_with_default_and_explicit_culture()
    {
        var timestamp = new DateTimeOffset(2026, 10, 3, 15, 26, 37, TimeSpan.FromHours(-5));
        foreach (IFormatProvider? provider in new IFormatProvider?[] { null, CultureInfo.GetCultureInfo("fr-FR") })
        foreach (string? format in new string?[] { null, "O", "G", "u", "HH:mm:ss", "yyyy-MM-dd" })
        {
            var runtime = new RecordingJsRuntime();
            string outputTemplate = format is null ? "{Timestamp}" : "{Timestamp:" + format + "}";
            using var logger = new LoggerConfiguration().WriteTo.BlazorConsole(runtime, outputTemplate: outputTemplate, formatProvider: provider).CreateLogger();
            logger.Write(new LogEvent(timestamp, LogEventLevel.Information, null, new MessageTemplateParser().Parse(""), []));
            using var expected = new StringWriter();
            new ScalarValue(timestamp).Render(expected, format, provider);
            if (runtime.Calls.Count != 1 || string.Concat(runtime.Calls[0]) != expected.ToString())
                throw new InvalidOperationException("Timestamp formatting changed.");
        }
    }

    [Test]
    public void Nested_logging_preserves_both_output_buffers()
    {
        var runtime = new RecordingJsRuntime();
        using var logger = new LoggerConfiguration().WriteTo.BlazorConsole(runtime, outputTemplate: "start {Value} end").CreateLogger();
        var template = new MessageTemplateParser().Parse("{Value}");
        var nested = new LogEvent(DateTimeOffset.UtcNow, LogEventLevel.Information, null, template,
            [new LogEventProperty("Value", new ScalarValue(123))]);
        var outer = new LogEvent(DateTimeOffset.UtcNow, LogEventLevel.Information, null, template,
            [new LogEventProperty("Value", new NestedLogValue(() => logger.Write(nested)))]);
        logger.Write(outer);
        if (runtime.Calls.Count != 2 || string.Concat(runtime.Calls[0]) != "start 123 end" || string.Concat(runtime.Calls[1]) != "start beforeafter end")
            throw new InvalidOperationException("Reentrant formatting corrupted a log event.");
    }

    [Test]
    public void Properties_excludes_message_and_output_properties_on_repeated_renders()
    {
        var runtime = new RecordingJsRuntime();
        using var logger = new LoggerConfiguration().WriteTo.BlazorConsole(runtime, outputTemplate: "{Message}|{Output}|{Properties}").CreateLogger();
        var template = new MessageTemplateParser().Parse("{MessageValue}");
        for (var i = 0; i < 2; i++)
        {
            logger.Write(new LogEvent(DateTimeOffset.UtcNow, LogEventLevel.Information, null, template,
                [new("MessageValue", new ScalarValue("message")), new("Output", new ScalarValue("output")), new("Extra", new ScalarValue(i))]));
            if (string.Concat(runtime.Calls[i]) != $"message|output|{i}")
                throw new InvalidOperationException("Property selection changed when using cached template metadata.");
        }
    }
}
