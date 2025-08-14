using System.CodeDom.Compiler;

namespace RWE.SmartHome.SHC.BackendCommunication.PublicStorageScope;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public interface IDalStorageService
{
	ShcTableStorageStoreResult StoreDeviceActivityLog(DeviceActivityLog[] dalEntries);

	void PurgeDeviceActivityLog();
}
