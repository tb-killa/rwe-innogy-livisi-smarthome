using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;

public interface IProtocolAdapter
{
	ProtocolIdentifier ProtocolId { get; }

	IProtocolSpecificLogicalStateRequestor LogicalState { get; }

	IProtocolSpecificPhysicalStateHandler PhysicalState { get; }

	IProtocolSpecificDeviceController DeviceController { get; }

	IProtocolSpecificTransformation Transformation { get; }

	IProtocolSpecificDataBackup DataBackup { get; }

	IEnumerable<Guid> GetHandledDevices();

	string GetDeviceDescription(Guid deviceId);

	void ResetDeviceInclusionState(Guid deviceId);

	void DropDiscoveredDevices(BaseDevice[] devices);

	ProtocolSpecificInformation GetProtocolSpecificInformation();
}
