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

		if (writer is null)
		{
			writer = new ReusableStringWriter();
			_writer = writer;
		}

		writer.Reset();
		return writer;
	}
}
