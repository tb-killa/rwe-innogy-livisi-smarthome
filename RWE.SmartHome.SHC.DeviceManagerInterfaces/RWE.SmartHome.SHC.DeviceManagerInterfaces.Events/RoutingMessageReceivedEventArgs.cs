using System;
using RWE.SmartHome.SHC.CommonFunctionality;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;

public class RoutingMessageReceivedEventArgs : EventArgs
{
	public SIPcosRouteManagementFrame RouteManagementFrame { get; set; }

	public DateTime TimeStamp { get; private set; }

	public RoutingMessageReceivedEventArgs(SIPcosRouteManagementFrame routeManagementFrame)
	{
		RouteManagementFrame = routeManagementFrame;
		TimeStamp = ShcDateTime.UtcNow;
	}
}
