using System;
using System.Threading;

namespace SHCWrapper.Drivers;

public class Button : IDisposable
{
	public delegate void ButtonEvent();

	public static uint LCD_BUTTON_1 = GPIOManager.GetBankAPioNum(7u);

	public static GPIOManager.LEVEL LCD_BUTTON_1_POLARITY = GPIOManager.LEVEL.HIGH;

	public static uint LCD_BUTTON_2 = GPIOManager.GetBankAPioNum(8u);

	public static GPIOManager.LEVEL LCD_BUTTON_2_POLARITY = GPIOManager.LEVEL.HIGH;

	private static GPIOManager _GpioManager = null;

	private IntPtr _hGpioEvent = IntPtr.Zero;

	private uint _GpioPin;

	private GPIOManager.LEVEL _GpioPolarity;

	private GPIOManager.LEVEL _GpioLevel;

	private Thread _InterruptThread;

	private bool _bContinue = true;

	private bool disposed;

	public event ButtonEvent OnButtonPress;

	public Button(uint button_pin, GPIOManager.LEVEL polarity)
	{
		if (_GpioManager == null)
		{
			_GpioManager = new GPIOManager();
			_GpioManager.Open();
		}
		GPIOManager.CONFIGURATION_DESCRIPTOR pio_config_desciptor = new GPIOManager.CONFIGURATION_DESCRIPTOR
		{
			pio_num = button_pin,
			type = GPIOManager.MODE.INTERRUPT,
			attribut = GPIOManager.ATTRIBUT.PIO_DEGLITCH
		};
		_GpioPolarity = polarity;
		_GpioPin = button_pin;
		_GpioManager.Configure(pio_config_desciptor);
		GPIOManager.LEVEL_DESCRIPTOR pio_state = new GPIOManager.LEVEL_DESCRIPTOR
		{
			pio_num = button_pin
		};
		_GpioManager.GetState(ref pio_state);
		_GpioLevel = pio_state.value;
		_hGpioEvent = _GpioManager.GetIntrEvent(button_pin);
		_InterruptThread = new Thread(ButtonIoEventThread);
		_InterruptThread.Start();
	}

	public GPIOManager.LEVEL_DESCRIPTOR GetCurrentLevel()
	{
		GPIOManager.LEVEL_DESCRIPTOR pio_state = new GPIOManager.LEVEL_DESCRIPTOR
		{
			pio_num = _GpioPin
		};
		_GpioManager.GetState(ref pio_state);
		return pio_state;
	}

	private void ButtonIoEventThread()
	{
		while (_bContinue)
		{
			if (Utils.WaitForSingleObject(_hGpioEvent, 1000) != 0)
			{
				continue;
			}
			GPIOManager.LEVEL_DESCRIPTOR currentLevel = GetCurrentLevel();
			if (_GpioLevel != currentLevel.value)
			{
				if (currentLevel.value == _GpioPolarity && this.OnButtonPress != null)
				{
					this.OnButtonPress();
				}
				_GpioLevel = currentLevel.value;
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!disposed && disposing)
		{
			_bContinue = false;
			if (_InterruptThread != null)
			{
				_InterruptThread.Join();
			}
			_GpioManager.Close();
		}
		disposed = true;
	}

	~Button()
	{
		Dispose(disposing: false);
	}
}
