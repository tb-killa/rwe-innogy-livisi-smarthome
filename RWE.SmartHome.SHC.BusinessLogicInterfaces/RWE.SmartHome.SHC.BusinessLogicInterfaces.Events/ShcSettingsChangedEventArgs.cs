namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class ShcSettingsChangedEventArgs
{
	public string DataId { get; set; }

	public ModificationType Modification { get; set; }
}
