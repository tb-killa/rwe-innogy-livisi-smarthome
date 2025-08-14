using System;

namespace SHCWrapper.Drivers;

public class LEDManager : IDisposable
{
	public static LED LED_GREEN_SMD = new LED(GPIOManager.GetBankAPioNum(5u), GPIOManager.LEVEL.HIGH, switchonatstartup: false);

	private static LED[] _ManagedLed = new LED[1] { LED_GREEN_SMD };

	private static bool _IsInitialize = false;

	private static GPIOManager _GpioManager = null;

	private bool disposed;

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!disposed && disposing)
		{
			_GpioManager.Close();
		}
		disposed = true;
	}

	~LEDManager()
	{
		Dispose(disposing: false);
	}

	private static bool Initialize()
	{
		bool flag = true;
		if (_IsInitialize)
		{
			flag = true;
		}
		else
		{
			try
			{
				_GpioManager = new GPIOManager();
				_GpioManager.Open();
				LED[] managedLed = _ManagedLed;
				foreach (LED lED in managedLed)
				{
					GPIOManager.CONFIGURATION_DESCRIPTOR pio_config_desciptor = new GPIOManager.CONFIGURATION_DESCRIPTOR
					{
						pio_num = lED.ID,
						type = GPIOManager.MODE.OUTPUT
					};
					if (lED.SwitchOnStateLevel == GPIOManager.LEVEL.HIGH)
					{
						if (lED.SwitchOnAtStartup)
						{
							pio_config_desciptor.default_value = GPIOManager.LEVEL.HIGH;
						}
						else
						{
							pio_config_desciptor.default_value = GPIOManager.LEVEL.LOW;
						}
					}
					else if (lED.SwitchOnAtStartup)
					{
						pio_config_desciptor.default_value = GPIOManager.LEVEL.LOW;
					}
					else
					{
						pio_config_desciptor.default_value = GPIOManager.LEVEL.HIGH;
					}
					pio_config_desciptor.attribut = GPIOManager.ATTRIBUT.PIO_DEFAULT;
					if (!_GpioManager.Configure(pio_config_desciptor))
					{
						flag = false;
						break;
					}
				}
			}
			catch
			{
				flag = false;
			}
			finally
			{
				if (flag)
				{
					_IsInitialize = true;
				}
				else
				{
					_GpioManager.Close();
				}
			}
		}
		return flag;
	}

	public static bool SwitchOn(LED led)
	{
		bool flag = true;
		if (!_IsInitialize)
		{
			flag = Initialize();
		}
		if (flag)
		{
			GPIOManager.LEVEL_DESCRIPTOR state = default(GPIOManager.LEVEL_DESCRIPTOR);
			state.pio_num = led.ID;
			if (led.SwitchOnStateLevel == GPIOManager.LEVEL.HIGH)
			{
				state.value = GPIOManager.LEVEL.HIGH;
			}
			else
			{
				state.value = GPIOManager.LEVEL.LOW;
			}
			flag = _GpioManager.SetState(state);
		}
		return flag;
	}

	public static bool SwitchOff(LED led)
	{
		bool flag = true;
		if (!_IsInitialize)
		{
			flag = Initialize();
		}
		if (flag)
		{
			GPIOManager.LEVEL_DESCRIPTOR state = default(GPIOManager.LEVEL_DESCRIPTOR);
			state.pio_num = led.ID;
			if (led.SwitchOnStateLevel == GPIOManager.LEVEL.HIGH)
			{
				state.value = GPIOManager.LEVEL.LOW;
			}
			else
			{
				state.value = GPIOManager.LEVEL.HIGH;
			}
			flag = _GpioManager.SetState(state);
		}
		return flag;
	}
}
