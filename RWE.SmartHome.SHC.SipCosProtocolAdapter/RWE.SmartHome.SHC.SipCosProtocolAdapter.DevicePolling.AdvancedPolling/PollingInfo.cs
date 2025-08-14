using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter.DevicePolling.AdvancedPolling;

internal class PollingInfo
{
	public PollingIntervals NextPollingInterval { get; private set; }

	public IDeviceInformation DeviceInfo { get; private set; }

	public Guid PollingTaskId { get; private set; }

	public IEnumerable<byte> StatusInfoChannels { get; private set; }

	public PollingInfo(IDeviceInformation deviceInfo, IEnumerable<byte> statusInfoChannels)
	{
		NextPollingInterval = PollingIntervals.PollIn5Secs;
		DeviceInfo = deviceInfo;
		StatusInfoChannels = statusInfoChannels;
	}

	public void PollingScheduled(Guid taskId)
	{
		Log.Debug(Module.SipCosProtocolAdapter, $"Device {DeviceInfo} unreachable. Will retry polling in {(int)NextPollingInterval} seconds.");
		PollingTaskId = taskId;
		NextPollingInterval = BidcosPollingStrategy.GetNextInterval(NextPollingInterval);
	}
}
