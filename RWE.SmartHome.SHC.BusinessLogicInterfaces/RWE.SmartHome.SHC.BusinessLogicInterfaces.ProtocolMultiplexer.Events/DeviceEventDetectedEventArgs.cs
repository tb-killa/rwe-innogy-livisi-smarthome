using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;

public class DeviceEventDetectedEventArgs : EventArgs
{
	public Guid LogicalDeviceId { get; private set; }

	public List<Property> EventDetails { get; private set; }

	public string EventType { get; private set; }

	public DeviceEventDetectedEventArgs(Guid deviceId, string eventType, List<Property> eventDetails)
	{
		LogicalDeviceId = deviceId;
		EventType = eventType;
		EventDetails = eventDetails;
	}
}
