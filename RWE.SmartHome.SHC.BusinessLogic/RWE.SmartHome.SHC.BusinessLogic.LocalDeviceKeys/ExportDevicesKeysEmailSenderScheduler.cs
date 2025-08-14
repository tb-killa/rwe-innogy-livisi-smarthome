using System;
using RWE.SmartHome.Common.ControlNodeSHCContracts;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces;
using RWE.SmartHome.SHC.BackendCommunicationInterfaces.Contracts.NotificationSender;
using RWE.SmartHome.SHC.BusinessLogicInterfaces.LocalDeviceKeys;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Exceptions;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;

namespace RWE.SmartHome.SHC.BusinessLogic.LocalDeviceKeys;

public class ExportDevicesKeysEmailSenderScheduler : DevicesKeysScheduler, IExportDevicesKeysEmailSenderScheduler
{
	private readonly IEventManager eventManager;

	private readonly Configuration configuration;

	private readonly IScheduler scheduler;

	private readonly INotificationServiceClient notificationClient;

	private Guid schedulerExportDevicesKeysEmailTaskId;

	public ExportDevicesKeysEmailSenderScheduler(IEventManager eventManager, Configuration configuration, IScheduler scheduler, INotificationServiceClient notificationClient)
	{
		this.eventManager = eventManager;
		this.configuration = configuration;
		this.scheduler = scheduler;
		this.notificationClient = notificationClient;
	}

	public void ScheduleEmailSending()
	{
		if (schedulerExportDevicesKeysEmailTaskId == Guid.Empty)
		{
			Log.Information(Module.BusinessLogic, "The scheduler for exporting the devices keys is starting");
			TwoWeeksSchedulerTask twoWeeksSchedulerTask = CreateTwoWeeksTask(delegate
			{
				TaskManagerAction();
			}, configuration, "send email reminder for exporting the devices keys");
			scheduler.AddSchedulerTask(twoWeeksSchedulerTask);
			schedulerExportDevicesKeysEmailTaskId = twoWeeksSchedulerTask.TaskId;
		}
	}

	private void TaskManagerAction()
	{
		if (FilePersistence.DevicesKeysExported)
		{
			throw new DeviceKeysAlreadyExportedException();
		}
		Log.Information(Module.BusinessLogic, "The devices keys were not exported,sending the email reminder");
		SystemNotification systemNotification = new SystemNotification();
		systemNotification.ProductId = CoreConstants.CoreAppId;
		systemNotification.Type = "KeyExportDue";
		NotificationResponse notificationResponse = notificationClient.SendSystemNotifications(systemNotification);
		if (notificationResponse.NotificationSendResult != NotificationSendResult.Success)
		{
			Log.Error(Module.BusinessLogic, $"Could not send the email reminder for exporting the devices keyes, {notificationResponse.NotificationSendResult}");
		}
	}
}
