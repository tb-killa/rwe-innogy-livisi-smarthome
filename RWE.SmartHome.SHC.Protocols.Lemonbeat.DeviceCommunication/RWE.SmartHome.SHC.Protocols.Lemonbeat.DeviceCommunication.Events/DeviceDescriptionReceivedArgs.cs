using System;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;

public class DeviceDescriptionReceivedArgs : EventArgs
{
	public DeviceIdentifier Identifier { get; set; }

	public DeviceDescription DeviceDescription { get; set; }

	public DeviceDescriptionReceivedArgs(DeviceIdentifier identifier, DeviceDescription deviceDescription)
	{
		Identifier = identifier;
		DeviceDescription = deviceDescription;
	}
}
