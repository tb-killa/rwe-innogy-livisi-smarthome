using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;

public interface IVirtualCurrentPhysicalStateHandler
{
	event EventHandler<VirtualDeviceAvailableArgs> StateChanged;

	PhysicalDeviceState GetState(Guid deviceId);
}
