namespace RWE.SmartHome.SHC.BusinessLogicInterfaces.Events;

public class MemoryShortageEventArgs
{
	public bool IsShortage { get; set; }

	public decimal MemoryLoad { get; set; }

	public bool IsStartup { get; set; }
}
