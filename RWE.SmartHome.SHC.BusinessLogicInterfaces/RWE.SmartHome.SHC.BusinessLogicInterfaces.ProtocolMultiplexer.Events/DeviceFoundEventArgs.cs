using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications.DeviceInclusion;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class DeviceFoundEventArgs : EventArgs
{
	public BaseDevice FoundDevice { get; set; }

	public DeviceFoundState State { get; set; }
}
