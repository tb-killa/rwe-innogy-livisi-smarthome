using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;

namespace RWE.SmartHome.SHC.Core.Logging;

public class LogEventArgs
{
	public Severity LogLevel { get; set; }

	public string Source { get; set; }

	public Module Module { get; set; }

	public string Message { get; set; }

	public bool IsPersisted { get; set; }

	public bool IsSynchronous { get; set; }

	public DateTime Timestamp { get; set; }

	public LogEventArgs()
	{
		IsPersisted = false;
		Timestamp = DateTime.UtcNow;
	}
}
