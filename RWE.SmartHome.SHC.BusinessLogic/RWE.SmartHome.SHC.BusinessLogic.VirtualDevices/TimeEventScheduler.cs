using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Rules;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.ConfigurationTransformation.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.CoreEvents;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogic.VirtualDevices;

public class TimeEventScheduler
{
	private readonly IScheduler scheduler;

	private readonly IEventManager eventManager;

	private readonly IRulesRepository rulesRepository;

	private readonly List<Guid> registeredTaskIds = new List<Guid>();

	private readonly SchedulerTaskBuilder schedulerTaskBuilder;

	private readonly object syncRoot = new object();

	public TimeEventScheduler(IRulesRepository rulesRepository, IEventManager eventManager, IScheduler scheduler)
	{
		this.rulesRepository = rulesRepository;
		this.eventManager = eventManager;
		this.scheduler = scheduler;
		schedulerTaskBuilder = new SchedulerTaskBuilder();
		eventManager.GetEvent<ConfigurationProcessedEvent>().Subscribe(OnConfigurationChanged, (ConfigurationProcessedEventArgs args) => args.ConfigurationPhase == ConfigurationProcessedPhase.UINotified, ThreadOption.PublisherThread, null);
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(OnShcStartup, (ShcStartupCompletedEventArgs a) => a.Progress == StartupProgress.CompletedRound2, ThreadOption.PublisherThread, null);
	}

	private void OnShcStartup(ShcStartupCompletedEventArgs args)
	{
		lock (syncRoot)
		{
			ScheduleTimeEvents();
		}
	}

	private void OnConfigurationChanged(ConfigurationProcessedEventArgs args)
	{
		lock (syncRoot)
		{
			ScheduleTimeEvents();
		}
	}

	private void ScheduleTimeEvents()
	{
		CleanupTaskList();
		List<CustomTrigger> source = rulesRepository.Rules.Where((Rule rule) => rule.CustomTriggers != null).SelectMany((Rule rule) => rule.CustomTriggers).ToList();
		source = (from ct in source
			group ct by ct.Id into @group
			select @group.First()).ToList();
		IEnumerable<SchedulerTaskBase> enumerable = source.SelectMany((CustomTrigger ct) => schedulerTaskBuilder.CreateTasks(ct, TriggerTimeEvent));
		foreach (SchedulerTaskBase item in enumerable)
		{
			scheduler.AddSchedulerTask(item);
			registeredTaskIds.Add(item.TaskId);
		}
	}

	private void CleanupTaskList()
	{
		RemoveTasks(registeredTaskIds);
		registeredTaskIds.Clear();
	}

	private void RemoveTasks(IEnumerable<Guid> tasksIds)
	{
		foreach (Guid tasksId in tasksIds)
		{
			scheduler.RemoveSchedulerTask(tasksId);
		}
	}

	private void TriggerTimeEvent(Guid customTriggerId)
	{
		try
		{
			eventManager.GetEvent<CustomTriggerEvent>().Publish(new CustomTriggerEventArgs(customTriggerId));
		}
		catch (Exception ex)
		{
			Log.Debug(Module.BusinessLogic, string.Format("An error occurred while publishing time event: {0}. {1}" + ex.Message, ex.StackTrace));
		}
	}
}
