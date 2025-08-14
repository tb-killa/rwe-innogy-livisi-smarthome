using System;
using CommonFunctionality.Encryption;

namespace RWE.SmartHome.SHC.DeviceManager;

public class DeviceKeyEncryptor
{
	public const int SerialNumberLength = 12;

	public const int KeyAuthenticationLength = 4;

	public const int AesKeyLength = 16;

	public const int AESBlockLength = 16;

	public static byte[] CCM(byte[] sgtin, byte[] secNumber, byte[] NetworkKey, byte[] DefaultKey, int displacement, int returnLength)
	{
		byte[] array = new byte[16];
		Buffer.BlockCopy(sgtin, 0, array, 0, sgtin.Length);
		ulong num = (ulong)(secNumber[0] << 24);
		num += (ulong)(secNumber[1] << 16);
		num += (ulong)(secNumber[2] << 8);
		num += (ulong)(secNumber[3] + displacement);
		array[12] = (byte)((num >> 24) & 0xFF);
		array[13] = (byte)((num >> 16) & 0xFF);
		array[14] = (byte)((num >> 8) & 0xFF);
		array[15] = (byte)(num & 0xFF);
		Aes aes = new Aes(KeySize.Bits128, DefaultKey);
		byte[] output = new byte[16];
		aes.Cipher(array, out output);
		byte[] array2 = new byte[returnLength];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = (byte)(output[i] ^ NetworkKey[i]);
		}
		return array2;
	}
}
