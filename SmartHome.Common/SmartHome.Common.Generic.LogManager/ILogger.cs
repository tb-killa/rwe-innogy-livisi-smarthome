using System;

namespace SmartHome.Common.Generic.LogManager;

public interface ILogger
{
	void LogAs(LogLevel logLevel, object message);

	void LogAs(LogLevel logLevel, object message, Exception exception);

	void Info(object message);

	void Info(object message, Exception exception);

	void Warn(object message);

	void Warn(object message, Exception exception);

	void Error(object message);

	void Error(object message, Exception exception);

	void Debug(object message);

	void Debug(object message, Exception exception);

	void DebugConstructor();

	void DebugConstructor(object optionalMessage);

	void DebugEnterMethod(string methodName);

	void DebugEnterMethod(string methodName, string optionalMessage);

	void DebugExitMethod(string methodName);

	void DebugExitMethod(string methodName, string optionalMessage);

	bool IsDebugEnabled();

	void LogAndThrow<T>(string message) where T : Exception;
}
