using System;

namespace RWE.SmartHome.SHC.Core.Logging;

public interface IFileLogger : IService
{
	bool ProcessAllLines(Func<string, bool> action);

	void StopLoggingAndMoveTo(string path);

	void RestoreLogFrom(string path);

	void Flush();

	void RestoreLegacyLog(string fileName);
}
