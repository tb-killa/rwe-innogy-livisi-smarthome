using System;
using System.Collections;

namespace RWE.SmartHome.SHC.wMBusProtocol;

public class DateTimeConverter
{
	public static DateTime ToDate(byte[] dataBytes)
	{
		int day = 0x1F & dataBytes[0];
		int month = 0xF & dataBytes[1];
		int num = (0xE0 & dataBytes[0]) >> 5;
		int num2 = (0xF0 & dataBytes[1]) >> 1;
		int num3 = 2000 + num + num2;
		return new DateTime(2000 + num3, month, day);
	}

	public static DateTime ToDateTime(byte[] dataBytes)
	{
		int minute = dataBytes[0] & 0x3F;
		int hour = dataBytes[1] & 0x1F;
		int day = dataBytes[2] & 0x1F;
		int month = dataBytes[3] & 0xF;
		int num = (0xE0 & dataBytes[2]) >> 5;
		int num2 = (0xF0 & dataBytes[3]) >> 1;
		int num3 = (0x60 & dataBytes[1]) >> 5;
		if (num3 == 0)
		{
			num3 = 1;
		}
		int year = 1900 + 100 * num3 + num + num2;
		return new DateTime(year, month, day, hour, minute, 0);
	}

	public static ushort ToDateValue(DateTime date)
	{
		ushort num = (ushort)date.Day;
		ushort num2 = (ushort)(date.Month << 8);
		BitArray bitArray = new BitArray(new int[1] { date.Year - 2000 });
		BitArray bitArray2 = new BitArray(32);
		bitArray2[5] = bitArray[0];
		bitArray2[6] = bitArray[1];
		bitArray2[7] = bitArray[2];
		bitArray2[12] = bitArray[3];
		bitArray2[13] = bitArray[4];
		bitArray2[14] = bitArray[5];
		bitArray2[15] = bitArray[6];
		int[] array = new int[1];
		bitArray2.CopyTo(array, 0);
		ushort num3 = (ushort)array[0];
		return (ushort)(num3 | num2 | num);
	}

	public static uint ToDateTimeValue(DateTime date)
	{
		uint num = (uint)(date.Day << 16);
		uint num2 = (uint)(date.Month << 24);
		uint minute = (uint)date.Minute;
		uint num3 = (uint)(date.Hour << 8);
		BitArray bitArray = new BitArray(new int[1] { date.Year - 2000 });
		BitArray bitArray2 = new BitArray(32);
		bitArray2[21] = bitArray[0];
		bitArray2[22] = bitArray[1];
		bitArray2[23] = bitArray[2];
		bitArray2[28] = bitArray[3];
		bitArray2[29] = bitArray[4];
		bitArray2[30] = bitArray[5];
		bitArray2[31] = bitArray[6];
		int[] array = new int[1];
		bitArray2.CopyTo(array, 0);
		uint num4 = (uint)array[0];
		return num4 | num2 | num | num3 | minute;
	}
}
