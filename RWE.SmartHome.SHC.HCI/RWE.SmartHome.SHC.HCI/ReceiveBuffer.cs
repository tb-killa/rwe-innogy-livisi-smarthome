using System.Collections.Generic;

namespace RWE.SmartHome.SHC.HCI;

public class ReceiveBuffer
{
	private List<byte> internalBuffer = new List<byte>();

	public int Size { get; set; }

	public bool Filled { get; set; }

	public byte this[int i] => internalBuffer[i];

	public ReceiveBuffer(int size)
	{
		Size = size;
	}

	public void Add(byte value)
	{
		if (internalBuffer.Count >= Size)
		{
			internalBuffer.RemoveAt(0);
		}
		internalBuffer.Add(value);
		Filled = internalBuffer.Count == Size;
	}

	public byte[] ToArray()
	{
		return internalBuffer.ToArray();
	}
}
