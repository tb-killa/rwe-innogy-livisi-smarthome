namespace RWE.SmartHome.SHC.ExternalCommandDispatcherInterfaces.Events;

public class OwnerMessageBoxChangedEventArgs
{
	public bool OwnerHasUnreadMessages { get; set; }

	public bool OwnerHasUnreadAlerts { get; set; }
}
