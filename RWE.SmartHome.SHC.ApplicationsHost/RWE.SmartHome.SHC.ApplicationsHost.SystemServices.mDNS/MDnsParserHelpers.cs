using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RWE.SmartHome.SHC.ApplicationsHost.SystemServices.mDNS;

internal static class MDnsParserHelpers
{
	public static MDnsDomainName ParseDomainName(byte[] data, ref int currentOffset)
	{
		List<string> list = new List<string>();
		byte b = data[currentOffset];
		bool flag = true;
		int num = 0;
		int num2 = currentOffset;
		while (b > 0)
		{
			if (num++ > 20)
			{
				throw new Exception("Circular links. Invalid message");
			}
			if (b > 63)
			{
				num2 = 256 * (data[num2] & 0x3F) + data[num2 + 1];
				if (flag)
				{
					currentOffset++;
					flag = false;
				}
				b = data[num2];
			}
			list.Add(Encoding.ASCII.GetString(data, num2 + 1, b));
			num2 += b + 1;
			if (flag)
			{
				currentOffset += b + 1;
			}
			b = data[num2];
		}
		currentOffset++;
		return new MDnsDomainName(list);
	}

	public static List<string> ParseTextLabels(byte[] data, int startOffset, int dataSizeLeft)
	{
		List<string> list = new List<string>();
		while (dataSizeLeft > 0)
		{
			byte b = data[startOffset];
			list.Add(Encoding.ASCII.GetString(data, startOffset + 1, b));
			startOffset += b + 1;
			dataSizeLeft -= b + 1;
		}
		return list;
	}

	public static ushort ParseUInt16(byte[] data, ref int currentOffset)
	{
		ushort result = (ushort)((data[currentOffset] << 8) | data[currentOffset + 1]);
		currentOffset += 2;
		return result;
	}

	public static uint ParseUInt32(byte[] data, ref int currentOffset)
	{
		uint result = (uint)((data[currentOffset] << 24) | (data[currentOffset + 1] << 16) | (data[currentOffset + 2] << 8) | data[currentOffset + 3]);
		currentOffset += 4;
		return result;
	}

	public static string ToReadable(byte[] me, int size)
	{
		if (me == null)
		{
			return "Null";
		}
		StringBuilder stringBuilder = new StringBuilder(size * 2);
		foreach (byte item in me.Take(size))
		{
			stringBuilder.Append(item.ToString("X2"));
		}
		return stringBuilder.ToString();
	}
}
