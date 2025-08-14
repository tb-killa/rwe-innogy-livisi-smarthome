using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.DeviceState;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;

public interface IPhysicalStateHandler
{
	void RegisterProtocolSpecificStateRequestor(ProtocolIdentifier protocolIdentifier, IProtocolSpecificPhysicalStateHandler protocolSpecificPhysicalStateHandler);

	PhysicalDeviceState Get(Guid deviceId);

	List<PhysicalDeviceState> GetAll();

	void UpdateDeviceConfigurationState(Guid deviceId, DeviceConfigurationState newConfigurationState);
}
