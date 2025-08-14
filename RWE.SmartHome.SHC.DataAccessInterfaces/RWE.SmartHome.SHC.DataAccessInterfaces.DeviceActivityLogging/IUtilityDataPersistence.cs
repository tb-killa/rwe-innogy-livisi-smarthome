using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;

public interface IUtilityDataPersistence : IService
{
	void Add(UtilityData data);

	List<UtilityData> Get(UtilityType utilityType, string meterId, DateTime start, DateTime end, int offset, int limit, string dataPattern);

	Survey GetSurvey(UtilityType utilityType, string meterId, string dataPattern);

	void Delete();

	void Delete(UtilityType utilityType, string meterId);
}
