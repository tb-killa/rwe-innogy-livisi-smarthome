using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceInformation.PropertiesSets;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;

namespace RWE.SmartHome.SHC.SipCosProtocolAdapter;

public class SipCosPhysicalStateHandler : IProtocolSpecificPhysicalStateHandler
{
	private readonly IDeviceManager deviceManager;

	private readonly IRepository configurationRepository;

	public SipCosPhysicalStateHandler(IDeviceManager deviceManager, IRepository configurationRepository)
	{
		this.configurationRepository = configurationRepository;
		this.deviceManager = deviceManager;
	}

	public PhysicalDeviceState Get(Guid deviceId)
	{
		PhysicalDeviceState result = null;
		IDeviceInformation deviceInformation = deviceManager.DeviceList[deviceId];
		if (deviceInformation != null)
		{
			result = CreatePhysicalDeviceState(deviceInformation);
		}
		return result;
	}

	private static PhysicalDeviceState CreatePhysicalDeviceState(IDeviceInformation device)
	{
		PhysicalDeviceState physicalDeviceState = new PhysicalDeviceState();
		physicalDeviceState.PhysicalDeviceId = device.DeviceId;
		PhysicalDeviceState physicalDeviceState2 = physicalDeviceState;
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.IsReachable, !device.DeviceUnreachable, device.DeviceUnreachableTimestamp.GetValueOrDefault());
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.DeviceConfigurationState, device.DeviceConfigurationState.ToContractsConfigurationState(), device.DeviceConfigurationStateTimestamp.GetValueOrDefault());
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.DeviceInclusionState, device.DeviceInclusionState.ToContractsInclusionState(), device.DeviceInclusionStateTimestamp.GetValueOrDefault());
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.UpdateState, device.UpdateState.ToContractsUpdateState(), device.UpdateStateTimestamp.GetValueOrDefault());
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.UpdateState, device.UpdateState.ToContractsUpdateState(), device.UpdateStateTimestamp.GetValueOrDefault());
		physicalDeviceState2.DeviceProperties.SetValue(PhysicalDeviceBasicProperties.FirmwareVersion, device.ManufacturerDeviceAndFirmware.ToString("X2").Insert(1, "."));
		return physicalDeviceState2;
	}

	public List<PhysicalDeviceState> GetAll()
	{
		lock (deviceManager.DeviceList.SyncRoot)
		{
			return (from device in deviceManager.DeviceList
				select CreatePhysicalDeviceState(device) into pds
				where configurationRepository.GetBaseDevice(pds.PhysicalDeviceId) != null
				select pds).ToList();
		}
	}

	public void UpdateDeviceConfigurationState(Guid deviceId, DeviceConfigurationState newConfigurationState)
	{
		IDeviceInformation deviceInformation = deviceManager.DeviceList[deviceId];
		if (deviceInformation != null)
		{
			deviceInformation.DeviceConfigurationState = newConfigurationState;
		}
	}
}
