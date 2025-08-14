using System.Collections.Generic;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.DevicePolling.AdvancedPolling;

internal static class BidcosPollingStrategy
{
	private static List<PollingIntervals> intervals = new List<PollingIntervals>
	{
		PollingIntervals.PollIn5Secs,
		PollingIntervals.PollIn1Min,
		PollingIntervals.PollIn15Mins,
		PollingIntervals.PollIn1H,
		PollingIntervals.PollIn3H,
		PollingIntervals.PollIn6H,
		PollingIntervals.PollIn12H,
		PollingIntervals.PollEnd
	};

	internal static int IntervalCount => intervals.Count;

	public static PollingIntervals GetNextInterval(PollingIntervals current)
	{
		int num = intervals.IndexOf(current);
		if (num >= intervals.Count)
		{
			return PollingIntervals.PollEnd;
		}
		return intervals[++num];
	}
}
