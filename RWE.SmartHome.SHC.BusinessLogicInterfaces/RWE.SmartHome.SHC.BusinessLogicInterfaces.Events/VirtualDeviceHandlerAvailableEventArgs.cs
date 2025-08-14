using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class VirtualDeviceHandlerAvailableEventArgs : EventArgs
{
	public IVirtualCurrentPhysicalStateHandler Handler { get; private set; }

	public VirtualDeviceHandlerAvailableEventArgs(IVirtualCurrentPhysicalStateHandler handler)
	{
		Handler = handler;
	}
}
