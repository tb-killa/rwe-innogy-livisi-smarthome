using System;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.DeviceCommunication.Interfaces;

public interface IDeviceDescriptionService
{
	event EventHandler<DeviceDescriptionReceivedArgs> DeviceDescriptionReceived;

	DeviceDescription GetDeviceDescription(DeviceIdentifier identifier);

	void ExcludeDevice(DeviceIdentifier identifier);

	void IncludeDevice(DeviceIdentifier identifier);
}
