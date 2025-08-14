namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;

public interface IDeviceMasterKeyRepository
{
	void StoreMasterKey(string exportKey);

	bool IsMasterExportKeyAlreadyCreated();

	byte[] GetMasterKeyFromFile();
}
