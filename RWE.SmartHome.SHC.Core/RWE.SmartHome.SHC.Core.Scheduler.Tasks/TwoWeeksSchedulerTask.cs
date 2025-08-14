using System;
using System.Globalization;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core.Exceptions;
using RWE.SmartHome.SHC.Core.Logging;

namespace RWE.SmartHome.SHC.Core.Scheduler.Tasks;

public class TwoWeeksSchedulerTask : SchedulerTaskBase
{
	private DateTime nextExecutionTime;

	private readonly int SendEmailReminderInNumberOfDays;

	public override Action TaskAction
	{
		get
		{
			Action action = base.TaskAction;
			return delegate
			{
				try
				{
					action();
					CalculateNextDate();
				}
				catch (DeviceKeysAlreadyExportedException)
				{
				}
				catch (Exception ex2)
				{
					Log.Exception(Module.Core, ex2, "Error occured in executing TwoWeeksSchedulerTask Action");
					CalculateNextDate();
				}
			};
		}
	}

	public TwoWeeksSchedulerTask(Guid id, Action taskAction, TimeSpan timeOfDay, int sendEmailReminderInNumberOfDays)
		: this(id, taskAction, timeOfDay, sendEmailReminderInNumberOfDays, runOnce: false)
	{
	}

	public TwoWeeksSchedulerTask(Guid id, Action taskAction, TimeSpan timeOfDay, int sendEmailReminderInNumberOfDays, bool runOnce)
		: base(id, taskAction, runOnce)
	{
		SendEmailReminderInNumberOfDays = sendEmailReminderInNumberOfDays;
		if (string.IsNullOrEmpty(FilePersistence.EmailReminderSendingTime))
		{
			FilePersistence.EmailReminderSendingTime = new DateTime(ShcDateTime.Now.Year, ShcDateTime.Now.Month, ShcDateTime.Now.Day).Add(timeOfDay).AddDays(SendEmailReminderInNumberOfDays).ToString(CultureInfo.CurrentCulture);
		}
		nextExecutionTime = DateTime.Parse(FilePersistence.EmailReminderSendingTime, CultureInfo.CurrentCulture);
	}

	public override bool ShouldExecute(DateTime now)
	{
		double totalDays = (ShcDateTime.Now - nextExecutionTime).TotalDays;
		if (totalDays >= 0.0)
		{
			return true;
		}
		return false;
	}

	protected void CalculateNextDate()
	{
		DateTime now = ShcDateTime.Now;
		DateTime dateTime = DateTime.Parse(FilePersistence.EmailReminderSendingTime, CultureInfo.CurrentCulture);
		nextExecutionTime = new DateTime(now.Year, now.Month, now.Day, dateTime.Hour, dateTime.Minute, dateTime.Second).AddDays(SendEmailReminderInNumberOfDays);
		FilePersistence.EmailReminderSendingTime = nextExecutionTime.ToString(CultureInfo.CurrentCulture);
	}
}
