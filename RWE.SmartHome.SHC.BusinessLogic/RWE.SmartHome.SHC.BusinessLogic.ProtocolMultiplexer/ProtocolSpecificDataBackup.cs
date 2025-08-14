using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ProtocolSpecific;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Configuration;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.DataAccessInterfaces.Events;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.ProtocolMultiplexer;

public class ProtocolSpecificDataBackup : Dispatcher, IService, IExecutable, IProtocolSpecificDataBackup
{
	private const string LoggingSource = "ProtocolSpecificDataBackup";

	private readonly Guid schedulerTaskId = Guid.NewGuid();

	private readonly Configuration configuration;

	private readonly IEventManager eventManager;

	private readonly IScheduler scheduler;

	private readonly List<IProtocolSpecificDataBackup> protocolSpecificDataBackupImplementations = new List<IProtocolSpecificDataBackup>();

	private SubscriptionToken shcStartupCompletedEventSubscriptionToken;

	public ProtocolSpecificDataBackup(IEventManager eventManager, IScheduler scheduler, IConfigurationManager configurationManager)
	{
		this.scheduler = scheduler;
		this.eventManager = eventManager;
		configuration = new Configuration(configurationManager);
	}

	public void RegisterProtocolSpecificDataBackup(IProtocolSpecificDataBackup protocolSpecificDataBackup)
	{
		protocolSpecificDataBackupImplementations.Add(protocolSpecificDataBackup);
	}

	public void Initialize()
	{
		if (shcStartupCompletedEventSubscriptionToken == null)
		{
			eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(delegate
			{
				CreateSchedulerTask();
			}, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound1, ThreadOption.SubscriberThread, this);
		}
		Start();
	}

	private void TriggerProtocolSpecificDataPersistence()
	{
		Dispatch(this);
	}

	private void CreateSchedulerTask()
	{
		FixedTimeSchedulerTask schedulerTask = new FixedTimeSchedulerTask(schedulerTaskId, TriggerProtocolSpecificDataPersistence, ChoosePollingTime());
		scheduler.AddSchedulerTask(schedulerTask);
	}

	private TimeSpan ChoosePollingTime()
	{
		string protocolSpecificDataPersistenceStartTime = configuration.ProtocolSpecificDataPersistenceStartTime;
		string protocolSpecificDataPersistenceStopTime = configuration.ProtocolSpecificDataPersistenceStopTime;
		TimeSpan result = RandomTimeGenerator.GenerateTimeBetween(protocolSpecificDataPersistenceStartTime, protocolSpecificDataPersistenceStopTime);
		Log.Information(Module.ProtocolMultiplexer, "ProtocolSpecificDataBackup", $"The SHC will automatically persist all protocol(s) data every day at {result.Hours:D2}:{result.Minutes:D2} ");
		return result;
	}

	public void Uninitialize()
	{
		Stop();
		if (shcStartupCompletedEventSubscriptionToken != null)
		{
			eventManager.GetEvent<ShcStartupCompletedEvent>().Unsubscribe(shcStartupCompletedEventSubscriptionToken);
			shcStartupCompletedEventSubscriptionToken = null;
			scheduler.RemoveSchedulerTask(schedulerTaskId);
		}
	}

	public void Execute()
	{
		Backup();
	}

	public void Backup()
	{
		foreach (IProtocolSpecificDataBackup protocolSpecificDataBackupImplementation in protocolSpecificDataBackupImplementations)
		{
			protocolSpecificDataBackupImplementation.Backup();
		}
		eventManager.GetEvent<ProtocolSpecificDataModifiedEvent>().Publish(new ProtocolSpecificDataModifiedEventArgs());
	}

	public void Restore(bool restoreDefaults)
	{
		foreach (IProtocolSpecificDataBackup protocolSpecificDataBackupImplementation in protocolSpecificDataBackupImplementations)
		{
			protocolSpecificDataBackupImplementation.Restore(restoreDefaults);
		}
	}
}
