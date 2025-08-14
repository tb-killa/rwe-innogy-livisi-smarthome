using System;

namespace SHCWrapper.Drivers;

public class LCD : IDisposable
{
	private SPI _spi;

	private bool disposed;

	public LCD()
	{
		_spi = new SPI(0, 0);
		_spi.Open();
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
			_spi.Close();
		}
		disposed = true;
	}

	~LCD()
	{
		Dispose(disposing: false);
	}

	public bool Initialize()
	{
		GPIOManager gPIOManager = new GPIOManager();
		gPIOManager.Open();
		gPIOManager.Configure(new GPIOManager.CONFIGURATION_DESCRIPTOR
		{
			attribut = GPIOManager.ATTRIBUT.PIO_DEFAULT,
			default_value = GPIOManager.LEVEL.LOW,
			pio_num = GPIOManager.GetBankBPioNum(28u),
			type = GPIOManager.MODE.OUTPUT
		});
		gPIOManager.Close();
		return true;
	}

	public bool SendCommand(byte command, byte[] Data, int Data_Length)
	{
		if (Data_Length < 0)
		{
			Data_Length = 0;
		}
		byte[] array = new byte[Data_Length + 1];
		array[0] = command;
		if (Data_Length > 0 && Data != null)
		{
			Array.Copy(Data, 0, array, 1, Data_Length);
		}
		if (!_spi.TransacSPI(null, array, Data_Length + 1, bSplitTransaction: true))
		{
			return false;
		}
		return true;
	}

	public bool SetBackLightState(bool bState)
	{
		GPIOManager gPIOManager = new GPIOManager();
		gPIOManager.Open();
		gPIOManager.Configure(new GPIOManager.CONFIGURATION_DESCRIPTOR
		{
			attribut = GPIOManager.ATTRIBUT.PIO_DEFAULT,
			default_value = (bState ? GPIOManager.LEVEL.HIGH : GPIOManager.LEVEL.LOW),
			pio_num = GPIOManager.GetBankBPioNum(29u),
			type = GPIOManager.MODE.OUTPUT
		});
		gPIOManager.Close();
		return true;
	}
}
