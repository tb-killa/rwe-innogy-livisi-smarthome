using System;
using System.Security.Cryptography;

namespace WebSocketLibrary.Helpers;

public static class FrameMaskHelper
{
	private static readonly RandomNumberGenerator randomNumberGenerator = new RNGCryptoServiceProvider();

	private static readonly object sync = new object();

	private static readonly byte[] keyBuffer = new byte[4];

	public static ulong ApplyMaskOnBuffer(byte[] buffer, ulong offset, ulong size, uint mask)
	{
		if (mask == 0)
		{
			return size;
		}
		int num = 0;
		ulong num2 = Math.Min(offset + size, (ulong)buffer.Length);
		ulong num3 = offset;
		while (num3 < num2)
		{
			buffer[num3] ^= (byte)((mask >> (3 - num) * 8) & 0xFF);
			num3++;
			num = (num + 1) % 4;
		}
		return num2 - offset;
	}

	public static uint GetNewMaskingKey()
	{
		lock (sync)
		{
			randomNumberGenerator.GetBytes(keyBuffer);
			return ConverterHelper.GetUIntFromBuffer(keyBuffer, 0);
		}
	}
}
