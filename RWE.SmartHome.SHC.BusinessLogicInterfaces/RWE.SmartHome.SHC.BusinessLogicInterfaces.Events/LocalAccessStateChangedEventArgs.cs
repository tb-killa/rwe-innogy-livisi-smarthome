namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class LocalAccessStateChangedEventArgs
{
	public string IP { get; set; }

	public bool IsLocalAccessEnabled { get; set; }
}
