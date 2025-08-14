using RWE.SmartHome.SHC.CommonFunctionality;

namespace RWE.SmartHome.SHC.StartupLogic;

public static class PendingUpdateTasks
{
	public static bool DatabaseUpdateNecessary
	{
		get
		{
			return IsFlagSet(PendingUpdateTasksStatus.DatabaseUpdateNecessary);
		}
		set
		{
			SetFlag(value, PendingUpdateTasksStatus.DatabaseUpdateNecessary);
		}
	}

	public static bool RestoreSequenceCounterNecessary
	{
		get
		{
			return IsFlagSet(PendingUpdateTasksStatus.RestoreSequenceCounterNecessary);
		}
		set
		{
			SetFlag(value, PendingUpdateTasksStatus.RestoreSequenceCounterNecessary);
		}
	}

	private static void SetFlag(bool flagValue, PendingUpdateTasksStatus status)
	{
		if (flagValue)
		{
			FilePersistence.PendingUpdateTasks |= status;
		}
		else
		{
			FilePersistence.PendingUpdateTasks &= ~status;
		}
	}

	private static bool IsFlagSet(PendingUpdateTasksStatus flag)
	{
		return (FilePersistence.PendingUpdateTasks & flag) != 0;
	}
}
