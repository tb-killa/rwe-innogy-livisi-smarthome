namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public class BackoffTime
{
	public int OverallWaitTime { get; private set; }

	public int DeviceSpecificWaitTime { get; private set; }

	public int ErrorCountIncrease { get; private set; }

	public BackoffTime(int overallWaitTime, int deviceSpecificWaitTime, int errorCountIncrease)
	{
		OverallWaitTime = overallWaitTime;
		DeviceSpecificWaitTime = deviceSpecificWaitTime;
		ErrorCountIncrease = errorCountIncrease;
	}
}
