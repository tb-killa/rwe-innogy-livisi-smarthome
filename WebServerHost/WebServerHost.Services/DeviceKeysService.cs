using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;

namespace WebServerHost.Services;

public class DeviceKeysService : IDeviceKeysService
{
	private readonly IDeviceMasterKeyRepository masterKeyRepository;

	private readonly IDeviceKeyRepository deviceKeyRepository;

	public DeviceKeysService(IDeviceMasterKeyRepository masterKeyRepository, IDeviceKeyRepository deviceKeyRepository)
	{
		this.masterKeyRepository = masterKeyRepository;
		this.deviceKeyRepository = deviceKeyRepository;
	}

	public bool GetMasterKeyStatus()
	{
		try
		{
			return masterKeyRepository.IsMasterExportKeyAlreadyCreated();
		}
		catch (Exception ex)
		{
			Log.Error(Module.WebServerHost, $"There was a problem getting the master key status. {ex.Message} {ex.StackTrace}");
			return false;
		}
	}

	public int GetNumberOfDeviceKeys()
	{
		try
		{
			return deviceKeyRepository.GetDeviceKeysCount();
		}
		catch (Exception ex)
		{
			Log.Error(Module.WebServerHost, $"There was a problem getting the device keys number. {ex.Message} {ex.StackTrace}");
			return 0;
		}
	}

	public bool IsExportOfDeviceKeysNeeded()
	{
		try
		{
			return !FilePersistence.DevicesKeysExported;
		}
		catch (Exception ex)
		{
			Log.Error(Module.WebServerHost, $"There was a problem getting the devices keys exported flag from the file persistence. {ex.Message} {ex.StackTrace}");
			return true;
		}
	}
}
