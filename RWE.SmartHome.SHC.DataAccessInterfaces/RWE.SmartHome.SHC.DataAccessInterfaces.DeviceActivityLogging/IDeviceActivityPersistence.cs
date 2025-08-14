using System.Collections.Generic;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.DataAccessInterfaces.DeviceActivityLogging;

public interface IDeviceActivityPersistence : IService
{
	bool AddEntry(DeviceActivityLogEntry newEntry);

	List<DeviceActivityLogEntry> GetAllEvents();

	List<DeviceActivityLogEntry> GetOldestEvents(int n);

	List<DeviceActivityLogEntry> GetPendingEvents();

	void RemoveEntriesById(int fromId, int toId);

	void RemoveEntries();

	void RemoveFromLocalCache(IEnumerable<DeviceActivityLogEntry> entries);

	void PurgeOldestEntries(int n);

	void PurgeAllEntries();

	int GetLogCount();
}
