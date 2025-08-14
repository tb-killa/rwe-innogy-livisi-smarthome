using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.wMBusProtocol;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Interfaces;

public interface IDeviceInformation
{
	Guid DeviceId { get; set; }

	DeviceInclusionState DeviceInclusionState { get; set; }

	DateTime DeviceFound { get; set; }

	byte[] DeviceIdentifier { get; set; }

	bool Reachable { get; set; }

	DeviceConfigurationState DeviceConfigurationState { get; set; }

	byte Version { get; set; }

	string Manufacturer { get; set; }

	byte[] ManufacturerCode { get; set; }

	DeviceTypeIdentification DeviceTypeIdentification { get; set; }

	DateTime LastTimeActive { get; set; }

	byte[] DecryptionKey { get; set; }

	SGTIN96 SGTIN96 { get; }

	byte? DeviceStatus { get; set; }

	DateTime? ReachableTimestamp { get; }

	DateTime? DeviceInclusionStateTimestamp { get; }

	DateTime? UpdateStateTimestamp { get; }

	DateTime? DeviceConfigurationStateTimestamp { get; }

	event EventHandler<DeviceInclusionStateChangedEventArgs> DeviceInclusionStateChanged;
}
