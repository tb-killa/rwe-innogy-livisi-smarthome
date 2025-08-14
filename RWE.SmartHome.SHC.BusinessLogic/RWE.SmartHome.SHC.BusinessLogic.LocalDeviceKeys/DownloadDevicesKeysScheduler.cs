using System;
using System.Collections.Generic;
using System.Linq;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;

namespace RWE.SmartHome.SHC.BusinessLogic.LocalDeviceKeys;

public class DownloadDevicesKeysScheduler : DevicesKeysScheduler, IDownloadDevicesKeysScheduler
{
	private readonly IEventManager eventManager;

	private readonly Configuration configuration;

	private readonly IScheduler scheduler;

	private readonly List<byte[]> devicesToDownload;

	private Guid schedulerDownloadDevicesKeysTaskId;

	public DownloadDevicesKeysScheduler(IEventManager eventManager, Configuration configuration, IScheduler scheduler)
	{
		this.eventManager = eventManager;
		this.configuration = configuration;
		this.scheduler = scheduler;
		devicesToDownload = new List<byte[]>();
	}

	public void AddDevicesToDownloadLaterInScheduler(List<byte[]> devices)
	{
		Log.Information(Module.BusinessLogic, "Trying to add devices keys to download later in the scheduler");
		foreach (byte[] device in devices)
		{
			if (!devicesToDownload.Contains(device))
			{
				devicesToDownload.Add(device);
			}
		}
		ScheduleDownloadDevicesKeysTask();
	}

	public void RemoveDevicesToDownloadLaterFromScheduler(List<byte[]> devices)
	{
		if (schedulerDownloadDevicesKeysTaskId == Guid.Empty)
		{
			Log.Information(Module.BusinessLogic, "The scheduler for downloading the devices keys is not initialized");
			return;
		}
		Log.Information(Module.BusinessLogic, "Removing the newly downloaded devices keys from the scheduler");
		foreach (byte[] device in devices)
		{
			if (devicesToDownload.Contains(device))
			{
				devicesToDownload.Remove(device);
			}
		}
		if (!devicesToDownload.Any())
		{
			StopDownloadDevicesKeysTask();
		}
	}

	private void ScheduleDownloadDevicesKeysTask()
	{
		if (schedulerDownloadDevicesKeysTaskId == Guid.Empty)
		{
			Log.Information(Module.BusinessLogic, "The scheduler for downloading the devices keys is starting");
			FixedTimeSchedulerTask fixedTimeSchedulerTask = CreateTask(delegate
			{
				TaskManagerAction();
			}, configuration, "download the devices keys");
			scheduler.AddSchedulerTask(fixedTimeSchedulerTask);
			schedulerDownloadDevicesKeysTaskId = fixedTimeSchedulerTask.TaskId;
		}
	}

	private void StopDownloadDevicesKeysTask()
	{
		Log.Information(Module.BusinessLogic, "Stop the scheduler for downloading the devices keys");
		scheduler.RemoveSchedulerTask(schedulerDownloadDevicesKeysTaskId);
		schedulerDownloadDevicesKeysTaskId = Guid.Empty;
	}

	private void TaskManagerAction()
	{
		eventManager.GetEvent<GetDevicesKeysEvent>().Publish(new GetDevicesKeysEventArgs
		{
			Sgtins = devicesToDownload
		});
	}
}
