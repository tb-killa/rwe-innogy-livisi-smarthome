namespace SHCWrapper.Drivers;

public class LED
{
	internal uint ID;

	internal GPIOManager.LEVEL SwitchOnStateLevel;

	internal bool SwitchOnAtStartup;

	internal LED(uint id, GPIOManager.LEVEL switchonstatelevel, bool switchonatstartup)
	{
		ID = id;
		SwitchOnStateLevel = switchonstatelevel;
		SwitchOnAtStartup = switchonatstartup;
	}
}
