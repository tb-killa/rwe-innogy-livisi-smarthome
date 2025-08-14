using System;

namespace Rebex;

public class TraceLogWriter : LogWriterBase
{
	private bool gnfxt;

	protected override bool IsClosed => gnfxt;

	public TraceLogWriter()
		: this(LogLevel.Info)
	{
	}

	public TraceLogWriter(LogLevel level)
	{
		base.Level = level;
	}

	protected override void Dispose(bool disposing)
	{
		if (!IsClosed || 1 == 0)
		{
			base.Dispose(disposing);
			gnfxt = true;
		}
	}

	protected override void WriteMessage(string message)
	{
		if (IsClosed && 0 == 0)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
	}
}
