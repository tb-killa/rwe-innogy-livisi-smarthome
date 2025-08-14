using System;

namespace RWE.SmartHome.SHC.HCI.Messages;

public class SimpleResponseMessage
{
	public bool Success { get; set; }

	public SimpleResponseMessage()
	{
		Success = false;
	}

	public SimpleResponseMessage(byte[] buffer)
	{
		if (buffer.Length != 1)
		{
			throw new ArgumentException("The buffer size must be 1 bytes.");
		}
		Success = buffer[0] == 1;
	}

	public byte[] GetBytes()
	{
		return new byte[1] { (byte)(Success ? 1u : 0u) };
	}
}
