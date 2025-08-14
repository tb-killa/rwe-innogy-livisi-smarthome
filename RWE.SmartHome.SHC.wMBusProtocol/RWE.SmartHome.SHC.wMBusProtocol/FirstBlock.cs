using System;

namespace RWE.SmartHome.SHC.wMBusProtocol;

public class FirstBlock : IBlock
{
	public byte Length { get; set; }

	public byte Control { get; set; }

	public byte[] Manufacturer { get; set; }

	public byte[] Address { get; set; }

	public byte BlockSize => 10;

	public FirstBlock()
	{
	}

	public FirstBlock(byte[] buffer)
	{
		Length = buffer[0];
		Control = buffer[1];
		Manufacturer = new byte[2];
		Array.Copy(buffer, 2, Manufacturer, 0, 2);
		Address = new byte[6];
		Array.Copy(buffer, 4, Address, 0, 6);
	}

	public byte[] ToArray()
	{
		byte[] array = new byte[BlockSize];
		array[0] = Length;
		array[1] = Control;
		Array.Copy(Manufacturer, 0, array, 2, 2);
		Array.Copy(Address, 0, array, 4, 6);
		return array;
	}
}
