using System;
using System.Collections.Generic;
using System.Text;

namespace RWE.SmartHome.SHC.wMBusProtocol;

public class WMBusFrame
{
	public byte Length { get; set; }

	public byte Control { get; set; }

	public string Manufacturer { get; set; }

	public byte[] ManufacturerCode { get; set; }

	public DeviceTypeIdentification DeviceTypeIdentification { get; set; }

	public byte VersionIdentification { get; set; }

	public byte[] Identification { get; set; }

	public ControlInformationCode ControlInformation { get; set; }

	public List<byte> Payload { get; private set; }

	public WMBusFrame()
	{
		Payload = new List<byte>();
	}

	public WMBusFrame(byte[] buffer)
		: this()
	{
		List<byte> list = new List<byte>(buffer);
		FirstBlock firstBlock = new FirstBlock(list.ToArray());
		list.RemoveRange(0, firstBlock.BlockSize);
		Add(firstBlock);
		byte[] array = new byte[Math.Min(list.Count, 16)];
		Array.Copy(list.ToArray(), array, array.Length);
		SecondBlock secondBlock = new SecondBlock(array);
		Add(secondBlock);
		list.RemoveRange(0, (list.Count > secondBlock.BlockSize) ? secondBlock.BlockSize : list.Count);
		while (list.Count > 0)
		{
			array = new byte[(list.Count > 18) ? 18 : list.Count];
			Array.Copy(list.ToArray(), array, array.Length);
			DataBlock dataBlock = new DataBlock(array);
			Add(dataBlock);
			list.RemoveRange(0, (list.Count > dataBlock.BlockSize) ? dataBlock.BlockSize : list.Count);
		}
	}

	public static string ManufacturerName(byte[] ManufacturerId, int offset)
	{
		ushort num = (ushort)(ManufacturerId[1 + offset] << 8);
		num |= ManufacturerId[offset];
		StringBuilder stringBuilder = new StringBuilder();
		byte b = (byte)(num / 1024 + 64);
		num = (ushort)(num - (b - 64) * 32 * 32);
		byte b2 = (byte)(num / 32 + 64);
		num = (ushort)(num - (b2 - 64) * 32);
		byte value = (byte)(num + 64);
		stringBuilder.Append((char)b);
		stringBuilder.Append((char)b2);
		stringBuilder.Append((char)value);
		return stringBuilder.ToString();
	}

	public static byte[] CreateManufacturerId(string name)
	{
		name.ToUpper();
		byte[] array = new byte[2];
		short num = 0;
		if (name.Length > 0)
		{
			num += (short)((name.ToUpper()[0] - 64) * 32 * 32);
		}
		if (name.Length > 1)
		{
			num += (short)((name.ToUpper()[1] - 64) * 32);
		}
		if (name.Length > 2)
		{
			num += (short)(name.ToUpper()[2] - 64);
		}
		array[1] = (byte)(num << 8 >> 8);
		array[0] = (byte)(num >> 8);
		return array;
	}

	public void Add(IBlock block)
	{
		if (block is FirstBlock firstBlock)
		{
			Length = firstBlock.Length;
			Control = firstBlock.Control;
			ManufacturerCode = firstBlock.Manufacturer;
			Manufacturer = ManufacturerName(firstBlock.Manufacturer, 0);
			byte[] array = new byte[4];
			Array.Copy(firstBlock.Address, array, 4);
			Identification = array;
			VersionIdentification = firstBlock.Address[4];
			DeviceTypeIdentification = (DeviceTypeIdentification)firstBlock.Address[5];
		}
		if (block is SecondBlock secondBlock)
		{
			ControlInformation = secondBlock.ControlInformation;
			Payload.AddRange(secondBlock.Data);
		}
		if (block is DataBlock dataBlock)
		{
			Payload.AddRange(dataBlock.Data);
		}
	}

	public List<IBlock> GetBlocks()
	{
		List<IBlock> list = new List<IBlock>();
		FirstBlock firstBlock = new FirstBlock();
		firstBlock.Address = new byte[6];
		Array.Copy(Identification, 0, firstBlock.Address, 0, 4);
		firstBlock.Address[4] = VersionIdentification;
		firstBlock.Address[5] = (byte)DeviceTypeIdentification;
		firstBlock.Control = Control;
		firstBlock.Manufacturer = ManufacturerCode;
		firstBlock.Length = (byte)(10 + Payload.Count);
		list.Add(firstBlock);
		SecondBlock secondBlock = new SecondBlock();
		secondBlock.ControlInformation = ControlInformation;
		int count = Payload.Count;
		int num = 0;
		int num2 = ((count - 15 > 0) ? 15 : (15 - Math.Abs(count - 15)));
		secondBlock.Data = new byte[num2];
		byte[] sourceArray = Payload.ToArray();
		Array.Copy(sourceArray, num, secondBlock.Data, 0, num2);
		count -= num2;
		list.Add(secondBlock);
		num += 15;
		while (count > 0)
		{
			DataBlock dataBlock = new DataBlock();
			num2 = ((count - 16 > 0) ? 16 : (16 - Math.Abs(count - 16)));
			dataBlock.Data = new byte[num2];
			Array.Copy(sourceArray, num, dataBlock.Data, 0, num2);
			num += 16;
			count -= 16;
			list.Add(dataBlock);
		}
		return list;
	}
}
