using System;

namespace Rebex;

public interface ILogWriter
{
	LogLevel Level { get; set; }

	void Write(LogLevel level, Type objectType, int objectId, string area, string message);

	void Write(LogLevel level, Type objectType, int objectId, string area, string message, byte[] buffer, int offset, int length);
}
