using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Interfaces;
using RWE.SmartHome.SHC.wMBusProtocol;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Persistence;

public class DeviceInformationEntity
{
	public Guid DeviceId { get; set; }

	public int DeviceInclusionState { get; set; }

	public long DeviceFound { get; set; }

	public byte[] DeviceIdentifier { get; set; }

	public byte DeviceTypeIdentification { get; set; }

	public byte[] Manufacturer { get; set; }

	public byte[] DecryptionKey { get; set; }

	public byte Version { get; set; }

	public DeviceInformationEntity()
	{
	}

	public DeviceInformationEntity(IDeviceInformation deviceInformation)
	{
		DeviceId = deviceInformation.DeviceId;
		DeviceFound = deviceInformation.DeviceFound.Ticks;
		DeviceInclusionState = (int)deviceInformation.DeviceInclusionState;
		DeviceIdentifier = deviceInformation.DeviceIdentifier;
		DeviceTypeIdentification = (byte)deviceInformation.DeviceTypeIdentification;
		Manufacturer = deviceInformation.ManufacturerCode;
		DecryptionKey = deviceInformation.DecryptionKey;
		Version = deviceInformation.Version;
	}

	public IDeviceInformation Convert()
	{
		IDeviceInformation deviceInformation = new DeviceInformation();
		deviceInformation.DeviceFound = new DateTime(DeviceFound);
		deviceInformation.DeviceId = DeviceId;
		deviceInformation.DeviceInclusionState = (DeviceInclusionState)DeviceInclusionState;
		deviceInformation.DeviceIdentifier = DeviceIdentifier;
		deviceInformation.DeviceTypeIdentification = (DeviceTypeIdentification)DeviceTypeIdentification;
		deviceInformation.ManufacturerCode = Manufacturer;
		deviceInformation.Manufacturer = WMBusFrame.ManufacturerName(Manufacturer, 0);
		deviceInformation.DecryptionKey = DecryptionKey;
		deviceInformation.Version = Version;
		return deviceInformation;
	}
}
