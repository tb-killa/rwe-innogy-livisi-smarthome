using System;

namespace RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;

public static class DateTimeExtension
{
	public static DateTime Min(this DateTime me, DateTime other)
	{
		if (me.Ticks < other.Ticks)
		{
			return me;
		}
		return other;
	}

	public static DateTime Max(this DateTime me, DateTime other)
	{
		if (me.Ticks > other.Ticks)
		{
			return me;
		}
		return other;
	}
}
