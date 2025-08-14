using System;

namespace WebSocketLibrary.Helpers;

public static class ConverterHelper
{
	public static void PopulateBufferWithUShort(ushort number, byte[] buffer, int offset)
	{
		if (buffer.Length < offset + 2)
		{
			throw new ArgumentException("PopulateBufferWithUShort - insuficient buffer size");
		}
		buffer[offset] = (byte)((number >> 8) & 0xFF);
		buffer[offset + 1] = (byte)(number & 0xFF);
	}

	public static void PopulateBufferWithUInt(uint number, byte[] buffer, int offset)
	{
		if (buffer.Length < offset + 4)
		{
			throw new ArgumentException("PopulateBufferWithULong - insuficient buffer size");
		}
		buffer[offset] = (byte)((number >> 24) & 0xFF);
		buffer[offset + 1] = (byte)((number >> 16) & 0xFF);
		buffer[offset + 2] = (byte)((number >> 8) & 0xFF);
		buffer[offset + 3] = (byte)(number & 0xFF);
	}

	public static void PopulateBufferWithULong(ulong number, byte[] buffer, int offset)
	{
		if (buffer.Length < offset + 8)
		{
			throw new ArgumentException("PopulateBufferWithULong - insuficient buffer size");
		}
		buffer[offset] = (byte)((number >> 56) & 0xFF);
		buffer[offset + 1] = (byte)((number >> 48) & 0xFF);
		buffer[offset + 2] = (byte)((number >> 40) & 0xFF);
		buffer[offset + 3] = (byte)((number >> 32) & 0xFF);
		buffer[offset + 4] = (byte)((number >> 24) & 0xFF);
		buffer[offset + 5] = (byte)((number >> 16) & 0xFF);
		buffer[offset + 6] = (byte)((number >> 8) & 0xFF);
		buffer[offset + 7] = (byte)(number & 0xFF);
	}

	public static ushort GetUShortFromBuffer(byte[] data, int offset)
	{
		ushort num = 0;
		if (data.Length < offset + 2)
		{
			throw new ArgumentException("GetUShortFromBuffer - insuficient buffer size");
		}
		num |= data[offset];
		num <<= 8;
		return (ushort)(num | data[offset + 1]);
	}

	public static uint GetUIntFromBuffer(byte[] data, int offset)
	{
		uint num = 0u;
		if (data.Length < offset + 4)
		{
			throw new ArgumentException("GetUintFromBuffer - insuficient buffer size");
		}
		num |= data[offset];
		num <<= 8;
		num |= data[offset + 1];
		num <<= 8;
		num |= data[offset + 2];
		num <<= 8;
		return num | data[offset + 3];
	}
}
