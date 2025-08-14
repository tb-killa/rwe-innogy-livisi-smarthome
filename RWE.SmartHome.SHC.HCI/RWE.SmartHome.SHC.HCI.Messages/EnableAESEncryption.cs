using System;

namespace RWE.SmartHome.SHC.HCI.Messages;

public class EnableAESEncryption
{
	public bool StoreInNVM { get; set; }

	public bool Activated { get; set; }

	public EnableAESEncryption()
	{
		StoreInNVM = false;
	}

	public EnableAESEncryption(byte[] buffer)
	{
		if (buffer.Length != 2)
		{
			throw new ArgumentException("The buffer size must be 2 bytes.");
		}
		StoreInNVM = buffer[0] == 1;
		Activated = buffer[1] == 1;
	}

	public byte[] GetBytes()
	{
		return new byte[17]
		{
			(byte)(StoreInNVM ? 1u : 0u),
			(byte)(Activated ? 1u : 0u),
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
	}
}
