using System;
using System.Collections.Generic;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer;

public interface IProtocolMultiplexer
{
	IDeviceController DeviceController { get; }

	IPhysicalStateHandler PhysicalState { get; }

	IProtocolSpecificDataBackup DataBackup { get; }

	List<ProtocolSpecificInformation> GetProtocolSpecificInformation();

	IEnumerable<IProtocolSpecificTransformation> GetProtocolSpecificTransformations();

	string GetDeviceDescription(Guid deviceId);

	List<Guid> GetHandledDevices();

	void ResetDeviceInclusionState(Guid deviceId);

	uint GetMaximumNumberOfHandledDevices();

	void ActivateDeviceDiscovery(List<string> appIds);

	void DeactivateDeviceDiscovery();

	void DropDiscoveredDevices(BaseDevice[] devices);
}
