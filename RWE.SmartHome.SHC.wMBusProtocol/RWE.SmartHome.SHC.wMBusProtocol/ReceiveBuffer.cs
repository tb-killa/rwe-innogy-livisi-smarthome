using System.Collections.Generic;

namespace RWE.SmartHome.SHC.wMBusProtocol;

public class ReceiveBuffer
{
	private List<byte> internalBuffer = new List<byte>();

	private int size;

	public bool Filled { get; set; }

	public byte this[int i] => internalBuffer[i];

	public ReceiveBuffer(int size)
	{
		this.size = size;
	}

	public void Add(byte value)
	{
		if (internalBuffer.Count >= size)
		{
			internalBuffer.RemoveAt(0);
		}
		internalBuffer.Add(value);
		Filled = internalBuffer.Count == size;
	}

	public byte[] ToArray()
	{
		return internalBuffer.ToArray();
	}
}
