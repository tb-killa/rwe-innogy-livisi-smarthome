using System;
using Rebex;

namespace SmartHome.SHC.SCommAdapter;

public class HandleLogWriter : ILogWriter
{
	private readonly WriteLog writeLogHandler;

	public LogLevel Level { get; set; }

	public HandleLogWriter(WriteLog handler, LogLevel logLevel)
	{
		writeLogHandler = handler;
		Level = logLevel;
	}

	public void Write(LogLevel level, Type objectType, int objectId, string area, string message, byte[] buffer, int offset, int length)
	{
		writeLogHandler?.Invoke(level, objectType, objectId, area, message, buffer, offset, length);
	}

	public void Write(LogLevel level, Type objectType, int objectId, string area, string message)
	{
		Write(level, objectType, objectId, area, message, null, 0, 0);
	}
}
