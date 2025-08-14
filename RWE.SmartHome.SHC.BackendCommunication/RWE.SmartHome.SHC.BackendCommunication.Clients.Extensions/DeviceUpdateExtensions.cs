using System;
using RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate;

namespace RWE.SmartHome.SHC.BackendCommunication.Clients.Extensions;

internal static class DeviceUpdateExtensions
{
	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateInfo ToShcDeviceUpdateInfo(this RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope.DeviceUpdateInfo updateInfo)
	{
		try
		{
			RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateInfo deviceUpdateInfo = new RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateInfo();
			deviceUpdateInfo.ImageUrl = updateInfo.ImageUrl;
			deviceUpdateInfo.ReleaseNotesLocation = updateInfo.ReleaseNotesLocation;
			deviceUpdateInfo.UpdateType = (RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdateType)updateInfo.UpdateType;
			deviceUpdateInfo.MD5Hash = updateInfo.ImageChecksum;
			deviceUpdateInfo.Version = updateInfo.VersionNumber;
			return deviceUpdateInfo;
		}
		catch
		{
			return null;
		}
	}

	public static RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope.DeviceDescriptor ToServerDeviceDescriptor(this RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceDescriptor deviceDescriptor)
	{
		try
		{
			RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope.DeviceDescriptor deviceDescriptor2 = new RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope.DeviceDescriptor();
			deviceDescriptor2.AddInVersion = deviceDescriptor.AddInVersion;
			deviceDescriptor2.CurrentFirmwareVersion = deviceDescriptor.FirmwareVersion;
			deviceDescriptor2.HardwareVersion = deviceDescriptor.HardwareVersion;
			deviceDescriptor2.Manufacturer = deviceDescriptor.Manufacturer;
			deviceDescriptor2.ManufacturerSpecified = true;
			deviceDescriptor2.ProductId = (int)deviceDescriptor.ProductId;
			deviceDescriptor2.ProductIdSpecified = true;
			return deviceDescriptor2;
		}
		catch
		{
			return null;
		}
	}

	public static RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateResultCode ToShcDeviceUpdateResultCode(this RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope.DeviceUpdateResultCode code)
	{
		return code switch
		{
			RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope.DeviceUpdateResultCode.AlreadyLatestVersion => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateResultCode.AlreadyLatestVersion, 
			RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope.DeviceUpdateResultCode.Failure => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateResultCode.Failure, 
			RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope.DeviceUpdateResultCode.NewerVersionAvailable => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateResultCode.NewerVersionAvailable, 
			RWE.SmartHome.SHC.BackendCommunication.DeviceUpdateScope.DeviceUpdateResultCode.NotAuthorized => RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.DeviceUpdate.DeviceUpdateResultCode.NotAuthorized, 
			_ => throw new ArgumentOutOfRangeException("code"), 
		};
	}
}
