using System;
using System.Reflection;
using System.Text;

namespace SmartHome.Common.Generic.LogManager;

internal class Logger : ILogger
{
	private const string LogMessageHeaderFormat = "{0} {1}\t{2} : ";

	private const string DebugConstructorLogFormat = "[##INSTANTIATE {0}] {1} ";

	private const string DebugEnterMethodLogFormat = "[>>ENTER {0}] {1} ";

	private const string DebugExitMethodLogFormat = "[<<EXIT {0}] {1} ";

	private readonly Type logType;

	public event LogMessageHandler LogMessageEvent;

	public Logger(Type logType)
	{
		this.logType = logType;
	}

	public void LogAs(LogLevel logLevel, object message)
	{
		LogAs(logLevel, message, null);
	}

	public void LogAs(LogLevel logLevel, object message, Exception exception)
	{
		switch (logLevel)
		{
		case LogLevel.Debug:
			Debug(message, exception);
			break;
		case LogLevel.Infomration:
			Info(message, exception);
			break;
		case LogLevel.Warning:
			Warn(message, exception);
			break;
		case LogLevel.Error:
			Error(message, exception);
			break;
		}
	}

	public void Info(object message)
	{
		Info(message, null);
	}

	public void Info(object message, Exception exception)
	{
		InvokeLogMessageEvent(LogLevel.Infomration, message, exception);
	}

	private void InvokeLogMessageEvent(LogLevel logLevel, object message, Exception exception)
	{
		LogMessageHandler logMessageEvent = this.LogMessageEvent;
		if (logMessageEvent != null)
		{
			string logMessage = FormatLogMessage(logLevel, message, exception);
			logMessageEvent(logType, logLevel, logMessage);
		}
	}

	private string FormatLogMessage(LogLevel logLevel, object message, Exception exception)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(message).Append('.');
		if (exception != null)
		{
			stringBuilder.Append(' ').Append(exception);
		}
		return stringBuilder.ToString();
	}

	public void Warn(object message)
	{
		Warn(message, null);
	}

	public void Warn(object message, Exception exception)
	{
		InvokeLogMessageEvent(LogLevel.Warning, message, exception);
	}

	public void Error(object message)
	{
		Error(message, null);
	}

	public void Error(object message, Exception exception)
	{
		InvokeLogMessageEvent(LogLevel.Error, message, exception);
	}

	public void Debug(object message)
	{
		Debug(message, null);
	}

	public void Debug(object message, Exception exception)
	{
		InvokeLogMessageEvent(LogLevel.Debug, message, exception);
	}

	public void DebugConstructor()
	{
		DebugConstructor(null);
	}

	public void DebugConstructor(object optionalMessage)
	{
		if (IsDebugEnabled())
		{
			string message = $"[##INSTANTIATE {logType}] {optionalMessage} ";
			InvokeLogMessageEvent(LogLevel.Debug, message, null);
		}
	}

	public void DebugEnterMethod(string methodName)
	{
		DebugEnterMethod(methodName, null);
	}

	public void DebugEnterMethod(string methodName, string optionalMessage)
	{
		if (IsDebugEnabled())
		{
			string message = $"[>>ENTER {methodName}] {optionalMessage} ";
			InvokeLogMessageEvent(LogLevel.Debug, message, null);
		}
	}

	public void DebugExitMethod(string methodName)
	{
		DebugExitMethod(methodName, null);
	}

	public void DebugExitMethod(string methodName, string optionalMessage)
	{
		if (IsDebugEnabled())
		{
			string message = $"[<<EXIT {methodName}] {optionalMessage} ";
			InvokeLogMessageEvent(LogLevel.Debug, message, null);
		}
	}

	public bool IsDebugEnabled()
	{
		return true;
	}

	public void LogAndThrow<T>(string message) where T : Exception
	{
		Warn(message);
		ConstructorInfo constructor = typeof(T).GetConstructor(new Type[1] { typeof(string) });
		T val = (T)constructor.Invoke(new object[1] { message });
		throw val;
	}
}
