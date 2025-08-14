using System;
using System.Collections.Generic;
using RWE.SmartHome.SHC.CommonFunctionality.Implementations;
using RWE.SmartHome.SHC.CommonFunctionality.Interfaces;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class FilePersistence
{
	public class LogEntry
	{
		public byte Severity { get; set; }

		public DateTime Timestamp { get; set; }

		public string Source { get; set; }

		public string Message { get; set; }
	}

	private static IFilePersistence Instance = new FilePersistenceImplementation();

	public static FactoryResetRequestedStatus FactoryResetStatus
	{
		get
		{
			return Instance.FactoryResetStatus;
		}
		set
		{
			Instance.FactoryResetStatus = value;
		}
	}

	public static string TimeZoneName
	{
		get
		{
			return Instance.TimeZoneName;
		}
		set
		{
			Instance.TimeZoneName = value;
		}
	}

	public static PendingUpdateTasksStatus PendingUpdateTasks
	{
		get
		{
			return Instance.PendingUpdateTasks;
		}
		set
		{
			Instance.PendingUpdateTasks = value;
		}
	}

	public static UpdatePerformedStatus UpdatePerformedState
	{
		get
		{
			return Instance.UpdatePerformedStatus;
		}
		set
		{
			Instance.UpdatePerformedStatus = value;
		}
	}

	public static int ElevatedLogLevelTimeRemaining
	{
		get
		{
			return Instance.ElevatedLogLevelTimeRemaining;
		}
		set
		{
			Instance.ElevatedLogLevelTimeRemaining = value;
		}
	}

	public static List<string> ElevatedLogLevelModules
	{
		get
		{
			return Instance.ElevatedLogLevelModules;
		}
		set
		{
			Instance.ElevatedLogLevelModules = value;
		}
	}

	public static bool EnableSerialLogging
	{
		get
		{
			return Instance.EnableSerialLogging;
		}
		set
		{
			Instance.EnableSerialLogging = value;
		}
	}

	public static List<Guid> ActiveDevices
	{
		get
		{
			return Instance.ActiveDevices;
		}
		set
		{
			Instance.ActiveDevices = value;
		}
	}

	public static bool InteractionFunctionValuesFixed
	{
		get
		{
			return Instance.InteractionFunctionValuesFixed;
		}
		set
		{
			Instance.InteractionFunctionValuesFixed = value;
		}
	}

	public static bool InteractionsVerified
	{
		get
		{
			return Instance.InteractionsVerfied;
		}
		set
		{
			Instance.InteractionsVerfied = value;
		}
	}

	public static LocalUser LocalUser
	{
		get
		{
			return Instance.LocalUser;
		}
		set
		{
			Instance.LocalUser = value;
		}
	}

	public static bool LocalAccessEnabled
	{
		get
		{
			return Instance.LocalAccessEnabled;
		}
		set
		{
			Instance.LocalAccessEnabled = value;
		}
	}

	public static bool DevicesKeysExported
	{
		get
		{
			return Instance.DevicesKeysExported;
		}
		set
		{
			Instance.DevicesKeysExported = value;
		}
	}

	public static string EmailReminderSendingTime
	{
		get
		{
			return Instance.EmailReminderSendingTime;
		}
		set
		{
			Instance.EmailReminderSendingTime = value;
		}
	}

	public static EmailSettings EmailSettings
	{
		get
		{
			return Instance.EmailSettings;
		}
		set
		{
			Instance.EmailSettings = value;
		}
	}

	public static string ApplicationsTokenHash
	{
		get
		{
			return Instance.ApplicationsTokenHash;
		}
		set
		{
			Instance.ApplicationsTokenHash = value;
		}
	}

	internal static void OverrideImplementation(IFilePersistence implementation)
	{
		Instance = implementation;
	}
}
