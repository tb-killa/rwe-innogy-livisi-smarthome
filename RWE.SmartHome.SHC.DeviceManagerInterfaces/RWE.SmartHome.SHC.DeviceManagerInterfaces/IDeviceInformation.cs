using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Events;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceFirmwareUpdate;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Events;
using SipcosCommandHandler;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces;

public interface IDeviceInformation : IBasicDeviceInformation
{
	byte ManufacturerDeviceAndFirmware { get; set; }

	short ManufacturerCode { get; set; }

	uint ManufacturerDeviceType { get; set; }

	DateTime DeviceFound { get; set; }

	DateTime DeviceExclusionTime { get; set; }

	DeviceStatusInfo StatusInfo { get; set; }

	byte[] Sgtin { get; }

	DateTime AwakeUntil { get; set; }

	byte AllOperationModes { get; }

	NetworkAcceptFrame? PreparedNetworkAcceptFrame { get; set; }

	byte[] LastCollisionAddress { get; set; }

	int CollisionFrameCount { get; set; }

	byte[] RouterAddress { get; set; }

	bool IsRoutedInclusion { get; set; }

	bool SupportsEncryption { get; set; }

	byte[] Nonce { get; set; }

	CosIPDeviceUpdateState UpdateState { get; set; }

	int? PendingVersionNumber { get; set; }

	DateTime? DeviceUnreachableTimestamp { get; }

	DateTime? DeviceInclusionStateTimestamp { get; }

	DateTime? UpdateStateTimestamp { get; }

	DateTime? DeviceConfigurationStateTimestamp { get; }

	event EventHandler<DeviceInclusionStateChangedEventArgs> DeviceInclusionStateChanged;

	event EventHandler<DeviceConfiguredEventArgs> DeviceConfiguredStateChanged;

	event EventHandler<DeviceUpdateStateChangedEventArgs> DeviceUpdateStateChanged;
}
