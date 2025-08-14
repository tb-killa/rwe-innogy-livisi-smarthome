using System;
using System.Collections.Generic;
using System.Net;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.FirmwareUpdate;

namespace RWE.SmartHome.SHC.Lemonbeat.ProtocolAdapter.Persistence;

public class DeviceInformationEntity
{
	public Guid DeviceId { get; set; }

	public byte[] ByteAddress { get; set; }

	public uint? SubDeviceId { get; set; }

	public int? GatewayId { get; set; }

	public string DeviceKey { get; set; }

	public byte[] SGTIN { get; set; }

	public int DeviceInclusionState { get; set; }

	public int? DeviceUpdateState { get; set; }

	public DateTime DeviceFound { get; set; }

	public DeviceDescription DeviceDescription { get; set; }

	public List<MemoryInformation> MemoryInformation { get; set; }

	public List<ServiceDescription> ServiceDescriptions { get; set; }

	public List<ValueDescription> ValueDescriptions { get; set; }

	public int TimezoneOffset { get; set; }

	public bool WasReachable { get; set; }

	public DeviceInformationEntity()
	{
	}

	public DeviceInformationEntity(DeviceInformation deviceInfo)
	{
		DeviceId = deviceInfo.DeviceId;
		ByteAddress = deviceInfo.Identifier.IPAddress.GetAddressBytes();
		SubDeviceId = deviceInfo.Identifier.SubDeviceId;
		GatewayId = deviceInfo.Identifier.GatewayId;
		SGTIN = deviceInfo.DeviceDescription.SGTIN.GetSerialData().ToArray();
		DeviceInclusionState = (int)deviceInfo.DeviceInclusionState;
		DeviceUpdateState = (int)deviceInfo.DeviceUpdateState;
		DeviceFound = deviceInfo.DeviceFound;
		DeviceDescription = deviceInfo.DeviceDescription;
		MemoryInformation = deviceInfo.MemoryInformation;
		ServiceDescriptions = deviceInfo.ServiceDescriptions;
		ValueDescriptions = deviceInfo.ValueDescriptions;
		DeviceKey = deviceInfo.DeviceKey;
		TimezoneOffset = deviceInfo.TimezoneOffset;
		WasReachable = deviceInfo.IsReachable;
	}

	public DeviceInformation Convert()
	{
		DeviceDescription deviceDescription = new DeviceDescription();
		deviceDescription.SGTIN = SGTIN96.Create(SGTIN);
		DeviceDescription deviceDescription2 = deviceDescription;
		DeviceInformation deviceInformation = new DeviceInformation(DeviceId, new DeviceIdentifier(new IPAddress(ByteAddress), SubDeviceId, GatewayId ?? LemonbeatUsbDongle.LemonbeatUSBDongleGatewayID), deviceDescription2, DeviceFound);
		deviceInformation.DeviceInclusionState = (LemonbeatDeviceInclusionState)DeviceInclusionState;
		deviceInformation.DeviceUpdateState = (DeviceUpdateState.HasValue ? ((LemonbeatDeviceUpdateState)DeviceUpdateState.Value) : LemonbeatDeviceUpdateState.UpToDate);
		deviceInformation.DeviceDescription = DeviceDescription;
		deviceInformation.MemoryInformation = MemoryInformation;
		deviceInformation.ServiceDescriptions = ServiceDescriptions;
		deviceInformation.ValueDescriptions = ValueDescriptions;
		deviceInformation.DeviceKey = DeviceKey;
		deviceInformation.TimezoneOffset = TimezoneOffset;
		deviceInformation.IsReachable = WasReachable;
		return deviceInformation;
	}
}
