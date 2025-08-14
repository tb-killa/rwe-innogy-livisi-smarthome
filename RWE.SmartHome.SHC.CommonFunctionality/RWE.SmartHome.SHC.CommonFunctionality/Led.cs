using SHCWrapper.Drivers;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public static class Led
{
	private static bool on = true;

	public static bool On
	{
		get
		{
			return on;
		}
		set
		{
			if (value)
			{
				LEDManager.SwitchOn(LEDManager.LED_GREEN_SMD);
			}
			else
			{
				LEDManager.SwitchOff(LEDManager.LED_GREEN_SMD);
			}
			on = value;
		}
	}
}
