using System.Collections.Generic;
using RWE.SmartHome.SHC.Core.Logging.Entities;

namespace RWE.SmartHome.SHC.Core.Logging;

public interface IDataAccess : IService
{
	IEnumerable<LogEntry> GetLogEntries();

	void SaveLogEntry(LogEntry logEntry);

	void DeleteLogEntry(int id);

	void PurgeOldEntries(int count);
}
