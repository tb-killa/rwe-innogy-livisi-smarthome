using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;

namespace RWE.SmartHome.SHC.Core.Logging;

public static class SyncLog
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
				IsSynchronous = true
			});
		}
	}
}
