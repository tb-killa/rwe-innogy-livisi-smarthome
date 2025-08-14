using System;
using System.Threading;

namespace SHCWrapper.Drivers;

public class Switch : IDisposable
{
	public enum SWITCH_STATE
	{
		ON,
		OFF
	}

	public delegate void SwitchEvent(SWITCH_STATE new_state);

	public static uint SHC_SWITCH = GPIOManager.GetBankAPioNum(6u);

	public static GPIOManager.LEVEL SHC_SWITCH_POLARITY = GPIOManager.LEVEL.HIGH;

	private static GPIOManager _GpioManager = null;

	private IntPtr _hGpioEvent = IntPtr.Zero;

	private uint _GpioPin;

	private GPIOManager.LEVEL _GpioPolarity;

	private GPIOManager.LEVEL _GpioLevel;

	private Thread _InterruptThread;

	private bool _bContinue = true;

	public event SwitchEvent OnSwitchChange;

	public Switch()
		: this(SHC_SWITCH, SHC_SWITCH_POLARITY)
	{
	}

	public Switch(uint switch_pin, GPIOManager.LEVEL polarity)
	{
		if (_GpioManager == null)
		{
			_GpioManager = new GPIOManager();
			_GpioManager.Open();
		}
		GPIOManager.CONFIGURATION_DESCRIPTOR pio_config_desciptor = new GPIOManager.CONFIGURATION_DESCRIPTOR
		{
			pio_num = switch_pin,
			type = GPIOManager.MODE.INTERRUPT,
			attribut = GPIOManager.ATTRIBUT.PIO_DEGLITCH
		};
		_GpioPolarity = polarity;
		_GpioPin = switch_pin;
		_GpioManager.Configure(pio_config_desciptor);
		GPIOManager.LEVEL_DESCRIPTOR pio_state = new GPIOManager.LEVEL_DESCRIPTOR
		{
			pio_num = switch_pin
		};
		_GpioManager.GetState(ref pio_state);
		_GpioLevel = pio_state.value;
		_hGpioEvent = _GpioManager.GetIntrEvent(switch_pin);
		_InterruptThread = new Thread(SwitchIoEventThread);
		_InterruptThread.Start();
	}

	private void SwitchIoEventThread()
	{
		while (_bContinue)
		{
			if (Utils.WaitForSingleObject(_hGpioEvent, 1000) != 0)
			{
				continue;
			}
			GPIOManager.LEVEL_DESCRIPTOR pio_state = new GPIOManager.LEVEL_DESCRIPTOR
			{
				pio_num = _GpioPin
			};
			_GpioManager.GetState(ref pio_state);
			if (_GpioLevel == pio_state.value)
			{
				continue;
			}
			if (pio_state.value == _GpioPolarity)
			{
				if (this.OnSwitchChange != null)
				{
					this.OnSwitchChange(SWITCH_STATE.ON);
				}
			}
			else if (this.OnSwitchChange != null)
			{
				this.OnSwitchChange(SWITCH_STATE.OFF);
			}
			_GpioLevel = pio_state.value;
		}
	}

	public SWITCH_STATE GetCurrentState()
	{
		GPIOManager.LEVEL_DESCRIPTOR pio_state = new GPIOManager.LEVEL_DESCRIPTOR
		{
			pio_num = _GpioPin
		};
		_GpioManager.GetState(ref pio_state);
		_GpioLevel = pio_state.value;
		if (_GpioLevel == _GpioPolarity)
		{
			return SWITCH_STATE.ON;
		}
		return SWITCH_STATE.OFF;
	}

	public void Dispose()
	{
		_bContinue = false;
	}
}
