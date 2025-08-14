using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Configuration;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;

namespace RWE.SmartHome.SHC.BusinessLogic.ProtocolMultiplexer;

public class PhysicalStateHandler : IPhysicalStateHandler
{
	private readonly Dictionary<ProtocolIdentifier, IProtocolSpecificPhysicalStateHandler> protocolSpecificPhysicalStateHandlers = new Dictionary<ProtocolIdentifier, IProtocolSpecificPhysicalStateHandler>();

	private readonly IRepository configurationRepository;

	public PhysicalStateHandler(IRepository configurationRepository)
	{
		this.configurationRepository = configurationRepository;
	}

	public void RegisterProtocolSpecificStateRequestor(ProtocolIdentifier protocolIdentifier, IProtocolSpecificPhysicalStateHandler protocolSpecificPhysicalStateHandler)
	{
		if (protocolSpecificPhysicalStateHandler != null)
		{
			protocolSpecificPhysicalStateHandlers.Add(protocolIdentifier, protocolSpecificPhysicalStateHandler);
		}
	}

	public PhysicalDeviceState Get(Guid deviceId)
	{
		BaseDevice baseDevice = configurationRepository.GetBaseDevice(deviceId);
		if (baseDevice != null && protocolSpecificPhysicalStateHandlers.TryGetValue(baseDevice.ProtocolId, out var value))
		{
			return value.Get(deviceId);
		}
		return null;
	}

	public List<PhysicalDeviceState> GetAll()
	{
		List<PhysicalDeviceState> list = new List<PhysicalDeviceState>();
		foreach (IProtocolSpecificPhysicalStateHandler value in protocolSpecificPhysicalStateHandlers.Values)
		{
			List<PhysicalDeviceState> all = value.GetAll();
			if (all != null && all.Count > 0)
			{
				list.AddRange(all);
			}
		}
		return list;
	}

	public void UpdateDeviceConfigurationState(Guid deviceId, DeviceConfigurationState newConfigurationState)
	{
		BaseDevice baseDevice = configurationRepository.GetBaseDevice(deviceId);
		if (baseDevice != null && protocolSpecificPhysicalStateHandlers.TryGetValue(baseDevice.ProtocolId, out var value))
		{
			value.UpdateDeviceConfigurationState(deviceId, newConfigurationState);
		}
	}
}
