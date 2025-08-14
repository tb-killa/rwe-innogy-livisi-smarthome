using SHCWrapper.Misc;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class FactoryResetHandling
{
	public static FactoryResetRequestedStatus WasFactoryResetRequested(bool checkFactoryResetButton)
	{
		FactoryResetRequestedStatus factoryResetStatus = FilePersistence.FactoryResetStatus;
		if (factoryResetStatus != FactoryResetRequestedStatus.NotRequested)
		{
			return factoryResetStatus;
		}
		if (checkFactoryResetButton && ResetManager.IsFactoryReset())
		{
			RequestFactoryReset(factoryResetButton: true);
			return FactoryResetRequestedStatus.RequestedThroughFactoryResetButton;
		}
		return FactoryResetRequestedStatus.NotRequested;
	}

	public static void RequestFactoryReset()
	{
		RequestFactoryReset(factoryResetButton: false);
	}

	private static void RequestFactoryReset(bool factoryResetButton)
	{
		FilePersistence.FactoryResetStatus = ((!factoryResetButton) ? FactoryResetRequestedStatus.RequestedRemotely : FactoryResetRequestedStatus.RequestedThroughFactoryResetButton);
	}

	public static void UndoFactoryResetRequest()
	{
		FilePersistence.FactoryResetStatus = FactoryResetRequestedStatus.NotRequested;
	}
}
