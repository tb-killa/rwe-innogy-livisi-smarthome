using System;

namespace SHCWrapper.Drivers;

public class SPI
{
	private string _DriverName = "";

	private IntPtr _Handle = Utils.INVALID_HANDLE_VALUE;

	private bool _IsOpened;

	public bool IsOpened => _IsOpened;

	public SPI(int spi_port, int chip_select)
	{
		switch (spi_port)
		{
		case 0:
			switch (chip_select)
			{
			case 0:
				_DriverName = "SPI1:";
				break;
			case 1:
				throw new Exception("Not implemented");
			case 2:
				break;
			}
			break;
		case 1:
			switch (chip_select)
			{
			case 0:
				throw new Exception("Not implemented");
			case 1:
				_DriverName = "SPI2:";
				break;
			case 2:
				throw new Exception("Not implemented");
			}
			break;
		}
	}

	public bool Open()
	{
		bool flag = false;
		if (_Handle != Utils.INVALID_HANDLE_VALUE || _DriverName == "")
		{
			flag = false;
		}
		else
		{
			_Handle = PrivateWrapper.OpenSPI(_DriverName);
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
			PrivateWrapper.CloseSPI(_Handle);
			flag = true;
			_IsOpened = false;
		}
		return flag;
	}

	public bool TransacSPI(byte[] pBufIn, byte[] pBufOut, int Len, bool bSplitTransaction)
	{
		bool flag = false;
		if (_Handle == Utils.INVALID_HANDLE_VALUE)
		{
			return false;
		}
		return PrivateWrapper.TransacSPI(_Handle, pBufIn, pBufOut, Len, bSplitTransaction);
	}
}
