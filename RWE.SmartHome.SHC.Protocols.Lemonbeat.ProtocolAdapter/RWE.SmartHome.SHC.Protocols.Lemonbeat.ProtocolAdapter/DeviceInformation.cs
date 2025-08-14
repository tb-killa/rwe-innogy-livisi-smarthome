using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.Events;
using RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter.FirmwareUpdate;
using SmartHome.SHC.API.Protocols.Lemonbeat;

namespace RWE.SmartHome.SHC.Protocols.Lemonbeat.ProtocolAdapter;

public class DeviceInformation
{
	private LemonbeatDeviceInclusionState deviceInclusionState;

	private LemonbeatDeviceTypeIdentifier deviceTypeIdentifier;

	private int isReachable;

	private DeviceConfigurationState deviceConfigurationState;

	private LemonbeatDeviceUpdateState deviceUpdateState;

	public Guid DeviceId { get; set; }

	public DeviceIdentifier Identifier { get; set; }

	public DateTime DeviceFound { get; set; }

	public DeviceDescription DeviceDescription { get; set; }

	public List<RWE.SmartHome.SHC.Protocols.Lemonbeat.DomainModel.MemoryInformation> MemoryInformation { get; set; }

	public List<ValueDescription> ValueDescriptions { get; set; }

	public List<ServiceDescription> ServiceDescriptions { get; set; }

	public string DeviceKey { get; set; }

	public string ControllerKey { get; set; }

	public DateTime LastDeviceReachabilityTestedTime { get; set; }

	public TimeSpan? PollingInterval { get; set; }

	public bool ReachabilityParametersReadFromAddin { get; set; }

	public LemonbeatDeviceTypeIdentifier DeviceTypeIdentifier
	{
		get
		{
			if (deviceTypeIdentifier == null)
			{
				deviceTypeIdentifier = new LemonbeatDeviceTypeIdentifier(DeviceDescription.ManufacturerId, DeviceDescription.ManufacturerProductId);
			}
			return deviceTypeIdentifier;
		}
	}

	public DateTime? IsReachableTimestamp { get; private set; }

	public bool IsReachable
	{
		get
		{
			return isReachable != 0;
		}
		set
		{
			int num = (value ? 1 : 0);
			int num2 = Interlocked.CompareExchange(ref isReachable, num, 1 - num);
			if (num2 != num)
			{
				IsReachableTimestamp = ShcDateTime.UtcNow;
				this.DeviceReachabilityChanged?.Invoke(this, new DeviceReachabilityChangedEventArgs(this, isReachable != 0));
			}
		}
	}

	public DateTime? DeviceInclusionStateTimestamp { get; private set; }

	public LemonbeatDeviceInclusionState DeviceInclusionState
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
				this.DeviceInclusionStateChanged?.Invoke(this, new LemonbeatDeviceInclusionStateChangedEventArgs(DeviceId, deviceInclusionState));
			}
		}
	}

	public DateTime? DeviceConfigurationStateTimestamp { get; private set; }

	public DeviceConfigurationState DeviceConfigurationState
	{
		get
		{
			return deviceConfigurationState;
		}
		set
		{
			if (deviceConfigurationState != value)
			{
				deviceConfigurationState = value;
				DeviceConfigurationStateTimestamp = ShcDateTime.UtcNow;
				this.DeviceConfiguredStateChanged?.Invoke(this, new DeviceConfiguredEventArgs
				{
					DeviceId = DeviceId,
					State = value
				});
			}
		}
	}

	public DateTime? DeviceUpdateStateTimestamp { get; private set; }

	public LemonbeatDeviceUpdateState DeviceUpdateState
	{
		get
		{
			return deviceUpdateState;
		}
		set
		{
			if (deviceUpdateState != value)
			{
				LemonbeatDeviceUpdateState lemonbeatUpdateState = deviceUpdateState;
				deviceUpdateState = value;
				DeviceUpdateStateTimestamp = ShcDateTime.UtcNow;
				this.DeviceUpdateStateChanged?.Invoke(this, new DeviceUpdateStateChangedEventArgs
				{
					DeviceId = DeviceId,
					NewDeviceUpdateState = deviceUpdateState.ToFirmwareManagerUpdateState(),
					OldDeviceUpdateState = lemonbeatUpdateState.ToFirmwareManagerUpdateState()
				});
			}
		}
	}

	public int TimezoneOffset { get; set; }

	public event EventHandler<LemonbeatDeviceInclusionStateChangedEventArgs> DeviceInclusionStateChanged;

	public event EventHandler<DeviceConfiguredEventArgs> DeviceConfiguredStateChanged;

	public event EventHandler<DeviceUpdateStateChangedEventArgs> DeviceUpdateStateChanged;

	public event EventHandler<DeviceReachabilityChangedEventArgs> DeviceReachabilityChanged;

	public DeviceInformation()
	{
	}

	public DeviceInformation(Guid deviceId, DeviceIdentifier identifier, DeviceDescription deviceDescription, DateTime deviceFound)
		: this(deviceId, identifier, deviceDescription, deviceFound, isReachable: true)
	{
	}

	internal DeviceInformation(Guid deviceId, DeviceIdentifier identifier, DeviceDescription deviceDescription, DateTime deviceFound, bool isReachable)
	{
		DeviceId = deviceId;
		Identifier = identifier;
		DeviceDescription = deviceDescription;
		DeviceFound = deviceFound;
		this.isReachable = (isReachable ? 1 : 0);
		DeviceUpdateStateTimestamp = DateTime.UtcNow;
		IsReachableTimestamp = DateTime.UtcNow;
	}

	public override string ToString()
	{
		try
		{
			if (DeviceDescription != null)
			{
				string text = ((DeviceDescription.SGTIN != null) ? RweSerialNumberStrategy.CreateSerialNumberFromSgtin(DeviceDescription.SGTIN) : "unknown");
				return $"[Lemonbeat device Vendor/Product: {DeviceDescription.ManufacturerId}/{DeviceDescription.ManufacturerProductId} with ID {DeviceId}, serial {text}, address {Identifier.IPAddress.GetAddressBytes().ToReadable()}, firmware: {DeviceDescription.HardwareVersion}]";
			}
			return $"[Unknown Lemonbeat device with ID {DeviceId} and address {Identifier.IPAddress.GetAddressBytes().ToReadable()}]";
		}
		catch
		{
			return $"[Unknown Lemonbeat device with ID {DeviceId} and address {((Identifier != null) ? Identifier.IPAddress : IPAddress.IPv6None)}]";
		}
	}
}
