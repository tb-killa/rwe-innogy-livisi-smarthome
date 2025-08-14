using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter.Interfaces;
using RWE.SmartHome.SHC.wMBusProtocol;

namespace RWE.SmartHome.SHC.Protocols.wMBus.CommunicationStack.ProtocolAdapter;

internal class DeviceInformation : IDeviceInformation
{
	private DeviceInclusionState deviceInclusionState;

	public bool isReachable;

	private DeviceConfigurationState deviceConfigurationState;

	public Guid DeviceId { get; set; }

	public byte[] ManufacturerCode { get; set; }

	public DeviceInclusionState DeviceInclusionState
	{
		get
		{
			return deviceInclusionState;
		}
		set
		{
			if (deviceInclusionState != value)
			{
				deviceInclusionState = value;
				DeviceInclusionStateTimestamp = ShcDateTime.UtcNow;
				this.DeviceInclusionStateChanged?.Invoke(this, new DeviceInclusionStateChangedEventArgs(DeviceId, deviceInclusionState, ProtocolIdentifier.wMBus.ToString()));
			}
		}
	}

	public DateTime DeviceFound { get; set; }

	public byte[] DeviceIdentifier { get; set; }

	public bool Reachable
	{
		get
		{
			return isReachable;
		}
		set
		{
			if (value != isReachable)
			{
				ReachableTimestamp = ShcDateTime.UtcNow;
			}
			isReachable = value;
		}
	}

	public DeviceConfigurationState DeviceConfigurationState
	{
		get
		{
			return deviceConfigurationState;
		}
		set
		{
			if (value != deviceConfigurationState)
			{
				DeviceConfigurationStateTimestamp = ShcDateTime.UtcNow;
			}
			deviceConfigurationState = value;
		}
	}

	public byte Version { get; set; }

	public string Manufacturer { get; set; }

	public DeviceTypeIdentification DeviceTypeIdentification { get; set; }

	public DateTime LastTimeActive { get; set; }

	public byte[] DecryptionKey { get; set; }

	public SGTIN96 SGTIN96
	{
		get
		{
			ushort itemReference = BitConverter.ToUInt16(new byte[2] { 2, 1 }, 0);
			byte[] array = new byte[8];
			byte[] array2 = WMBusFrame.CreateManufacturerId(Manufacturer);
			Array.Copy(array2, 0, array, 0, array2.Length);
			ulong companyPrefix = BitConverter.ToUInt64(array, 0);
			byte[] array3 = new byte[8];
			Array.Copy(DeviceIdentifier, 0, array3, 0, DeviceIdentifier.Length);
			int num = BCDConverter.ConvertFromBcd(array3);
			SGTIN96 sGTIN = new SGTIN96();
			sGTIN.FilterValue = 0;
			sGTIN.Partition = 5;
			sGTIN.ItemReference = itemReference;
			sGTIN.CompanyPrefix = companyPrefix;
			sGTIN.SerialNumber = (ulong)num;
			return sGTIN;
		}
	}

	public byte? DeviceStatus { get; set; }

	public DateTime? ReachableTimestamp { get; private set; }

	public DateTime? DeviceInclusionStateTimestamp { get; private set; }

	public DateTime? UpdateStateTimestamp { get; private set; }

	public DateTime? DeviceConfigurationStateTimestamp { get; private set; }

	public event EventHandler<DeviceInclusionStateChangedEventArgs> DeviceInclusionStateChanged;

	public DeviceInformation(Guid deviceId, DeviceInclusionState deviceInclusionState, DateTime deviceFound, byte[] identification, byte version, string manufacturer, byte[] manufacturerCode, DeviceTypeIdentification deviceTypeIdentification)
	{
		ManufacturerCode = manufacturerCode;
		DeviceId = deviceId;
		DeviceInclusionState = deviceInclusionState;
		DeviceFound = deviceFound;
		DeviceIdentifier = identification;
		isReachable = true;
		deviceConfigurationState = DeviceConfigurationState.Complete;
		Version = version;
		Manufacturer = manufacturer;
		DeviceTypeIdentification = deviceTypeIdentification;
	}

	public DeviceInformation()
	{
	}

	public override string ToString()
	{
		return $"Id: {DeviceId}, Identifier {DeviceIdentifier.ToReadable()}, Device found: {DeviceFound}, Inclusion state: {DeviceInclusionState}, Reachable: {Reachable}, Configuration state: {DeviceConfigurationState}";
	}
}
