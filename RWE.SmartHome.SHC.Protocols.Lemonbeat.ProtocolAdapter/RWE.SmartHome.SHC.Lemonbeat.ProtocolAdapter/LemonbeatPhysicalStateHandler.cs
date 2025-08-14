using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.PropertiesSets;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.FirmwareUpdate;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Interfaces;

namespace RWE.SmartHome.SHC.Lemonbeat.ProtocolAdapter;

internal class LemonbeatPhysicalStateHandler : IProtocolSpecificPhysicalStateHandler
{
	private readonly IDeviceList deviceList;

	public LemonbeatPhysicalStateHandler(IDeviceList deviceList)
	{
		this.deviceList = deviceList;
	}

	public PhysicalDeviceState Get(Guid physicalDeviceId)
	{
		DeviceInformation deviceInformation = deviceList[physicalDeviceId];
		if (deviceInformation != null)
		{
			return GetPhysicalDeviceState(deviceInformation);
		}
		return null;
	}

	public List<PhysicalDeviceState> GetAll()
	{
		return deviceList.SyncSelect((DeviceInformation deviceInfo) => GetPhysicalDeviceState(deviceInfo));
	}

	private static PhysicalDeviceState GetPhysicalDeviceState(DeviceInformation deviceInfo)
	{
		PhysicalDeviceState physicalDeviceState = new PhysicalDeviceState();
		physicalDeviceState.PhysicalDeviceId = deviceInfo.DeviceId;
		PhysicalDeviceState physicalDeviceState2 = physicalDeviceState;
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.IsReachable, deviceInfo.IsReachable, GetTimestamp(deviceInfo.IsReachableTimestamp));
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.DeviceConfigurationState, deviceInfo.DeviceConfigurationState.ToContractsConfigurationState(), GetTimestamp(deviceInfo.DeviceConfigurationStateTimestamp));
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.DeviceInclusionState, deviceInfo.DeviceInclusionState.ToProtocolMultiplexerState().ToContractsInclusionState(), GetTimestamp(deviceInfo.DeviceInclusionStateTimestamp));
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.UpdateState, deviceInfo.DeviceUpdateState.ToContractsUpdateState(), GetTimestamp(deviceInfo.DeviceUpdateStateTimestamp));
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.FirmwareVersion, deviceInfo.DeviceDescription.BootloaderVersion + "-" + deviceInfo.DeviceDescription.StackVersion + "-" + deviceInfo.DeviceDescription.ApplicationVersion);
		return physicalDeviceState2;
	}

	private static DateTime GetTimestamp(DateTime? dateTime)
	{
		return dateTime ?? DateTime.UtcNow;
	}

	public void UpdateDeviceConfigurationState(Guid deviceId, DeviceConfigurationState newConfigurationState)
	{
		DeviceInformation deviceInformation = deviceList[deviceId];
		if (deviceInformation != null)
		{
			deviceInformation.DeviceConfigurationState = newConfigurationState;
		}
	}
}
