using System;

namespace RWE.SmartHome.SHC.DeviceManagerInterfaces.DeviceHandler;

public interface ISipcosConfigurator
{
	Guid CreateLink(byte[] targetDeviceAddress, byte targetChannel, byte[] partnerDeviceAddress, byte partnerChannel, byte operationMode);

	Guid RemoveLink(byte[] targetDeviceAddress, byte targetChannel, byte[] partnerDeviceAddress, byte partnerChannel);

	Guid ConfigureLink(byte[] targetDeviceAddress, byte targetChannel, byte[] partnerDeviceAddress, byte partnerChannel, byte parameterListNo, byte[] parameters, byte operationMode, bool withCreate);

	void FlushToSendQueue();
}
