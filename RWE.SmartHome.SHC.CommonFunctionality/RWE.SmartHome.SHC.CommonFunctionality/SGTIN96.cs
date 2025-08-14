using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RWE.SmartHome.SHC.CommonFunctionality;

public class SGTIN96
{
	private static byte header = 48;

	private static readonly byte[] itemReferenceBitCount = new byte[7] { 4, 7, 10, 14, 17, 20, 24 };

	public byte FilterValue { get; set; }

	public byte Partition { get; set; }

	public ulong CompanyPrefix { get; set; }

	public uint ItemReference { get; set; }

	public ulong SerialNumber { get; set; }

	public static SGTIN96 Create(string id)
	{
		byte[] array = new byte[12];
		for (int i = 0; i < 12; i++)
		{
			int startIndex = i * 2;
			array[i] = byte.Parse(id.Substring(startIndex, 2), NumberStyles.HexNumber);
		}
		SGTIN96 sGTIN = new SGTIN96();
		sGTIN.Parse(array);
		return sGTIN;
	}

	public static SGTIN96 Create(byte[] id)
	{
		SGTIN96 sGTIN = new SGTIN96();
		sGTIN.Parse(id);
		return sGTIN;
	}

	public List<byte> GetSerialData()
	{
		List<byte> list = new List<byte>();
		list.Add(header);
		list.Add((byte)(FilterValue << 5));
		list[1] |= (byte)(Partition << 2);
		int num = itemReferenceBitCount[Partition];
		ulong num2 = (CompanyPrefix << num) | ItemReference;
		list[1] |= (byte)(num2 >> 42);
		list.Add((byte)(num2 >> 34));
		list.Add((byte)(num2 >> 26));
		list.Add((byte)(num2 >> 18));
		list.Add((byte)(num2 >> 10));
		list.Add((byte)(num2 >> 2));
		list.Add((byte)(num2 << 6));
		list[7] |= (byte)((SerialNumber >> 32) & 0xFF);
		list.Add((byte)((SerialNumber >> 24) & 0xFF));
		list.Add((byte)((SerialNumber >> 16) & 0xFF));
		list.Add((byte)((SerialNumber >> 8) & 0xFF));
		list.Add((byte)(SerialNumber & 0xFF));
		return list;
	}

	private void Parse(byte[] message)
	{
		if (message.Length >= 12)
		{
			header = message[0];
			FilterValue = (byte)((message[1] & 0xE0) >> 5);
			Partition = (byte)((message[1] & 0x1C) >> 2);
			ulong num = (ulong)((long)(message[1] & 3) << 42);
			num |= (ulong)message[2] << 34;
			num |= (ulong)message[3] << 26;
			num |= (ulong)message[4] << 18;
			num |= (ulong)message[5] << 10;
			num |= (ulong)message[6] << 2;
			num |= (uint)((message[7] & 0xC0) >> 6);
			int num2 = itemReferenceBitCount[Partition];
			CompanyPrefix = (uint)(num >> num2);
			ItemReference = (uint)(num & (uint)(Math.Pow(2.0, num2) - 1.0));
			SerialNumber = (ulong)((long)(message[7] & 0x3F) << 32);
			SerialNumber |= (ulong)message[8] << 24;
			SerialNumber |= (ulong)message[9] << 16;
			SerialNumber |= (ulong)message[10] << 8;
			SerialNumber |= message[11];
		}
	}

	public bool Equals(SGTIN96 other)
	{
		if (object.Equals(other.FilterValue, FilterValue) && other.Partition == Partition && other.CompanyPrefix == CompanyPrefix && other.ItemReference == ItemReference)
		{
			return other.SerialNumber == SerialNumber;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (object.ReferenceEquals(null, obj))
		{
			return false;
		}
		if ((object)obj.GetType() != typeof(SGTIN96))
		{
			return false;
		}
		return Equals((SGTIN96)obj);
	}

	public override int GetHashCode()
	{
		int hashCode = header.GetHashCode();
		hashCode = (hashCode * 397) ^ FilterValue.GetHashCode();
		hashCode = (hashCode * 397) ^ Partition.GetHashCode();
		hashCode = (hashCode * 397) ^ CompanyPrefix.GetHashCode();
		hashCode = (hashCode * 397) ^ ItemReference.GetHashCode();
		return (hashCode * 397) ^ SerialNumber.GetHashCode();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("urn:epc:tag:sgtin-96:");
		stringBuilder.Append((int)FilterValue);
		stringBuilder.Append(".");
		stringBuilder.Append(CompanyPrefix);
		stringBuilder.Append(".");
		stringBuilder.Append(ItemReference);
		stringBuilder.Append(".");
		stringBuilder.Append(SerialNumber);
		return stringBuilder.ToString();
	}
}
