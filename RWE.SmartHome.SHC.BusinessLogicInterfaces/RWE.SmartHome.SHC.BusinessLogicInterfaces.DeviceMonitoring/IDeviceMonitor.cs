using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.DeviceMonitoring;

public interface IDeviceMonitor
{
	List<BasicDriverInformation> MonitoredActiveDrivers { get; }

	event Action<BasicDriverInformation> DeviceConnected;

	void Start();

	void Stop();
}
