namespace CommonFunctionality.Encryption;

public class crc
{
	private byte[] CRC16_LookupHigh = new byte[16]
	{
		0, 16, 32, 48, 64, 80, 96, 112, 129, 145,
		161, 177, 193, 209, 225, 241
	};

	private byte[] CRC16_LookupLow = new byte[16]
	{
		0, 33, 66, 99, 132, 165, 198, 231, 8, 41,
		74, 107, 140, 173, 206, 239
	};

	public byte CRC16_High;

	public byte CRC16_Low;

	public void CRC16_init()
	{
		CRC16_High = byte.MaxValue;
		CRC16_Low = byte.MaxValue;
	}

	private void CRC16_update_4bits(byte val)
	{
		byte b = (byte)(CRC16_High >> 4);
		b ^= val;
		CRC16_High = (byte)((CRC16_High << 4) | (CRC16_Low >> 4));
		CRC16_Low <<= 4;
		CRC16_High ^= CRC16_LookupHigh[b];
		CRC16_Low ^= CRC16_LookupLow[b];
	}

	public void CRC16_update(int val)
	{
		CRC16_update((byte)val);
	}

	public void CRC16_update(byte val)
	{
		CRC16_update_4bits((byte)(val >> 4));
		CRC16_update_4bits((byte)(val & 0xF));
	}
}
