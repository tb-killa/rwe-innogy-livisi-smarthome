using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.PropertiesSets;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Interfaces;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter;

internal class WMBusPhysicalStateHandler : IProtocolSpecificPhysicalStateHandler
{
	private readonly IDeviceList deviceList;

	public WMBusPhysicalStateHandler(IDeviceList deviceList)
	{
		this.deviceList = deviceList;
	}

	public PhysicalDeviceState Get(Guid physicalDeviceId)
	{
		IDeviceInformation deviceInformation = deviceList[physicalDeviceId];
		if (deviceInformation == null)
		{
			return null;
		}
		return CreatePhysicalDeviceState(deviceInformation);
	}

	public List<PhysicalDeviceState> GetAll()
	{
		List<IDeviceInformation> source = new List<IDeviceInformation>(deviceList);
		return source.Select((IDeviceInformation deviceInformation) => CreatePhysicalDeviceState(deviceInformation)).ToList();
	}

	public void UpdateDeviceConfigurationState(Guid deviceId, DeviceConfigurationState newConfigurationState)
	{
		IDeviceInformation deviceInformation = deviceList[deviceId];
		if (deviceInformation != null)
		{
			deviceInformation.DeviceConfigurationState = newConfigurationState;
		}
	}

	private PhysicalDeviceState CreatePhysicalDeviceState(IDeviceInformation devInfo)
	{
		PhysicalDeviceState physicalDeviceState = new PhysicalDeviceState();
		physicalDeviceState.PhysicalDeviceId = devInfo.DeviceId;
		PhysicalDeviceState physicalDeviceState2 = physicalDeviceState;
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.IsReachable, devInfo.Reachable, devInfo.ReachableTimestamp.GetValueOrDefault());
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.DeviceConfigurationState, devInfo.DeviceConfigurationState.ToContractsConfigurationState(), devInfo.DeviceConfigurationStateTimestamp.GetValueOrDefault());
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.DeviceInclusionState, devInfo.DeviceInclusionState.ToContractsInclusionState(), devInfo.DeviceInclusionStateTimestamp.GetValueOrDefault());
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.FirmwareVersion, devInfo.Version.ToString(CultureInfo.InvariantCulture));
		return physicalDeviceState2;
	}
}
