using System;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;
using SmartHome.SHC.API.Protocols.Lemonbeat;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.LemonbeatCoreServices;

internal class PhysicalDeviceProvider : IPhysicalDeviceProvider
{
	private readonly IDeviceList deviceList;

	internal PhysicalDeviceProvider(IDeviceList deviceList)
	{
		this.deviceList = deviceList;
	}

	public LemonbeatPhysicalDevice GetPhysicalDevice(Guid baseDeviceId)
	{
		DeviceInformation deviceInformation = deviceList[baseDeviceId];
		if (deviceInformation != null)
		{
			LemonbeatPhysicalDevice lemonbeatPhysicalDevice = new LemonbeatPhysicalDevice();
			lemonbeatPhysicalDevice.DeviceDescription = LemonbeatApiConverters.ToApiDeviceDescription(deviceInformation.DeviceDescription);
			lemonbeatPhysicalDevice.MemoryInformation = LemonbeatApiConverters.ToApiMemoryInformation(deviceInformation.MemoryInformation);
			lemonbeatPhysicalDevice.ValueDescription = LemonbeatApiConverters.ToApiValueDescription(deviceInformation.ValueDescriptions);
			return lemonbeatPhysicalDevice;
		}
		return null;
	}
}
