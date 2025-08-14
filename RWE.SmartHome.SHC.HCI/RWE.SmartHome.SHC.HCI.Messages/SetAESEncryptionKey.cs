using System;

namespace RWE.SmartHome.SHC.HCI.Messages;

public class SetAESEncryptionKey
{
	public byte[] AESKey { get; set; }

	public bool StoreInNVM { get; set; }

	public SetAESEncryptionKey()
	{
		StoreInNVM = false;
	}

	public SetAESEncryptionKey(byte[] buffer)
	{
		if (buffer.Length != 17)
		{
			throw new ArgumentException("The buffer size must be 17 bytes.");
		}
		StoreInNVM = buffer[0] == 1;
		AESKey = new byte[16];
		Array.Copy(buffer, 1, AESKey, 0, 16);
	}

	public byte[] GetBytes()
	{
		byte[] array = new byte[17]
		{
			(byte)(StoreInNVM ? 1u : 0u),
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};
		Array.Copy(AESKey, 0, array, 1, 16);
		return array;
	}
}
