using System.Collections.Generic;

namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;

public interface IDownloadDevicesKeysScheduler
{
	void AddDevicesToDownloadLaterInScheduler(List<byte[]> devices);

	void RemoveDevicesToDownloadLaterFromScheduler(List<byte[]> devices);
}
