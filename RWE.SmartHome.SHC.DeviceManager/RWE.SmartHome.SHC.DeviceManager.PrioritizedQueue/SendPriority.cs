using System;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public class SendPriority
{
	public int Awake { get; set; }

	public int Sleeping { get; set; }

	public SendPriority(int awake, int sleeping)
	{
		Awake = awake;
		Sleeping = sleeping;
	}

	public SendPriority(int awake)
	{
		Awake = awake;
		Sleeping = 0;
	}

	public SendPriority Max(SendPriority priority)
	{
		if (Awake > priority.Awake)
		{
			if (Sleeping >= priority.Sleeping)
			{
				return this;
			}
		}
		else if (Sleeping <= priority.Sleeping)
		{
			return priority;
		}
		throw new InvalidOperationException("The order of the priorities shall not change. That means that both (awake, sleepin) values have to be higher.");
	}
}
