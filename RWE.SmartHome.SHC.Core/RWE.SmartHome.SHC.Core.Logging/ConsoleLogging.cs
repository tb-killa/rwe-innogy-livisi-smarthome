using System;
using Microsoft.Practices.Mobile.ContainerModel;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Logging;
using RWE.SmartHome.SHC.Core.Logging.Entities;

namespace RWE.SmartHome.SHC.Core.Logging;

public class ConsoleLogging : Dispatcher, IService
{
	private enum Color
	{
		Reset = 0,
		Black = 30,
		Red = 31,
		Green = 32,
		Yellow = 33,
		Blue = 34,
		Magenta = 35,
		Cyan = 36,
		White = 37
	}

	private readonly IEventManager eventManager;

	private SubscriptionToken subscriptionTokenAsync;

	private SubscriptionToken subscriptionTokenSync;

	private bool running;

	public ConsoleLogging(Container container)
	{
		running = false;
		eventManager = container.Resolve<IEventManager>();
	}

	public void Initialize()
	{
		LogEvent logEvent = eventManager.GetEvent<LogEvent>();
		if (subscriptionTokenAsync == null)
		{
			subscriptionTokenAsync = logEvent.Subscribe(InternalLog, (LogEventArgs p) => !p.IsSynchronous, ThreadOption.SubscriberThread, this);
		}
		if (subscriptionTokenSync == null)
		{
			subscriptionTokenSync = logEvent.Subscribe(InternalLog, (LogEventArgs p) => p.IsSynchronous, ThreadOption.PublisherThread, this);
		}
	}

	public void Uninitialize()
	{
		LogEvent logEvent = eventManager.GetEvent<LogEvent>();
		if (subscriptionTokenSync != null)
		{
			logEvent.Unsubscribe(subscriptionTokenSync);
			subscriptionTokenSync = null;
		}
		if (subscriptionTokenAsync != null)
		{
			logEvent.Unsubscribe(subscriptionTokenAsync);
			subscriptionTokenAsync = null;
		}
	}

	public override void Start()
	{
		if (!running)
		{
			running = true;
			Initialize();
			base.Start();
			Log.Information(Module.Logging, "ConsoleLogging service started.", isPersisted: false);
		}
	}

	public override void Stop()
	{
		if (running)
		{
			running = false;
			Uninitialize();
			base.Stop();
			Log.Information(Module.Logging, "ConsoleLogging service stopped.", isPersisted: false);
		}
	}

	private static void InternalLog(LogEventArgs args)
	{
		LogEntry logEntry = new LogEntry(args.Module, args.Source, args.LogLevel, args.Message, args.Timestamp);
		Color colorForLogLevel = GetColorForLogLevel(args.LogLevel);
		Console.WriteLine($"{$"\u001b[{(byte)colorForLogLevel}m"}SHC:{logEntry.Timestamp:u}: {logEntry.LogLevel.ToString().ToUpper()}: {logEntry.Source}: {logEntry.Message} {$"\u001b[{(byte)0}m"}");
	}

	private static Color GetColorForLogLevel(Severity logLevel)
	{
		return logLevel switch
		{
			Severity.Information => Color.Yellow, 
			Severity.Debug => Color.Green, 
			Severity.Warning => Color.Magenta, 
			Severity.Error => Color.Red, 
			_ => throw new ArgumentOutOfRangeException("logLevel", "Given log level has no color pendant"), 
		};
	}
}
