using System;

namespace SmartHome.SHC.API.Logging;

public interface ILogger
{
	void Debug(string message, params object[] args);

	void Information(string message, params object[] args);

	void Warning(string message, params object[] args);

	void Error(string message, params object[] args);

	void Exception(Exception exception, string message, params object[] args);
}
