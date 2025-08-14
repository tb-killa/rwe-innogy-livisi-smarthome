using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class PhysicalDeviceStateChangedEventArgs : EventArgs
{
	public PhysicalDeviceState PhysicalState { get; private set; }

	public Guid DeviceId { get; private set; }

	public PhysicalDeviceStateChangedEventArgs(Guid deviceId, PhysicalDeviceState physicalState)
	{
		DeviceId = deviceId;
		PhysicalState = physicalState;
	}
}
