using System.IO;
using System.Threading;
using SHCWrapper.Drivers;

namespace SHCWrapper.FirmwareUpdater;

public static class AVRFirmwareManager
{
	private enum LockType : byte
	{
		LOCK = 224,
		LOW = 160,
		HIGH = 168,
		EXTENDED = 164
	}

	private static SPI _spi;

	private static object thisLock = new object();

	private static bool WriteFile(Stream myFile)
	{
		bool result = false;
		if (myFile != null)
		{
			bool flag = true;
			short num = 0;
			short num2 = 64;
			byte[] array = new byte[2];
			if (!ProgrammingEnable())
			{
				return result;
			}
			if (!ChipErase())
			{
				return result;
			}
			Thread.Sleep(2);
			PollReady();
			do
			{
				if (myFile.Read(array, 0, 2) != 0)
				{
					if (SendDataByte(array, (byte)(num & 0x3F), bDataHigh: false))
					{
						if (!SendDataByte(array, (byte)(num & 0x3F), bDataHigh: true))
						{
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
					num++;
				}
				else
				{
					num = (short)(num + num2 - num % num2);
					flag = false;
					result = true;
				}
				if (num % num2 == 0)
				{
					WritePage((short)(num - num2));
					Thread.Sleep(15);
					PollReady();
				}
			}
			while (flag);
			myFile.Close();
		}
		return result;
	}

	public static bool UpdateAVR(Stream data_to_write)
	{
		lock (thisLock)
		{
			bool result = true;
			try
			{
				_spi = new SPI(1, 1);
				if (_spi.Open())
				{
					if (!WriteFile(data_to_write))
					{
						result = false;
					}
				}
				else
				{
					result = false;
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (_spi != null)
				{
					_spi.Close();
				}
			}
			return result;
		}
	}

	private static bool ProgrammingEnable()
	{
		bool result = true;
		byte[] array = new byte[4];
		byte[] array2 = new byte[4];
		array[0] = 172;
		array[1] = 83;
		array[2] = 0;
		array[3] = 0;
		if (!_spi.TransacSPI(array2, array, 4, bSplitTransaction: false))
		{
			result = false;
		}
		else if (array2[2] != 83)
		{
			result = false;
		}
		return result;
	}

	private static bool ChipErase()
	{
		bool result = true;
		byte[] array = new byte[4];
		byte[] array2 = new byte[4];
		array[0] = 172;
		array[1] = 128;
		array[2] = 0;
		array[3] = 0;
		if (!_spi.TransacSPI(array2, array, 4, bSplitTransaction: false))
		{
			result = false;
		}
		else if (array2[2] != 128)
		{
			result = false;
		}
		return result;
	}

	private static bool PollReady()
	{
		bool result = true;
		bool flag = true;
		byte[] array = new byte[4];
		byte[] array2 = new byte[4];
		do
		{
			array[0] = 240;
			array[1] = 0;
			array[2] = 0;
			array[3] = 0;
			if (!_spi.TransacSPI(array2, array, 4, bSplitTransaction: false))
			{
				flag = false;
				result = false;
			}
			else if (array2[1] == 240)
			{
				if (array2[3] == 254)
				{
					flag = false;
					result = false;
				}
			}
			else
			{
				result = false;
			}
		}
		while (flag);
		return result;
	}

	private static bool SendDataByte(byte[] dataFile, byte address, bool bDataHigh)
	{
		bool result = true;
		byte[] array = new byte[4];
		byte[] array2 = new byte[4];
		byte b;
		byte b2;
		if (bDataHigh)
		{
			b = 72;
			b2 = dataFile[1];
		}
		else
		{
			b = 64;
			b2 = dataFile[0];
		}
		array[0] = b;
		array[1] = 0;
		array[2] = address;
		array[3] = b2;
		if (!_spi.TransacSPI(array2, array, 4, bSplitTransaction: false))
		{
			result = false;
		}
		else if (array2[1] == b)
		{
			if (array2[3] != address)
			{
				result = false;
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	private static bool WritePage(short pageAddr)
	{
		bool result = true;
		byte[] array = new byte[4];
		byte[] array2 = new byte[4];
		byte b = (array[0] = 76);
		array[1] = (byte)((pageAddr >> 8) & 0xFF);
		array[2] = (byte)(pageAddr & 0xFF);
		array[3] = 0;
		if (!_spi.TransacSPI(array2, array, 4, bSplitTransaction: false))
		{
			result = false;
		}
		else if (array2[1] == b)
		{
			if (array2[3] != (byte)(pageAddr & 0xFF))
			{
				result = false;
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	private static bool WriteLockFuseBits(LockType LockType, byte value)
	{
		bool result = true;
		byte[] array = new byte[4];
		byte[] array2 = new byte[4];
		array[0] = 172;
		array[1] = (byte)LockType;
		array[2] = 0;
		array[3] = value;
		if (!_spi.TransacSPI(array2, array, 4, bSplitTransaction: false))
		{
			result = false;
		}
		else if (array2[1] != array[0] || array2[2] != array[1])
		{
			result = false;
		}
		return result;
	}

	public static bool WriteFuseAndLocksBits(byte Lockbits, byte FuseLowBits, byte FuseHighBits, byte ExtendedFuseBits)
	{
		lock (thisLock)
		{
			bool result = false;
			try
			{
				_spi = new SPI(1, 1);
				if (_spi.Open() && ProgrammingEnable())
				{
					Thread.Sleep(2);
					PollReady();
					if (WriteLockFuseBits(LockType.LOCK, Lockbits))
					{
						PollReady();
						if (WriteLockFuseBits(LockType.LOW, FuseLowBits))
						{
							PollReady();
							if (WriteLockFuseBits(LockType.HIGH, FuseHighBits))
							{
								PollReady();
								if (WriteLockFuseBits(LockType.EXTENDED, ExtendedFuseBits))
								{
									PollReady();
									result = true;
								}
							}
						}
					}
				}
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (_spi != null)
				{
					_spi.Close();
				}
			}
			return result;
		}
	}
}
