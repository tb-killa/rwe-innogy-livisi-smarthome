using System;
using System.Collections.Generic;

namespace RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

public interface IFilePersistence
{
	FactoryResetRequestedStatus FactoryResetStatus { get; set; }

	string TimeZoneName { get; set; }

	PendingUpdateTasksStatus PendingUpdateTasks { get; set; }

	UpdatePerformedStatus UpdatePerformedStatus { get; set; }

	int ElevatedLogLevelTimeRemaining { get; set; }

	List<string> ElevatedLogLevelModules { get; set; }

	bool EnableSerialLogging { get; set; }

	List<Guid> ActiveDevices { get; set; }

	bool InteractionFunctionValuesFixed { get; set; }

	LocalUser LocalUser { get; set; }

	bool LocalAccessEnabled { get; set; }

	bool InteractionsVerfied { get; set; }

	bool DevicesKeysExported { get; set; }

	string EmailReminderSendingTime { get; set; }

	EmailSettings EmailSettings { get; set; }

	string ApplicationsTokenHash { get; set; }
}
