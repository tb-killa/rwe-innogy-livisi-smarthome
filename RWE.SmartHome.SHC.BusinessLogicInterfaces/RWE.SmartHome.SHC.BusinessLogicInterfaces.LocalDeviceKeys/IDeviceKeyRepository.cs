using System.Collections.Generic;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;

public interface IDeviceKeyRepository
{
	List<StoredDevice> GetAllDevicesKeysFromStorage();

	List<StoredDevice> GetAllDevicesKeysFromFile(string filePath);

	void StoreKeys(ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary[] devicesKeys);

	void StoreDeviceKey(ArrayOfKeyValueOfbase64Binarybase64BinaryKeyValueOfbase64Binarybase64Binary deviceKey);

	int GetDeviceKeysCount();

	void ImportDevicesKeys(List<StoredDevice> devicesKeys);

	bool DeviceExistsInStorage(byte[] sgtin);

	List<List<byte[]>> SplitDevices(List<byte[]> devices);

	byte[] GetDeviceKeyFromCsvStorage(SGTIN96 deviceSgtin);
}
