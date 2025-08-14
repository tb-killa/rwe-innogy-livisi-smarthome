using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class ShcRebootScheduledEventArgs
{
	public TimeSpan Timeout { get; set; }

	public string Requester { get; set; }

	public string Reason { get; set; }

	public ShcRebootScheduledEventArgs()
	{
	}

	public ShcRebootScheduledEventArgs(TimeSpan timeout, string requester, string reason)
	{
		Timeout = timeout;
		Requester = requester;
		Reason = reason;
	}
}
