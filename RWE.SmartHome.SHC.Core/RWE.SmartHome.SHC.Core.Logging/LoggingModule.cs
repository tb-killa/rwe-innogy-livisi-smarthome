using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.Core.Logging;

public class LoggingModule : IModule
{
	private Container containerAccess;

	public void Configure(Container container)
	{
		containerAccess = container;
		IEventManager eventManager = container.Resolve<IEventManager>();
		Log.Initialize(eventManager);
		SyncLog.Initialize(eventManager);
		if (FilePersistence.EnableSerialLogging)
		{
			RegisterSerialLogging();
		}
		StartFileLogging();
	}

	private void RegisterSerialLogging()
	{
		try
		{
			containerAccess.Register("ConsoleLogging", (Func<Container, IService>)delegate(Container c)
			{
				ConsoleLogging consoleLogging = new ConsoleLogging(c);
				c.Resolve<ITaskManager>().Register(consoleLogging);
				return consoleLogging;
			}).InitializedBy(delegate(Container c, IService v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
			_ = (ConsoleLogging)containerAccess.ResolveNamed<IService>("ConsoleLogging");
			Log.Information(Module.Logging, "Initialization of ConsoleLogging successful");
		}
		catch (Exception ex)
		{
			Log.Error(Module.Logging, $"Initialization of ConsoleLogging failed: {ex.Message}");
		}
	}

	private void StopSerialLogging()
	{
		try
		{
			((ConsoleLogging)containerAccess.ResolveNamed<IService>("ConsoleLogging"))?.Stop();
		}
		catch (Exception ex)
		{
			Log.Error(Module.Logging, $"Stopping of ConsoleLogging failed: {ex.Message}");
		}
	}

	private void StartFileLogging()
	{
		try
		{
			containerAccess.Register((Func<Container, IFileLogger>)delegate(Container c)
			{
				FileLogger fileLogger = new FileLogger(c);
				c.Resolve<ITaskManager>().Register(fileLogger);
				return fileLogger;
			}).InitializedBy(delegate(Container c, IFileLogger v)
			{
				v.Initialize();
			}).ReusedWithin(ReuseScope.Container);
		}
		catch (Exception arg)
		{
			Log.Error(Module.Logging, $"Initialization of FileLogging failed: {arg}");
		}
	}
}
