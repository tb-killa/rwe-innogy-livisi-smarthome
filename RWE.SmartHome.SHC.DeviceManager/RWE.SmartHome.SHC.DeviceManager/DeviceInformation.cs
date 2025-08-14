using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.DeviceManager.PrioritizedQueue;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceFirmwareUpdate;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManager;

public class DeviceInformation : IDeviceInformation, IBasicDeviceInformation
{
	private bool deviceUnreachable;

	private DeviceInclusionState deviceInclusionState;

	private CosIPDeviceUpdateState deviceUpdateState;

	private DeviceConfigurationState deviceConfigurationState;

	public byte ManufacturerDeviceAndFirmware { get; set; }

	public short ManufacturerCode { get; set; }

	public uint ManufacturerDeviceType { get; set; }

	public DateTime DeviceFound { get; set; }

	public DateTime DeviceExclusionTime { get; set; }

	public Guid DeviceId { get; private set; }

	public byte[] Address { get; set; }

	public byte[] RouterAddress { get; set; }

	public bool IsRoutedInclusion { get; set; }

	public bool SupportsEncryption { get; set; }

	public byte SequenceNumber { get; set; }

	public DeviceStatusInfo StatusInfo { get; set; }

	public DateTime? DeviceUnreachableTimestamp { get; private set; }

	public bool DeviceUnreachable
	{
		get
		{
			return deviceUnreachable;
		}
		set
		{
			if (deviceUnreachable != value)
			{
				DeviceUnreachableTimestamp = ShcDateTime.UtcNow;
			}
			deviceUnreachable = value;
		}
	}

	public byte[] Sgtin { get; private set; }

	public byte Rssi { get; set; }

	public DateTime AwakeUntil { get; set; }

	public DeviceInfoOperationModes BestOperationMode { get; private set; }

	public byte AllOperationModes { get; private set; }

	public NetworkAcceptFrame? PreparedNetworkAcceptFrame { get; set; }

	private SortedList<AwakeModifier, TimeSpan> StayAwakeTimes { get; set; }

	public byte[] LastCollisionAddress { get; set; }

	public int CollisionFrameCount { get; set; }

	public byte[] Nonce { get; set; }

	public ProtocolType ProtocolType { get; set; }

	public DateTime? DeviceInclusionStateTimestamp { get; private set; }

	public DeviceInclusionState DeviceInclusionState
	{
		get
		{
			return deviceInclusionState;
		}
		set
		{
			BestOperationMode = DetermineOperationMode(value);
			if (deviceInclusionState != value)
			{
				deviceInclusionState = value;
				DeviceInclusionStateTimestamp = ShcDateTime.UtcNow;
				this.DeviceInclusionStateChanged?.Invoke(this, new DeviceInclusionStateChangedEventArgs(DeviceId, deviceInclusionState, ProtocolType.ToString()));
			}
		}
	}

	public DateTime? UpdateStateTimestamp { get; private set; }

	public CosIPDeviceUpdateState UpdateState
	{
		get
		{
			return deviceUpdateState;
		}
		set
		{
			if (deviceUpdateState != value)
			{
				CosIPDeviceUpdateState cosIPUpdateState = deviceUpdateState;
				deviceUpdateState = value;
				UpdateStateTimestamp = ShcDateTime.UtcNow;
				this.DeviceUpdateStateChanged?.Invoke(this, new DeviceUpdateStateChangedEventArgs
				{
					DeviceId = DeviceId,
					NewDeviceUpdateState = deviceUpdateState.ToFirmwareManagerUpdateState(),
					OldDeviceUpdateState = cosIPUpdateState.ToFirmwareManagerUpdateState(),
					FirmwareVersion = ManufacturerDeviceAndFirmware.ToString("X2").Insert(1, ".")
				});
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

	public int? PendingVersionNumber { get; set; }

	public event EventHandler<DeviceInclusionStateChangedEventArgs> DeviceInclusionStateChanged;

	public event EventHandler<DeviceUpdateStateChangedEventArgs> DeviceUpdateStateChanged;

	public event EventHandler<DeviceConfiguredEventArgs> DeviceConfiguredStateChanged;

	public DeviceInformation(Guid deviceId, DeviceInclusionState inclusionState, byte[] address, byte sequenceNumber, byte[] sgtin, byte operationMode, byte manufacturerDeviceAndFirmware, short manufacturerCode, uint manufacturerDeviceType, ProtocolType protocolType, bool isRoutedInclusion)
	{
		StatusInfo = null;
		ManufacturerDeviceType = manufacturerDeviceType;
		ManufacturerCode = manufacturerCode;
		ManufacturerDeviceAndFirmware = manufacturerDeviceAndFirmware;
		DeviceInclusionState = inclusionState;
		DeviceId = deviceId;
		Address = address;
		SequenceNumber = 0;
		AllOperationModes = operationMode;
		DeviceId = deviceId;
		Sgtin = sgtin;
		ProtocolType = protocolType;
		BestOperationMode = DetermineOperationMode(inclusionState);
		StayAwakeTimes = Defaults.GetAwakeTimes(protocolType, BestOperationMode);
		IsRoutedInclusion = isRoutedInclusion;
		UpdateState = CosIPDeviceUpdateState.UpToDate;
		CollisionFrameCount = 0;
	}

	private DeviceInfoOperationModes DetermineOperationMode(DeviceInclusionState inclusionState)
	{
		DeviceInfoOperationModes deviceInfoOperationModes = DeviceInfoOperationModes.EventListener;
		if ((AllOperationModes & 1) != 0)
		{
			deviceInfoOperationModes = DeviceInfoOperationModes.MainsPowered;
		}
		else if ((AllOperationModes & 4) != 0)
		{
			deviceInfoOperationModes = DeviceInfoOperationModes.BurstListener;
		}
		else if ((AllOperationModes & 8) != 0)
		{
			deviceInfoOperationModes = DeviceInfoOperationModes.TripleBurstListener;
		}
		else if ((AllOperationModes & 0x10) != 0)
		{
			deviceInfoOperationModes = DeviceInfoOperationModes.CyclicListener;
		}
		if (ProtocolType == ProtocolType.BidCos && inclusionState != DeviceInclusionState.Included && inclusionState != DeviceInclusionState.ExclusionPending && (deviceInfoOperationModes == DeviceInfoOperationModes.BurstListener || deviceInfoOperationModes == DeviceInfoOperationModes.TripleBurstListener))
		{
			deviceInfoOperationModes = DeviceInfoOperationModes.EventListener;
		}
		return deviceInfoOperationModes;
	}

	public void UpdateAwakeState(AwakeModifier awakeModifier)
	{
		bool flag = StayAwakeTimes[awakeModifier] == TimeSpan.MaxValue;
		AwakeUntil = (flag ? DateTime.MaxValue : DateTime.UtcNow.Add(StayAwakeTimes[awakeModifier]));
	}

	public void MarkDeviceAsSleeping()
	{
		if (BestOperationMode != DeviceInfoOperationModes.MainsPowered)
		{
			AwakeUntil = DateTime.MinValue;
		}
	}

	public bool Awake()
	{
		if (StayAwakeTimes[AwakeModifier.None] == TimeSpan.MaxValue)
		{
			return true;
		}
		return AwakeUntil > DateTime.UtcNow;
	}

	public override string ToString()
	{
		return CreateInfoString(ManufacturerCode, ManufacturerDeviceType, Sgtin, Address, ManufacturerDeviceAndFirmware);
	}

	private static string DebugDeviceType(short manufacturerCode, uint manufacturerDeviceType)
	{
		if (manufacturerCode == 1)
		{
			return ((DeviceTypesEq3)manufacturerDeviceType).ToString();
		}
		return "Unknown";
	}

	public static string CreateInfoString(short manufaturerCode, uint manufacturerDeviceType, byte[] sgtin, byte[] address, byte firmware)
	{
		return $"[{DebugDeviceType(manufaturerCode, manufacturerDeviceType)} with serial {SerialForDisplay.FromSgtin(sgtin)}, address {address.ToReadable()} and firmware: {firmware:X2}]";
	}

	public bool IsDeviceSleeping()
	{
		if (AwakeUntil == DateTime.MinValue)
		{
			return true;
		}
		return false;
	}
}
