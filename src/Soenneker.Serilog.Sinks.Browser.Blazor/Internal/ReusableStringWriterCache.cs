using System;
using Soenneker.Utils.ReusableStringWriter;

namespace Soenneker.Serilog.Sinks.Browser.Blazor.Internal;

internal static class ReusableStringWriterCache
{
	[ThreadStatic]
	private static ReusableStringWriter? _writer;

	internal static ReusableStringWriter Get()
	{
		ReusableStringWriter? writer = _writer;

		_writer = null;
		return writer ?? new ReusableStringWriter();
	}

	internal static void Return(ReusableStringWriter writer)
	{
		writer.Reset();
		_writer = writer;
	}
}
