using System;

namespace RWE.SmartHome.SHC.wMBusProtocol;

public class DataBlock : IBlock
{
	public byte[] Data { get; set; }

	public byte BlockSize => 18;

	public DataBlock()
	{
	}

	public DataBlock(byte[] buffer)
	{
		Data = new byte[buffer.Length];
		Array.Copy(buffer, Data, Data.Length);
	}

	public byte[] ToArray()
	{
		return Data;
	}
}
