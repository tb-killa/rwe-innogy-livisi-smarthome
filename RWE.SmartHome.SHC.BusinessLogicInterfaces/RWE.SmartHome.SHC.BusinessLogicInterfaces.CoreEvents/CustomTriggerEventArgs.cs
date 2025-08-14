using System;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.CoreEvents;

public class CustomTriggerEventArgs : EventArgs
{
	public Guid TriggerId { get; private set; }

	public CustomTriggerEventArgs(Guid triggerId)
	{
		TriggerId = triggerId;
	}
}
