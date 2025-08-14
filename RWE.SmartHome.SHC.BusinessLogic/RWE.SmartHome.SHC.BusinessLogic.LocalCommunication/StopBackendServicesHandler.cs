using System;
using System.Globalization;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalCommunication;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.LocalCommunication;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;

namespace RWE.SmartHome.SHC.BusinessLogic.LocalCommunication;

public class StopBackendServicesHandler : IStopBackendServicesHandler
{
	private readonly IEventManager eventManager;

	private Guid schedulerStopBackendServicesTaskId;

	private readonly IScheduler scheduler;

	private readonly IRegistrationService registrationService;

	public StopBackendServicesHandler(IEventManager eventManager, IScheduler scheduler, IRegistrationService registrationService)
	{
		this.eventManager = eventManager;
		this.scheduler = scheduler;
		this.registrationService = registrationService;
	}

	public void ScheduleStoppingBackendServices(string stopBackendRequestsDate)
	{
		string text = ConfigurationParser.TryGetStringValueFromPersistence(stopBackendRequestsDate, "3/1/2024");
		DateTime dateTime;
		try
		{
			dateTime = DateTime.Parse(text, CultureInfo.CurrentCulture);
		}
		catch (Exception ex)
		{
			dateTime = DateTime.Parse("3/1/2024", CultureInfo.CurrentCulture);
			Log.Error(Module.BusinessLogic, string.Format("Cannot parse the string {0} into DateTime, it will be set at it's default value:", text, dateTime.ToString(CultureInfo.CurrentCulture)));
			Log.Error(Module.BusinessLogic, $"{ex.Message} {ex.StackTrace}");
		}
		if ((!(ShcDateTime.Now > dateTime) || !registrationService.IsShcLocalOnly) && schedulerStopBackendServicesTaskId == Guid.Empty)
		{
			Log.Information(Module.BusinessLogic, "The scheduler for stopping the backend services is starting");
			FixDateFromPersistenceSchedulerTask fixDateFromPersistenceSchedulerTask = CreateTask(delegate
			{
				TaskManagerAction();
			}, dateTime);
			scheduler.AddSchedulerTask(fixDateFromPersistenceSchedulerTask);
			schedulerStopBackendServicesTaskId = fixDateFromPersistenceSchedulerTask.TaskId;
		}
	}

	private void TaskManagerAction()
	{
		eventManager.GetEvent<ShcRebootScheduledEvent>().Publish(new ShcRebootScheduledEventArgs());
		eventManager.GetEvent<PerformResetEvent>().Publish(new PerformResetEventArgs());
	}

	protected FixDateFromPersistenceSchedulerTask CreateTask(Action taskAction, DateTime stopBackendRequestsDate)
	{
		Log.Information(Module.BusinessLogic, $"The SHC will automatically stop the backend request on {stopBackendRequestsDate.ToString(CultureInfo.CurrentCulture)}");
		return new FixDateFromPersistenceSchedulerTask(Guid.NewGuid(), taskAction, stopBackendRequestsDate);
	}
}
