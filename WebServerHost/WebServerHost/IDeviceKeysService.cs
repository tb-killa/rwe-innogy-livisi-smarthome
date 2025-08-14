namespace WebServerHost;

public interface IDeviceKeysService
{
	bool GetMasterKeyStatus();

	int GetNumberOfDeviceKeys();

	bool IsExportOfDeviceKeysNeeded();
}
