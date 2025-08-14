using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;
using RWE.SmartHome.SHC.StartupLogicInterfaces.Events;

namespace RWE.SmartHome.SHC.BusinessLogic;

public class MemoryWatchdog : IMemoryWatchdog
{
	private const string LoggingSource = "MemoryWatchdog";

	private readonly Guid schedulerTaskId;

	private int actualLoad;

	private int callCounter;

	private readonly DateTime startTime;

	private IEventManager eventManager;

	private int threshold = 95;

	public MemoryWatchdog(IScheduler scheduler, IEventManager eventManager, Configuration configFile)
	{
		startTime = DateTime.UtcNow;
		schedulerTaskId = Guid.NewGuid();
		FixedTimeSpanSchedulerTask schedulerTask = new FixedTimeSpanSchedulerTask(schedulerTaskId, MonitorMemoryConsumption, new TimeSpan(0, 0, 0, 30));
		scheduler.AddSchedulerTask(schedulerTask);
		this.eventManager = eventManager;
		if (configFile.MemoryLoadThreshold.HasValue)
		{
			threshold = (int)configFile.MemoryLoadThreshold.Value;
		}
		eventManager.GetEvent<ShcStartupCompletedEvent>().Subscribe(ShcStarted, (ShcStartupCompletedEventArgs args) => args.Progress == StartupProgress.CompletedRound1, ThreadOption.PublisherThread, null);
	}

	private void ShcStarted(ShcStartupCompletedEventArgs args)
	{
		int loadPercentage = PerformanceMonitoring.GetGlobalMemoryStatus().LoadPercentage;
		eventManager.GetEvent<MemoryShortageEvent>().Publish(new MemoryShortageEventArgs
		{
			IsShortage = (loadPercentage > threshold),
			MemoryLoad = loadPercentage,
			IsStartup = true
		});
	}

	private void MonitorMemoryConsumption()
	{
		callCounter++;
		MemoryStatus globalMemoryStatus = PerformanceMonitoring.GetGlobalMemoryStatus();
		string performanceReport = PerformanceMonitoring.GetPerformanceReport();
		if (callCounter > 20)
		{
			callCounter = 0;
			TimeSpan timeSpan = DateTime.UtcNow - startTime;
			string empty = string.Empty;
			empty = ((timeSpan.Days != 0) ? $"{timeSpan.Days}days+{timeSpan.Hours}h" : ((timeSpan.Hours != 0) ? $"{timeSpan.Hours}h" : $"{timeSpan.Minutes}min"));
			Log.Information(Module.BusinessLogic, "MemoryWatchdog", performanceReport + ", uptime: " + empty);
		}
		else
		{
			Log.Debug(Module.BusinessLogic, "MemoryWatchdog", performanceReport);
		}
		if (globalMemoryStatus.LoadPercentage > threshold)
		{
			if (actualLoad != globalMemoryStatus.LoadPercentage)
			{
				Log.Warning(Module.BusinessLogic, "MemoryWatchdog", $"Memory shortage. Available memory {100 - globalMemoryStatus.LoadPercentage}%", isPersisted: true);
				Log.Warning(Module.BusinessLogic, "MemoryWatchdog", PerformanceMonitoring.GetPerformanceReport(), isPersisted: false);
				if (actualLoad <= threshold)
				{
					eventManager.GetEvent<MemoryShortageEvent>().Publish(new MemoryShortageEventArgs
					{
						IsShortage = true,
						MemoryLoad = globalMemoryStatus.LoadPercentage,
						IsStartup = false
					});
				}
			}
		}
		else if (actualLoad > threshold)
		{
			Log.Information(Module.BusinessLogic, "MemoryWatchdog", $"Memory shortage resolved. Available memory {100 - globalMemoryStatus.LoadPercentage}%", isPersisted: true);
			Log.Information(Module.BusinessLogic, "MemoryWatchdog", PerformanceMonitoring.GetPerformanceReport(), isPersisted: false);
			eventManager.GetEvent<MemoryShortageEvent>().Publish(new MemoryShortageEventArgs
			{
				IsShortage = false,
				MemoryLoad = globalMemoryStatus.LoadPercentage,
				IsStartup = false
			});
		}
		actualLoad = globalMemoryStatus.LoadPercentage;
	}
}
