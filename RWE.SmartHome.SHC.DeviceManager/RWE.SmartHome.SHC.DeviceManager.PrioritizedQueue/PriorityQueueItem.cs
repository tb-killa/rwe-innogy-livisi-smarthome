namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public class PriorityQueueItem<TValue, TPriority>
{
	public TValue Value { get; private set; }

	public TPriority Priority { get; private set; }

	public PriorityQueueItem(TValue value, TPriority priority)
	{
		Priority = priority;
		Value = value;
	}
}
