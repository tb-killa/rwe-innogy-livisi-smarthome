using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;

namespace RWE.SmartHome.SHC.Core.Logging;

public static class Log
{
	private static IEventManager events;

	public static void Initialize(IEventManager eventManager)
	{
		events = eventManager;
	}

	public static void Debug(Module module, string message)
	{
		Debug(module, string.Empty, message);
	}

	public static void Debug(Module module, Func<string> logAction)
	{
		if (0 >= (int)ModuleInfos.GetLogLevel(module))
		{
			Debug(module, logAction());
		}
	}

	public static void Debug(Module module, string source, string message)
	{
		Debug(module, source, message, isPersisted: true);
	}

	public static void Debug(Module module, string message, bool isPersisted)
	{
		Debug(module, string.Empty, message, isPersisted);
	}

	public static void Debug(Module module, string source, string message, bool isPersisted)
	{
		RaiseLogEvent(Severity.Debug, module, source, message, isPersisted);
	}

	public static void DebugFormat(Module module, string source, bool isPersisted, string message, params object[] args)
	{
		RaiseLogEvent(Severity.Debug, module, source, isPersisted, () => string.Format(message, args));
	}

	public static void Information(Module module, string message)
	{
		Information(module, string.Empty, message);
	}

	public static void Information(Module module, string source, string message)
	{
		Information(module, source, message, isPersisted: true);
	}

	public static void Information(Module module, string message, bool isPersisted)
	{
		Information(module, string.Empty, message, isPersisted);
	}

	public static void Information(Module module, string source, string message, bool isPersisted)
	{
		RaiseLogEvent(Severity.Information, module, source, message, isPersisted);
	}

	public static void InformationFormat(Module module, string source, bool isPersisted, string message, params object[] args)
	{
		RaiseLogEvent(Severity.Information, module, source, isPersisted, () => string.Format(message, args));
	}

	public static void Warning(Module module, string message)
	{
		Warning(module, string.Empty, message);
	}

	public static void Warning(Module module, string source, string message)
	{
		Warning(module, source, message, isPersisted: true);
	}

	public static void Warning(Module module, string message, bool isPersisted)
	{
		Warning(module, string.Empty, message, isPersisted);
	}

	public static void Warning(Module module, string source, string message, bool isPersisted)
	{
		RaiseLogEvent(Severity.Warning, module, source, message, isPersisted);
	}

	public static void WarningFormat(Module module, string source, bool isPersisted, string message, params object[] args)
	{
		RaiseLogEvent(Severity.Warning, module, source, isPersisted, () => string.Format(message, args));
	}

	public static void Error(Module module, string message)
	{
		Error(module, string.Empty, message);
	}

	public static void Error(Module module, string source, string message)
	{
		Error(module, source, message, isPersisted: true);
	}

	public static void Error(Module module, string message, bool isPersisted)
	{
		Error(module, string.Empty, message, isPersisted);
	}

	public static void Error(Module module, string source, string message, bool isPersisted)
	{
		RaiseLogEvent(Severity.Error, module, source, message, isPersisted);
	}

	public static void ErrorFormat(Module module, string source, bool isPersisted, string message, params object[] args)
	{
		RaiseLogEvent(Severity.Error, module, source, isPersisted, () => string.Format(message, args));
	}

	private static void RaiseLogEvent(Severity logLevel, Module module, string source, string message, bool isPersisted)
	{
		if (events != null && (int)logLevel >= (int)ModuleInfos.GetLogLevel(module))
		{
			events.GetEvent<LogEvent>().Publish(new LogEventArgs
			{
				Module = module,
				Source = source,
				LogLevel = logLevel,
				Message = message,
				IsPersisted = isPersisted,
				IsSynchronous = false
			});
		}
	}

	public static void Exception(Module module, string source, Exception ex, string message, params object[] args)
	{
		Exception(module, source, isPersisted: false, ex, message, args);
	}

	public static void Exception(Module module, Exception ex, string message, params object[] args)
	{
		Exception(module, string.Empty, isPersisted: false, ex, message, args);
	}

	public static void Exception(Module module, bool isPersisted, Exception ex, string message, params object[] args)
	{
		Exception(module, string.Empty, isPersisted, ex, message, args);
	}

	public static void Exception(Module module, string source, bool isPersisted, Exception ex, string message, params object[] args)
	{
		string exceptionStr = ((ModuleInfos.GetLogLevel(module) == Severity.Debug) ? ex.ToString() : ex.Message);
		Func<string> message2 = () => $"Exception encountered:\n{string.Format(message, args)}\n{exceptionStr}";
		RaiseLogEvent(Severity.Error, module, source, isPersisted, message2);
	}

	private static void RaiseLogEvent(Severity logLevel, Module module, string source, bool isPersisted, Func<string> message)
	{
		try
		{
			if (events != null && (int)logLevel >= (int)ModuleInfos.GetLogLevel(module))
			{
				events.GetEvent<LogEvent>().Publish(new LogEventArgs
				{
					Module = module,
					Source = source,
					LogLevel = logLevel,
					Message = message(),
					IsPersisted = isPersisted,
					IsSynchronous = false
				});
			}
		}
		catch (Exception ex)
		{
			if (events != null && 48 >= (int)ModuleInfos.GetLogLevel(module))
			{
				string message2 = $"Error logging {logLevel} module: {module} source: {source}\n{ex}";
				events.GetEvent<LogEvent>().Publish(new LogEventArgs
				{
					Module = module,
					Source = source,
					LogLevel = Severity.Error,
					Message = message2,
					IsPersisted = isPersisted,
					IsSynchronous = false
				});
			}
		}
	}
}
