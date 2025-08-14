using System;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public static class TimeSpanExtension
{
	public static TimeSpan Min(this TimeSpan me, TimeSpan other)
	{
		if (me.Ticks < other.Ticks)
		{
			return me;
		}
		return other;
	}

	public static TimeSpan Max(this TimeSpan me, TimeSpan other)
	{
		if (me.Ticks > other.Ticks)
		{
			return me;
		}
		return other;
	}
}
