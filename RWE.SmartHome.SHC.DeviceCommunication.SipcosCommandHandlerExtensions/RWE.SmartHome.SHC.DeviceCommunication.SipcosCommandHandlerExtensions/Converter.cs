using System;

namespace RWE.SmartHome.SHC.DeviceCommunication.SipcosCommandHandlerExtensions;

public static class Converter
{
	public static int ExtractAddress(byte[] buffer, int sourceIndex)
	{
		byte[] array = new byte[4];
		Array.Copy(buffer, sourceIndex, array, 0, 3);
		Array.Reverse(array, 0, 3);
		return BitConverter.ToInt32(array, 0);
	}

	public static byte[] CopyAddressToBuffer(byte[] buffer, int senderAddress, int position)
	{
		byte[] bytes = BitConverter.GetBytes(senderAddress);
		Array.Reverse(bytes, 0, 3);
		Array.Copy(bytes, 0, buffer, position, 3);
		return buffer;
	}

	public static ushort ReadUShort(byte[] buffer, int index)
	{
		return (ushort)((buffer[index] << 8) | buffer[index + 1]);
	}

	public static byte[] GetBytes(ushort checksum)
	{
		return new byte[2]
		{
			(byte)((checksum & 0xFF00) >> 8),
			(byte)(checksum & 0xFF)
		};
	}
}
