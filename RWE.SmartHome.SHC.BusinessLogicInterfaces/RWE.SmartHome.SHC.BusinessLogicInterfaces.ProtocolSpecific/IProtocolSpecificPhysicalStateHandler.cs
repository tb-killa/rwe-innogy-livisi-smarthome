using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;

public interface IProtocolSpecificPhysicalStateHandler
{
	PhysicalDeviceState Get(Guid physicalDeviceId);

	List<PhysicalDeviceState> GetAll();

	void UpdateDeviceConfigurationState(Guid deviceId, DeviceConfigurationState newConfigurationState);
}
