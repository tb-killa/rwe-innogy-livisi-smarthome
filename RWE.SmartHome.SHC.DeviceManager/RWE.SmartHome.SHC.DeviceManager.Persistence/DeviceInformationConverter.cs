using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolMultiplexer.Enums;
using RWE.SmartHome.SHC.DeviceManagerInterfaces;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceFirmwareUpdate;
using RWE.SmartHome.SHC.DeviceManagerInterfaces.Persistence;

namespace RWE.SmartHome.SHC.DeviceManager.Persistence;

public static class DeviceInformationConverter
{
	public static DeviceInformationEntity ConvertToDeviceInformationEntity(IDeviceInformation deviceInformation)
	{
		DeviceInformationEntity deviceInformationEntity = new DeviceInformationEntity();
		deviceInformationEntity.Address = deviceInformation.Address;
		deviceInformationEntity.DeviceId = deviceInformation.DeviceId;
		deviceInformationEntity.DeviceInclusionState = (int)deviceInformation.DeviceInclusionState;
		deviceInformationEntity.DeviceNotReachable = deviceInformation.DeviceUnreachable;
		deviceInformationEntity.ManufacturerCode = (byte)deviceInformation.ManufacturerCode;
		deviceInformationEntity.ManufacturerDeviceAndFirmware = deviceInformation.ManufacturerDeviceAndFirmware;
		deviceInformationEntity.ManufacturerDeviceType = deviceInformation.ManufacturerDeviceType;
		deviceInformationEntity.OperationModes = deviceInformation.AllOperationModes;
		deviceInformationEntity.SGTIN = deviceInformation.Sgtin;
		deviceInformationEntity.ProtocolType = (byte)deviceInformation.ProtocolType;
		deviceInformationEntity.UpdateState = (int)deviceInformation.UpdateState;
		deviceInformationEntity.PendingVersionNumber = deviceInformation.PendingVersionNumber;
		deviceInformationEntity.DeviceExclusionTime = deviceInformation.DeviceExclusionTime;
		deviceInformationEntity.IsRoutedInclusion = deviceInformation.IsRoutedInclusion;
		return deviceInformationEntity;
	}

	public static DeviceInformation ConvertToDeviceInformation(DeviceInformationEntity entity)
	{
		DeviceInformation deviceInformation = new DeviceInformation(entity.DeviceId, (DeviceInclusionState)entity.DeviceInclusionState, entity.Address, byte.MaxValue, entity.SGTIN, entity.OperationModes, entity.ManufacturerDeviceAndFirmware, entity.ManufacturerCode, entity.ManufacturerDeviceType, (ProtocolType)entity.ProtocolType, entity.IsRoutedInclusion);
		deviceInformation.DeviceUnreachable = entity.DeviceNotReachable;
		deviceInformation.UpdateState = (CosIPDeviceUpdateState)entity.UpdateState;
		deviceInformation.PendingVersionNumber = entity.PendingVersionNumber;
		deviceInformation.DeviceExclusionTime = entity.DeviceExclusionTime;
		return deviceInformation;
	}
}
