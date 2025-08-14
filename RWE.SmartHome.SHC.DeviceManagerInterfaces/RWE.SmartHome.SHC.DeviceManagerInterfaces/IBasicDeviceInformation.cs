using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces;

public interface IBasicDeviceInformation
{
	byte[] Address { get; set; }

	byte SequenceNumber { get; set; }

	ProtocolType ProtocolType { get; set; }

	DeviceInfoOperationModes BestOperationMode { get; }

	bool DeviceUnreachable { get; set; }

	byte Rssi { get; set; }

	DeviceInclusionState DeviceInclusionState { get; set; }

	DeviceConfigurationState DeviceConfigurationState { get; set; }

	Guid DeviceId { get; }

	bool Awake();

	void UpdateAwakeState(AwakeModifier awakeModifier);

	void MarkDeviceAsSleeping();

	bool IsDeviceSleeping();
}
