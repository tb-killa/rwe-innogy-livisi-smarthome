using System.Collections.Generic;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public class SendPriorityComparer : IComparer<SendPriority>
{
	public int Compare(SendPriority x, SendPriority y)
	{
		return x.Awake - y.Awake;
	}
}
