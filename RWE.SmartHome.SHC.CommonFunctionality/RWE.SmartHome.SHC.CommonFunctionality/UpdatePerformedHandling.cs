namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class UpdatePerformedHandling
{
	public static UpdatePerformedStatus WasUpdatePerformed()
	{
		return FilePersistence.UpdatePerformedState;
	}

	public static void SetUpdatePerformedState(UpdatePerformedStatus flag, bool flush)
	{
		if (flush)
		{
			FilePersistence.UpdatePerformedState = flag;
		}
		else
		{
			FilePersistence.UpdatePerformedState |= flag;
		}
	}

	public static void RemoveUpdatePerformedState(UpdatePerformedStatus flag)
	{
		FilePersistence.UpdatePerformedState &= ~flag;
	}

	public static void ReleaseUpdatePerformedState()
	{
		FilePersistence.UpdatePerformedState = (UpdatePerformedStatus)0;
	}
}
