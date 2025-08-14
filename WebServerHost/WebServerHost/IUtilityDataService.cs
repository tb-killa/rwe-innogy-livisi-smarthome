using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Notifications;
using RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;
using SmartHome.Common.API.Entities.Data;

namespace WebServerHost;

public interface IUtilityDataService
{
	void HandleUtilityReadNotification(GenericNotification utilityRead);

	Utility GetEnergyData(string meterId, string energyType, DateTime start, DateTime end, int page, int pageSize);

	Utility GetPowerData(string meterId, string powerType, DateTime start, DateTime end, int page, int pageSize);

	Utility GetStorageData(string meterId, DateTime start, DateTime end, int page, int pageSize);

	Survey GetEnergySurvey(string meterId, string energyType);

	Survey GetPowerSurvey(string meterId, string powerType);

	Survey GetStorageSurvey(string meterId);

	void Delete();

	void Delete(UtilityType utilityType, string meterId);
}
