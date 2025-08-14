namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class OSStateChangedEventArgs
{
	public OSState NewOSState { get; private set; }

	public OSStateChangedEventArgs(OSState newState)
	{
		NewOSState = newState;
	}
}
