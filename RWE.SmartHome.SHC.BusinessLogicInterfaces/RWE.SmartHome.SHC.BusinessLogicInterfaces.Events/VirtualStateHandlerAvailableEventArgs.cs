using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.VirtualDevices;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class VirtualStateHandlerAvailableEventArgs : EventArgs
{
	public IVirtualCoreActionHandler Handler { get; private set; }

	public VirtualStateHandlerAvailableEventArgs(IVirtualCoreActionHandler handler)
	{
		Handler = handler;
	}
}
