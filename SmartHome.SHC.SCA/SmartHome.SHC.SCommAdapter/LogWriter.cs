using System;
using Rebex;

namespace SmartHome.SHC.SCommAdapter;

public class LogWriter : ILogWriter
{
	public LogLevel Level { get; set; }

	public LogWriter(LogLevel level)
	{
		Level = level;
	}

	public void Write(LogLevel level, Type objectType, int objectId, string area, string message, byte[] buffer, int offset, int length)
	{
		Write(level, objectType, objectId, area, message);
	}

	public void Write(LogLevel level, Type objectType, int objectId, string area, string message)
	{
		if (level >= Level && (object)objectType != null)
		{
			Console.WriteLine("===[{0}]== objectType: {1}, area: {2}, message: {3}", level.ToString(), objectType.Name, area, message);
		}
	}
}
