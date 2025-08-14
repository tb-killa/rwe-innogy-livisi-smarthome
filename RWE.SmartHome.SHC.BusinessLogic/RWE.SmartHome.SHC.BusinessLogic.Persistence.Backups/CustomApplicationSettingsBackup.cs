using System;
using RWE.SmartHome.SHC.BusinessLogicInterfaces;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;

namespace RWE.SmartHome.SHC.BusinessLogic.Persistence.Backups;

public class CustomApplicationSettingsBackup
{
	private readonly IBackendPersistence backendPersistence;

	private readonly IRandomNumberGenerator generator;

	private readonly IScheduler scheduler;

	public CustomApplicationSettingsBackup(IBackendPersistence backendPersistence, IRandomNumberGenerator generator, IScheduler scheduler)
	{
		this.backendPersistence = backendPersistence;
		this.generator = generator;
		this.scheduler = scheduler;
		ScheduleBackup();
	}

	private void ScheduleBackup()
	{
		TimeSpan randomTime = GetRandomTime();
		TimeSpan timeOfDay = DateTime.Now.Date.Add(randomTime).ToUniversalTime().TimeOfDay;
		Log.Information(Module.BusinessLogic, $"Backup for CustomApplicationSettings is scheduled daily at {timeOfDay}");
		FixedTimeSchedulerTask schedulerTask = new FixedTimeSchedulerTask(Guid.NewGuid(), BackupCustomApplicationsSettings, randomTime);
		scheduler.AddSchedulerTask(schedulerTask);
	}

	private void BackupCustomApplicationsSettings()
	{
		BackendPersistenceResult backendPersistenceResult = backendPersistence.BackupCustomApplicationsSettings(null);
		if (backendPersistenceResult == BackendPersistenceResult.Success)
		{
			Log.Information(Module.BusinessLogic, "CustomApplicationSettings was backed up succesfully");
		}
		else
		{
			Log.Information(Module.BusinessLogic, $"CustomApplicationSettings failed to backup with result: {backendPersistenceResult}");
		}
	}

	private TimeSpan GetRandomTime()
	{
		int num = generator.Next(0, 1440);
		return TimeSpan.FromMinutes(num);
	}
}
