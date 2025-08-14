using System;
using RWE.SmartHome.SHC.CommonFunctionality;
using RWE.SmartHome.SHC.Core;
using RWE.SmartHome.SHC.Core.Logging;
using RWE.SmartHome.SHC.Core.Scheduler.Tasks;

namespace RWE.SmartHome.SHC.BusinessLogic.LocalDeviceKeys;

public class DevicesKeysScheduler
{
	private const string SoftwareUpdateDownloadOnlyStartTimeDefaultValue = "10:00";

	private const string SoftwareUpdateDownloadOnlyEndTimeDefaultValue = "15:00";

	private const int SendEmailReminderInNumberOfDays = 14;

	protected FixedTimeSchedulerTask CreateTask(Action taskAction, Configuration configuration, string reason)
	{
		TimeSpan timeOfDay = ChoosePollingTime(ConfigurationParser.TryGetStringValueFromPersistence(configuration.SoftwareUpdateDownloadOnlyStartTime, "10:00"), ConfigurationParser.TryGetStringValueFromPersistence(configuration.SoftwareUpdateDownloadOnlyEndTime, "15:00"), reason);
		return new FixedTimeSchedulerTask(Guid.NewGuid(), taskAction, timeOfDay);
	}

	protected TwoWeeksSchedulerTask CreateTwoWeeksTask(Action taskAction, Configuration configuration, string reason)
	{
		TimeSpan timeOfDay = ChoosePollingTime(ConfigurationParser.TryGetStringValueFromPersistence(configuration.SoftwareUpdateDownloadOnlyStartTime, "10:00"), ConfigurationParser.TryGetStringValueFromPersistence(configuration.SoftwareUpdateDownloadOnlyEndTime, "15:00"), reason);
		return new TwoWeeksSchedulerTask(Guid.NewGuid(), taskAction, timeOfDay, ConfigurationParser.TryGetIntValueFromPersistence(configuration.SendEmailReminderInNumberOfDays, 14));
	}

	private TimeSpan ChoosePollingTime(string windowStartTime, string windowEndTime, string reason)
	{
		TimeSpan result = RandomTimeGenerator.GenerateTimeBetween(windowStartTime, windowEndTime);
		Log.Information(Module.BusinessLogic, $"The SHC will automatically try at {result.Hours:D2}:{result.Minutes:D2} to {reason}");
		return result;
	}
}
