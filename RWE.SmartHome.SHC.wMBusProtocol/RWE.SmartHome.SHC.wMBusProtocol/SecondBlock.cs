using System;

namespace RWE.SmartHome.SHC.wMBusProtocol;

public class SecondBlock : IBlock
{
	public byte[] Data { get; set; }

	public ControlInformationCode ControlInformation { get; set; }

	public byte BlockSize => 16;

	public SecondBlock(byte[] buffer)
	{
		ControlInformation = (ControlInformationCode)buffer[0];
		Data = new byte[buffer.Length - 1];
		Array.Copy(buffer, 1, Data, 0, Data.Length);
	}

	public SecondBlock()
	{
	}

	public byte[] ToArray()
	{
		byte[] array = new byte[BlockSize];
		array[0] = (byte)ControlInformation;
		Array.Copy(Data, 0, array, 1, Data.Length);
		return array;
	}
}
