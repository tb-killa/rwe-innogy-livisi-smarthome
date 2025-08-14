using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;

namespace RWE.SmartHome.SHC.Core.Logging.Entities;

public class LogEntry
{
	public const string ID_COLUMN = "Id";

	public const string SEVERITY_COLUMN = "Severity";

	public const string TIMESTAMP_COLUMN = "TimeStamp";

	public const string SOURCE_COLUMN = "Source";

	public const string MESSAGE_COLUMN = "Message";

	public const string TABLE = "LogEntries";

	public const string PRIMARY_KEY = "LogEntries_PKey";

	public int Id { get; internal set; }

	public Severity LogLevel { get; set; }

	public DateTime Timestamp { get; internal set; }

	public string Source { get; set; }

	public string Message { get; set; }

	public LogEntry()
	{
		Id = 0;
		Timestamp = DateTime.UtcNow;
	}

	public LogEntry(Module module, string source, Severity logLevel, string message, DateTime timeStamp)
		: this()
	{
		LogLevel = logLevel;
		Message = message;
		Source = ((source != string.Empty) ? $"{module}.{source}" : module.ToString());
		Timestamp = timeStamp;
	}

	public LogEntry(string source, Severity logLevel, string message)
		: this()
	{
		LogLevel = logLevel;
		Message = message;
		Source = source;
	}
}
