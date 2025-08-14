using System;

namespace SHCWrapper.Drivers;

public class GPIOManager
{
	public enum MODE : uint
	{
		INPUT = 2u,
		OUTPUT,
		INTERRUPT
	}

	public enum ATTRIBUT : byte
	{
		PIO_DEFAULT = 0,
		PIO_PULLUP = 1,
		PIO_DEGLITCH = 2,
		PIO_OPENDRAIN = 4
	}

	public enum LEVEL
	{
		LOW,
		HIGH
	}

	public struct CONFIGURATION_DESCRIPTOR
	{
		private IntPtr name;

		public uint pio_num;

		public LEVEL default_value;

		public ATTRIBUT attribut;

		public MODE type;
	}

	public struct LEVEL_DESCRIPTOR
	{
		public uint pio_num;

		public LEVEL value;
	}

	private const string _DriverName = "PIO1:";

	public const uint MAX_PIO_PER_BANK = 32u;

	private IntPtr _Handle = Utils.INVALID_HANDLE_VALUE;

	private bool _IsOpened;

	public bool IsOpened => _IsOpened;

	public bool Open()
	{
		bool flag = false;
		if (_Handle != Utils.INVALID_HANDLE_VALUE)
		{
			flag = false;
		}
		else
		{
			_Handle = PrivateWrapper.OpenGpio("PIO1:");
			if (_Handle == Utils.INVALID_HANDLE_VALUE)
			{
				flag = false;
			}
			else
			{
				flag = true;
				_IsOpened = true;
			}
		}
		return flag;
	}

	public bool Close()
	{
		bool flag = false;
		if (_Handle == Utils.INVALID_HANDLE_VALUE)
		{
			flag = false;
		}
		else
		{
			PrivateWrapper.CloseGpio(_Handle);
			_Handle = Utils.INVALID_HANDLE_VALUE;
			flag = true;
			_IsOpened = false;
		}
		return flag;
	}

	public bool Configure(CONFIGURATION_DESCRIPTOR pio_config_desciptor)
	{
		bool flag = false;
		if (_Handle == Utils.INVALID_HANDLE_VALUE)
		{
			return false;
		}
		return PrivateWrapper.ConfigureGpio(_Handle, pio_config_desciptor);
	}

	public bool GetState(ref LEVEL_DESCRIPTOR pio_state)
	{
		bool flag = false;
		if (_Handle == Utils.INVALID_HANDLE_VALUE)
		{
			return false;
		}
		return PrivateWrapper.GetGpioState(_Handle, ref pio_state);
	}

	public bool SetState(LEVEL_DESCRIPTOR pio_state)
	{
		bool flag = false;
		if (_Handle == Utils.INVALID_HANDLE_VALUE)
		{
			return false;
		}
		return PrivateWrapper.SetGpioState(_Handle, pio_state);
	}

	public IntPtr GetIntrEvent(uint pin_number)
	{
		return PrivateWrapper.GetGpioIntrEvent(_Handle, pin_number);
	}

	public void ReleaseIntrEvent(IntPtr _hEvent)
	{
		PrivateWrapper.ReleaseGpioIntrEvent(_hEvent);
	}

	public static uint GetBankAPioNum(uint io)
	{
		return io;
	}

	public static uint GetBankBPioNum(uint io)
	{
		return 32 + io;
	}

	public static uint GetBankCPioNum(uint io)
	{
		return 64 + io;
	}
}
